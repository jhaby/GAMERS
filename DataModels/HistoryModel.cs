using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace GAMERS_TECH
{
    public class HistoryModel : INotifyPropertyChanged
    {
        private string dateTime;
        private string caseId;
        private string description;
        private string location;
        private string village;
        private string category;
        private string vHTCode;
        private string agentId;
        private string eMTCode;
        private string duration;
        private string visitationRef;
        private string status;

        public string DateTime { get => dateTime; set { dateTime = value; OnpropertyChange("DateTime"); } }
        public string CaseId { get => caseId; set { caseId = value; OnpropertyChange("CaseId"); } }
        public string Description { get => description; set { description = value; OnpropertyChange("Description"); } }
        public string Location { get => location; set { location = value; OnpropertyChange("Location"); } }
        public string Village { get => village; set { village = value; OnpropertyChange("Village"); } }
        public string Category { get => category; set { category = value; OnpropertyChange("Category"); } }
        public string VHTCode { get => vHTCode; set { vHTCode = value; OnpropertyChange("VHTCode"); } }
        public string AgentId { get => agentId; set { agentId = value; OnpropertyChange("AgentId"); } }
        public string EMTCode { get => eMTCode; set { eMTCode = value; OnpropertyChange("EMTCode"); } }
        public string Duration { get => duration; set { duration = value; OnpropertyChange("Duration"); } }
        public string VisitationRef { get => visitationRef; set { visitationRef = value; OnpropertyChange("VisitationRef"); } }
        public string Status { get => status; set { status = value; OnpropertyChange("Status"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChange(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
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
                return "Status: " + status;
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
