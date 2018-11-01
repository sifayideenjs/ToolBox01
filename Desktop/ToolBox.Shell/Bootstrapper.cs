using ToolBox.Common;
using ToolBox.Shell.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToolBox.Shell
{
    public class UICompositionBootstrapper : UnityBootstrapper
    {
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(ToolBox.Dashboard.DashboardModule));
            moduleCatalog.AddModule(typeof(ToolBox.COE.COEModule));
            //moduleCatalog.AddModule(typeof(ToolBox.Backbone.Tool.ToolModule));
            moduleCatalog.AddModule(typeof(ToolBox.ConsoleTool.ConsoleToolModule));
        }

        protected override DependencyObject CreateShell()
        {
            ShellView view = this.Container.TryResolve<ShellView>();
            return view;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }
    }
}
