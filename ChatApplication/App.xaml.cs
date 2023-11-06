using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ChatApplication.Models;
using ChatApplication.ViewModels;
using Connection;
using Connection.UDP;
using Connection.Datagrams;
using System.Windows.Navigation;

namespace ChatApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly UDPConnectionProvider _udpConnectionProvider;
        private readonly MessageDispatcher _messageDispatcher;
        public App()
        {
            _udpConnectionProvider = new();
            _messageDispatcher = new(_udpConnectionProvider);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_messageDispatcher)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _messageDispatcher.SendState(UserStatus.Offline);
            base.OnExit(e);
        }
    }
}
