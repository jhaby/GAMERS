using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GAMERS_TECH
{
    public class UserData : INotifyPropertyChanged
    {
        private string status;
        private string language;
        private string rank;

        public string UserId { get; set; }
        public string Username { get; set; }
        public string Photo { get; set; }
        public string AccessType { get; set; }
        public int TotalAlerts { get; set; }
        public int HandledAlerts { get; set; }
        public int MissedAlerts { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Language { get => "Language: " + language; set => language = value; }

        public string Rank { get => rank;

            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        }

        public string Status
        {
            get
            {
                return "Status: " + status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }


        public string Fullname
        {
            get
            {
                return Firstname + " " + Surname;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

    }

    public class PersonelInfo
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Village { get; set; }
        public string Kin { get; set; }
    }

    public class UsersRank
    {
        public string UserID { get; set; }
        public int Position { get; set; }
        public string ConnId { get; set; }

    }
}
