using ChatApplication.Commands;
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

namespace ChatApplication.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        private readonly MessageDispatcher _messageDispatcher;

        private User? _selectedUser;
        private string? _message;
        private ObservableCollection<User> _users;
        private ObservableCollection<Message> _usersMessages;

        public ICommand SendMessageCommand { get; }
        public IList<User>? Users => _users;
        public IList<Message>? Messages => _usersMessages;


        public User? SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                _usersMessages = new(_selectedUser?.Messages ?? new());
                _usersMessages.CollectionChanged += OnCollectionChanged;
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
            _usersMessages.Add(new Models.Message(_message, MessageType.Outgoing, DateTime.Now));
            _message = "";
           OnPropertyChanged(nameof(Message));
        }

        private bool CanSendMessage(object? obj) => !String.IsNullOrEmpty(_message);

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    foreach(Message item in e.NewItems!)
                    {
                        _selectedUser?.Messages?.Add(item);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ReceivedUserStateEventHandler(object? sender, ReceivedDataEventArgs a)
        {
            UserStateDatagram userState = (UserStateDatagram)a.Datagram;

            switch(userState.Status)
            {
                case UserStatus.Online:
                    _users.Add(new User(userState.HostName, userState.IPAddr, new()));
                    break;
                case UserStatus.Offline:
                    _users.Where(n => n.Address == userState.IPAddr).FirstOrDefault()!.Status = userState.Status;
                    break;
            }
        }

        public MainViewModel(MessageDispatcher messageDispatcher)
        {
            SendMessageCommand = new RelayCommand(SendMessageCommandHandler, CanSendMessage);
            List<Message> messages = new();
            _messageDispatcher = messageDispatcher;
            _usersMessages = new(_selectedUser?.Messages ?? new());
            _usersMessages.CollectionChanged += OnCollectionChanged;
            _users = new();
            _messageDispatcher.ReceivedUserState += ReceivedUserStateEventHandler;
        }
    }
}
