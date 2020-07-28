using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GAMERS_TECH
{
    public class PersonnelData : INotifyPropertyChanged
    {
        private string name;
        private string role;
        private string email;
        private string phone;
        private string filepath;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(value != name)
                {
                    this.name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                if (value != role)
                {
                    this.role = value;
                    OnPropertyChanged("Role");
                }
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                {
                    this.email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (value != phone)
                {
                    this.phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }
        public string Filepath
        {
            get
            {
                return filepath;
            }
            set
            {
                if (value != filepath)
                {
                    this.filepath = value;
                    OnPropertyChanged("Filepath");
                }
            }
        }
    }

    public class LoginDetails
    {
        public string Fullname { get; set; }
        public string Accesstype { get; set; }
        public int TotalAlerts { get; set; }
        public int HandledAlerts { get; set; }
        public int MissedAlerts { get; set; }
        public string Language { get; set; }
    }
}
