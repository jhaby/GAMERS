using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GAMERS_TECH
{
    public class ResponseViewModel : INotifyPropertyChanged
    {
        
        private string fullname;
        private string phone;
        private string village;
        private string kin_phone;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

        public string Fullname { get => fullname; set { fullname = value; OnPropertyChanged("Fullname"); } }
        public string Phone { get => phone; set { phone = value; OnPropertyChanged("Phone"); } }
        public string VHTVillage { get => village; set { village = value; OnPropertyChanged("VHTVillage"); } }
        public string Kin_phone { get => kin_phone; set { kin_phone = value; OnPropertyChanged("Kin_phone"); } }

       

    }


    public class EMTInfo 
    {
        public string TeamID { get; set; }
        public string Phone { get; set; }
        public string Transporter { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int Rank { get; set; }

        
    }

    public class MedicalInfo
    {
        public string ID { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public int Rank { get; set; }

        
    }
}
