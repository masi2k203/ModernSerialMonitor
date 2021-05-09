using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ModernSerialMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var w = new ModernSerialMonitor.Views.MainShellView();
            var vm = new ModernSerialMonitor.ViewModels.MainShellViewModel();

            w.DataContext = vm;
            w.Show();
        }
    }
}
