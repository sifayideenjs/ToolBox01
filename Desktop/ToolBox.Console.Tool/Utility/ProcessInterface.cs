using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace ToolBox.ConsoleTool.Utility
{
    public delegate void ProcessEventHanlder(object sender, ProcessEventArgs args);

    public class ProcessInterface
    {
        private Process process;

        private StreamWriter inputWriter;

        private TextReader outputReader;

        private TextReader errorReader;

        private BackgroundWorker outputWorker = new BackgroundWorker();

        private BackgroundWorker errorWorker = new BackgroundWorker();

        private string processFileName;

        private string processArguments;

        public event ProcessEventHanlder OnProcessOutput;

        public event ProcessEventHanlder OnProcessError;

        public event ProcessEventHanlder OnProcessInput;

        public event ProcessEventHanlder OnProcessExit;

        public ProcessInterface()
        {
            //  Configure the output worker.
            outputWorker.WorkerReportsProgress = true;
            outputWorker.WorkerSupportsCancellation = true;
            outputWorker.DoWork += new DoWorkEventHandler(outputWorker_DoWork);
            outputWorker.ProgressChanged += new ProgressChangedEventHandler(outputWorker_ProgressChanged);

            //  Configure the error worker.
            errorWorker.WorkerReportsProgress = true;
            errorWorker.WorkerSupportsCancellation = true;
            errorWorker.DoWork += new DoWorkEventHandler(errorWorker_DoWork);
            errorWorker.ProgressChanged += new ProgressChangedEventHandler(errorWorker_ProgressChanged);
        }

        public bool IsProcessRunning
        {
            get
            {
                try
                {
                    return (process != null && process.HasExited == false);
                }
                catch
                {
                    return false;
                }
            }
        }

        public Process Process
        {
            get { return process; }
        }

        public string ProcessFileName
        {
            get { return processFileName; }
        }

        public string ProcessArguments
        {
            get { return processArguments; }
        }

        void outputWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  We must be passed a string in the user state.
            if (e.UserState is string)
            {
                //  Fire the output event.
                FireProcessOutputEvent(e.UserState as string);
            }
        }

        void outputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (outputWorker.CancellationPending == false)
            {
                //  Any lines to read?
                int count = 0;
                char[] buffer = new char[1024];
                do
                {
                    StringBuilder builder = new StringBuilder();
                    count = outputReader.Read(buffer, 0, 1024);
                    builder.Append(buffer, 0, count);
                    outputWorker.ReportProgress(0, builder.ToString());
                } while (count > 0);

                System.Threading.Thread.Sleep(200);
            }
        }

        void errorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  The userstate must be a string.
            if (e.UserState is string)
            {
                //  Fire the error event.
                FireProcessErrorEvent(e.UserState as string);
            }
        }

        void errorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (errorWorker.CancellationPending == false)
            {
                //  Any lines to read?
                int count = 0;
                char[] buffer = new char[1024];
                do
                {
                    StringBuilder builder = new StringBuilder();
                    count = errorReader.Read(buffer, 0, 1024);
                    builder.Append(buffer, 0, count);
                    errorWorker.ReportProgress(0, builder.ToString());
                } while (count > 0);

                System.Threading.Thread.Sleep(200);
            }
        }

        public void StartProcess(string fileName, string arguments)
        {
            //  Create the process start info.
            var processStartInfo = new ProcessStartInfo(fileName, arguments);

            //  Set the options.
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;

            //  Specify redirection.
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            //  Create the process.
            process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;
            process.Exited += new EventHandler(currentProcess_Exited);

            //  Start the process.
            try
            {
                bool processStarted = process.Start();
            }
            catch (Exception e)
            {
                //  Trace the exception.
                Trace.WriteLine("Failed to start process " + fileName + " with arguments '" + arguments + "'");
                Trace.WriteLine(e.ToString());
                return;
            }

            //  Store name and arguments.
            processFileName = fileName;
            processArguments = arguments;

            //  Create the readers and writers.
            inputWriter = process.StandardInput;
            outputReader = TextReader.Synchronized(process.StandardOutput);
            errorReader = TextReader.Synchronized(process.StandardError);

            //  Run the workers that read output and error.
            outputWorker.RunWorkerAsync();
            errorWorker.RunWorkerAsync();
        }

        public void StopProcess()
        {
            //  Handle the trivial case.
            if (IsProcessRunning == false)
                return;

            //  Kill the process.
            process.Kill();
        }

        void currentProcess_Exited(object sender, EventArgs e)
        {
            //  Fire process exited.
            FireProcessExitEvent(process.ExitCode);

            //  Disable the threads.
            outputWorker.CancelAsync();
            errorWorker.CancelAsync();
            inputWriter = null;
            outputReader = null;
            errorReader = null;
            process = null;
            processFileName = null;
            processArguments = null;
        }

        private void FireProcessOutputEvent(string content)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessOutput;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(content));
        }

        private void FireProcessErrorEvent(string content)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessError;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(content));
        }

        private void FireProcessInputEvent(string content)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessInput;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(content));
        }

        private void FireProcessExitEvent(int code)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessExit;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(code));
        }

        public void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                inputWriter.WriteLine(input);
                inputWriter.Flush();
            }
        }
    }
}
