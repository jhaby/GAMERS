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
        public event Action<Sender> HandleEventReceived;
        public event Action<CasesModel> AlertReceived;
        public event Action<string,string,string> SendingSuccess;
        public event Action<ResponseViewModel> ProgressReportA;
        public event Action<EMTInfo> ProgressReportB;
        public event Action<MedicalInfo> ProgressReportC;
        public event Action<string, string> ResponseSuccess;
        public event Action<string> NewUser;
        public event Action<List<UsersRank>> Ranking;
        public event Action<string> DisconnectUser;
        public event Action<string> Test;

        public ConnService(HubConnection connection)
        {
            this._connection = connection;

            /* All signalR responses are received here and respective action invoke*/

            _connection.On<StatusModel>("ReceiveStatus", (status) => StatusReceived?.Invoke(status));
            _connection.On<Sender>("UpdateAlerts", (sender) => HandleEventReceived?.Invoke(sender));
            _connection.On<CasesModel>("AlertBroadcast", (sender) => AlertReceived?.Invoke(sender));
            _connection.On<string,string,string>("SMSSendingSuccess", (Sid,view,agent) => SendingSuccess?.Invoke(Sid,view,agent));
            _connection.On<string, string>("SMSResponseSuccess", (Sid, response) => ResponseSuccess?.Invoke(Sid, response));
            _connection.On<string>("NewUserConnected", (connid) => NewUser?.Invoke(connid));
            _connection.On<List<UsersRank>>("UsersRank", (rank) => Ranking?.Invoke(rank));
            _connection.On<string>("DisconnectUser", (connId) => DisconnectUser?.Invoke(connId));
            _connection.On<ResponseViewModel>("ProgressReportA", (obj) => ProgressReportA?.Invoke(obj));
            _connection.On<EMTInfo>("ProgressReportB", (obj) => ProgressReportB?.Invoke(obj));
            _connection.On<MedicalInfo>("ProgressReportC", (obj) => ProgressReportC?.Invoke(obj));
            _connection.On<string>("Test", (obj) => Test?.Invoke(obj));
        }


        /* All below are methods broadcasted to the server via signalR*/
        public async Task Connect()
        {
            await _connection.StartAsync();
        }
        public void  Disconnect()
        {
            _connection.StopAsync();
        }
        public async Task SendStatus(StatusModel status)
        {
            await _connection.SendAsync("SendStatus", status);
        }
        public async Task HandleAlert(Sender s, CasesModel cases)
        {
            await _connection.SendAsync("HandleAlertBroadcast", s,cases);
        }
        public async Task SendSMS(SMSDetails sms)
        {
            await _connection.SendAsync("SendSMS", sms);
        }
        public async Task ConnectionSync(string UserId, List<UsersRank> UsersList)
        {
            await _connection.SendAsync("NewUserStatus", UserId, UsersList);
        }
        public async Task ReorderList(string order,int pos,List<UsersRank> UsersList)
        {
            await _connection.SendAsync("ReorderRanks",order,pos, UsersList);
        }
        public async Task ReinstateCase(CasesModel alert)
        {
            await _connection.SendAsync("Reinstate", alert);
        }
        public async Task ResponseCalls(string type, UserData param1, List<string> param2)
        {
            if(type == "VHT")
            {
                await _connection.SendAsync("VHTResponse", param2);
            }
            else if (type == "EMT")
            {
                await _connection.SendAsync("EMTResponse", param2);
            }
            else if(type == "MED")
            {
                await _connection.SendAsync("MEDResponse", param2);
            }
            
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
    public class Sender
    {
        public string UserId { get; set; }
        public string CaseId { get; set; }
        public string Response { get; set; }
    }
    public class SMSDetails
    {
        public string Message { get; set; }
        public string Number { get; set; }
        public string View { get; set; }
        public string Sender { get; set; }
    }
}
