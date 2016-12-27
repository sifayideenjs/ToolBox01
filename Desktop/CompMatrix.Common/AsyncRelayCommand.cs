﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToolBox.Common
{
    public class AsyncRelayCommand : ICommand
    {
        private Func<object, Task> _action;
        private Task _task;

        public AsyncRelayCommand(Func<object, Task> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return _task == null || _task.IsCompleted;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            _task = _action(parameter);
            OnCanExecuteChanged();
            await _task;
            OnCanExecuteChanged();
        }

        private void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
