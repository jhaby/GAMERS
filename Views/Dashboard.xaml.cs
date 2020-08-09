using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        
        StatusModel status;
        private UserData User;
        private List<AgentsModel> AgentsList;

        public Dashboard(UserData Userinfo,StatusModel stat,ConnService signalService)
        {
            InitializeComponent();
            
            status = stat;

            User = Userinfo;
            this.DataContext = User;

            signalService.StatusReceived += (StatusModel obj) =>
            {
                int index = AgentsList.FindIndex(agent => agent.UserId.Equals(obj.UserId, StringComparison.Ordinal));
                if (obj.UserId == status.UserId)
                {
                    status.Status = obj.Status;
                    User.Status = obj.Status;
                    AgentsList[index].Status = obj.Status;
                    if (AgentsList[index].Status == "Status: Active")
                        AgentsList[index].Background = Colors.LightGreen;
                    else
                        AgentsList[index].Background = Colors.LightPink;
                }
                else
                {
                    
                    AgentsList[index].Status = obj.Status;
                    if (AgentsList[index].Status == "Status: Active")
                        AgentsList[index].Background = Colors.LightGreen;
                    else
                        AgentsList[index].Background = Colors.LightPink;
                }
            };
            LoadHistory();
            LoadAgents();
            StatusToggle.Unchecked += (o, e) =>
            {
                status.UserId = User.UserId;
                status.Status = "Unavailable";
                Task.Run(async () => {
                    await signalService.SendStatus(status);
                    await Helpers.UpdateStatus(status.Status, status.UserId);
                });
                User.Status = status.Status;
                
                
            };

            StatusToggle.Checked += (o, e) =>
            {
                status.UserId = User.UserId;
                status.Status = "Active";
                Task.Run(async () => {
                    await signalService.SendStatus(status);
                    await Helpers.UpdateStatus(status.Status, status.UserId);
                });

                User.Status = status.Status;
                
            };

            if (User.Status == "Status: Active")
                StatusToggle.IsChecked = true;
            else
                StatusToggle.IsChecked = false;

        }

        private void LoadAgents()
        {
           AgentsList = Helpers.LoadAgents().Result;
            agentslist.ItemsSource = AgentsList;
            foreach(var ag in AgentsList)
            {
                if(ag.Status == "Status: Active")
                   ag.Background = Colors.LightGreen;
                else
                   ag.Background = Colors.LightPink;
            }
        }

        private void LoadHistory()
        {
            List<HistoryModel> _history = new List<HistoryModel>();

            _history = Helpers.LoadHistoryAsync("dashboard").Result;

            history.ItemsSource = _history;
            
        }
    }

    
}
