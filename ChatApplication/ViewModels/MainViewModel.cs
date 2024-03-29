﻿using ChatApplication.Commands;
using ChatApplication.Models;
using Connection;
using Connection.Datagrams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security.Cryptography;
namespace ChatApplication.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        private readonly MessageDispatcher _messageDispatcher;

        private User? _selectedUser;
        private string? _message;
        private ObservableCollection<User> _users;
        public ICommand SendMessageCommand { get; }
        public IList<User>? Users => _users;
        public IList<Message>? Messages => _selectedUser?.Messages;


        public User? SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                OnPropertyChanged(nameof(Messages));
            }
        }

        public string? Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private void SendMessageCommandHandler(object? obj)
        {
            _selectedUser!.Messages.Add(new Message(_message!, MessageType.Outgoing,  DateTime.Now));
            RSAParameters publicKey = new RSAParameters();

            publicKey.Modulus = _selectedUser.Modulus;
            publicKey.Exponent = _selectedUser.Exponent;

            _messageDispatcher.Send(new MessageDatagram(_messageDispatcher.IPAddr, _message!, DateTime.Now), _selectedUser.Address, publicKey);
            _message = "";
           OnPropertyChanged(nameof(Message));
        }

        private bool CanSendMessage(object? obj) => !String.IsNullOrEmpty(_message) && _selectedUser != null && _selectedUser.Status == UserStatus.Online;

        private void ReceivedUserStateEventHandler(object? sender, ReceivedDataEventArgs e)
        {
            UserStateDatagram userState = (UserStateDatagram)e.Datagram;
            var user = _users.FirstOrDefault(x => x.Address.ToString() == userState.IPAddr.ToString());
            
            if(user != null)
            {
                if(userState.Status == UserStatus.Online)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _users[_users.IndexOf(user)] = new User(userState.HostName, userState.IPAddr, user.Messages, userState.Modulus, userState.Exponent);
                    });
                }
                else
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _users[_users.IndexOf(user)] = new User(user.Name, user.Address, user.Messages, user.Modulus, user.Exponent, UserStatus.Offline);
                    });
                }
            }
            else
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                { 
                    _users.Add(new User(userState.HostName, userState.IPAddr, new(), userState.Modulus, userState.Exponent)); 
                });
                
            }
        }

        private void ReceivedMessageEventHandler(object? sender, ReceivedDataEventArgs e)
        {
            MessageDatagram message = (MessageDatagram)e.Datagram;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                _users.Where(user => user.Address.ToString() == message.FromIPAddr.ToString())
                .FirstOrDefault()!
                .Messages
                .Add(new Message(
                    message.Text, MessageType.Incoming, message.Date)
                );
            });
        }

        public MainViewModel(MessageDispatcher messageDispatcher)
        {
            _users = new();
            SendMessageCommand = new RelayCommand(SendMessageCommandHandler, CanSendMessage);
            _messageDispatcher = messageDispatcher;
            _messageDispatcher.ReceivedUserState += ReceivedUserStateEventHandler;
            _messageDispatcher.ReceivedMessage += ReceivedMessageEventHandler;
            _messageDispatcher.Run();
        }
    }
}
