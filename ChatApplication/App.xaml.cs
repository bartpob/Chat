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
using System.Security.Cryptography;
namespace ChatApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly UDPConnectionProvider _udpConnectionProvider;
        private readonly MessageDispatcher _messageDispatcher;
        private readonly RSAParameters _rsaParameters;
        public App()
        {
            using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                _rsaParameters = rsa.ExportParameters(true);
            }

            _udpConnectionProvider = new(_rsaParameters);
            _messageDispatcher = new(_udpConnectionProvider, _rsaParameters);
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
