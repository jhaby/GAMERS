using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GAMERS_TECH
{
    public class ConnService
    {
        private readonly HubConnection _connection;
        public event Action<StatusModel> StatusReceived;
        public ConnService(HubConnection connection)
        {
            this._connection = connection;

            _connection.On<StatusModel>("ReceiveStatus", (status) => StatusReceived?.Invoke(status));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }
        public async Task SendStatus(StatusModel status)
        {
            await _connection.SendAsync("SendStatus", status);
        }
    }

    public class StatusModel: INotifyPropertyChanged
    {
        private string _userId { get; set; }
        private string _status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string member = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
        
        public string UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                if(value != _userId)
                {
                    this._userId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    this._status = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
