using ChatApplication.Commands;
using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApplication.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        private User? _selectedUser;
        private string? _message;
        private ObservableCollection<User>? _users;

        public ICommand SendMessageCommand { get; }
        public IEnumerable<User>? Users => _users;
        public IEnumerable<Message>? Messages => _selectedUser.Messages;


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
            _selectedUser.Messages.Add(new Models.Message(_message, MessageType.Outgoing, DateTime.Now));
            OnPropertyChanged(nameof(SelectedUser));
        }

        private bool CanSendMessage(object? obj) => !String.IsNullOrEmpty(_message);

        public MainViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessageCommandHandler, CanSendMessage);
            List<Message> messages = new();
            messages.Add(new Message("Witam", MessageType.Outgoing, DateTime.Parse("23/07/2023 13:27")));
            messages.Add(new Message("Dzień dobry", MessageType.Incoming, DateTime.Parse("23/07/2023 13:27")));
            messages.Add(new Message("Co słychać?", MessageType.Outgoing, DateTime.Parse("23/07/2023 13:27")));
            messages.Add(new Message("Aaa wszystko git a u ciebie?", MessageType.Incoming, DateTime.Parse("23/07/2023 13:27")));
            messages.Add(new Message("A też, dzięki że pytasz;laksjnd;alskdnas;lkdnas;lkdnas;lkdnas;lkdsan;kldasnd;laskndasn;ldsankd;aslkndas;lkdsan;asndlksandas;lkdnas;ldnas;lkdnsa;lkdsa" +
                ";aldksjndas;lkndas;lkdnsakl;dnaskl;dnas;knldasnkldasnkl;" +
                "da/klsndsa;lkdnsa;dnsa;lkdnas;lkdnas;lkdnas;lkdnas;kldnas;lkdnsa" +
                "d;laskndsa;lkndsa;lkdnas;lkdnsal;kdnsalk;dnsal;kndas;lkndas;lkndsakl;dnas", MessageType.Outgoing, DateTime.Parse("23/07/2023 13:27")));

            _users = new();
            _users.Add(new User("alfonso", IPAddress.Parse("192.168.1.1"), new(messages)));
            _users.Add(new User("Bart", IPAddress.Parse("192.168.1.1"), null));
            _users.Add(new User("Bart", IPAddress.Parse("192.168.1.1"), null));
            _users.Add(new User("Bart", IPAddress.Parse("192.168.1.1"), null, UserStatus.Offline));

            _selectedUser = _users.ElementAt(0);
        }
    }
}
