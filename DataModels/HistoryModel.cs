using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace GAMERS_TECH
{
    public class HistoryModel 
    {
        public string DateTime { get; set; }
        public string CaseId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Village { get; set; }
        public string Category { get; set; }
        public string VHTCode { get; set; }
        public string AgentId { get; set; }
        public string EMTCode { get; set; }
        public string Duration { get; set; }
        public string VisitationRef { get; set; }
        public string Status { get; set; }
        public string VHTName { get; set; }
        public string VHTPhone { get; set; }
        public string EMTName { get; set; }
        public string EMTPhone { get; set; }
        public string CHWCode { get; set; }
        public string CHWName { get; set; }
        public string CHWPhone { get; set; }
        public string Notes { get; set; }
    }

    public class AgentsModel : INotifyPropertyChanged
    {
        private string username;
        private string status;
        private string photo;
        private string userId;
        private Color background;
        private int rank;

        public int Rank { get => rank;

            set
            {
                rank = value;
                OnpropertyChange("Rank");
            }
        }
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                OnpropertyChange("UserId");
            }
        }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnpropertyChange("Username");
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
                status = value;
                OnpropertyChange("Status");
            }
        }
        public string Photo
        {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
                OnpropertyChange("Photo");
            }
        }
        public Color Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                OnpropertyChange("Background");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChange(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
