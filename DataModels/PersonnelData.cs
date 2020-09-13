using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace GAMERS_TECH
{
    public class PersonnelData : INotifyPropertyChanged
    {
        private string _userId;
        private string fname;
        private string name;
        private string sname;
        private string role;
        private string email;
        private string phone;
        private string photoPath;
        private string status;
        private string message;
        private ICommand sendMessageCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (value != message)
                {
                    this.message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        public string UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                if (value != _userId)
                {
                    this._userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }
        public string Name
        {
            get
            {
                return fname + " " + sname;
            }
            set
            {
                name = fname + " " + sname;
                OnPropertyChanged("Name");

            }
        }

        public string Firstname
        {
            get
            {
                return fname;
            }
            set
            {
                if (value != fname)
                {
                    this.fname = value;
                    OnPropertyChanged("Firstname");
                }
            }
        }
        public string Surname
        {
            get
            {
                return sname;
            }
            set
            {
                if (value != sname)
                {
                    this.sname = value;
                    OnPropertyChanged("Surname");
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
        public string PhotoPath
        {
            get
            {
                return photoPath;
            }
            set
            {
                if (value != photoPath)
                {
                    this.photoPath = value;
                    OnPropertyChanged("PhotoPath");
                }
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    this.status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                if (sendMessageCommand == null)
                    sendMessageCommand = new RespondCommand(ExceuteMethod);
                return sendMessageCommand;
            }
        }


        private async void ExceuteMethod(object obj)
        {

            SMSDetails sms = new SMSDetails()
            {
                Message = message,
                Number = phone,
                View = "Personnel"
            };

            await Personnelinfo.SendSms(sms);
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
