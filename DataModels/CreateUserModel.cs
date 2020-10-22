using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GAMERS_TECH
{
    public class CreateUserData : INotifyPropertyChanged
    {
        private string prevUid;
        private string firstname;
        private string surname;
        private string othername;
        private string role;
        private string phone;
        private string altPhone;
        private string email;
        private string tribe;
        private string language;
        private string photoPath;

        public string PrevUid
        {
            get => prevUid;
            set
            {
                prevUid = value;
                OnPropertyChanged("PrevUid");
            }
        }
        public string Firstname
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged("Firstname");
            }
        }
        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Othername
        {
            get => othername;
            set
            {
                othername = value;
                OnPropertyChanged("Othername");
            }
        }
        public string Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public string AltPhone
        {
            get => altPhone;
            set
            {
                altPhone = value;
                OnPropertyChanged("AltPhone");
            }
        }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Tribe
        {
            get => tribe;
            set
            {
                tribe = value;
                OnPropertyChanged("Tribe");
            }
        }
        public string Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged("Language");
            }
        }
        public string PhotoPath { get => photoPath; 
            set
            {
                photoPath = value;
                OnPropertyChanged("PhotoPath");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
