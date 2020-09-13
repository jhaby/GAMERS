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
        List<UsersRank> userRank;

        public Dashboard(UserData Userinfo,StatusModel stat,ConnService signalService,List<AgentsModel> AgentsList)
        {
            InitializeComponent();
            
            status = stat;

            User = Userinfo;
            this.DataContext = User;

           userRank = new List<UsersRank>();

            for (var i = 0; i < AgentsList.Count; i++)
            {
                if (AgentsList[i].Status == "Status: Active")
                {
                    userRank.Add(new UsersRank
                    {
                        UserID = AgentsList[i].UserId,
                        Position = AgentsList[i].Rank,
                        ConnId = "null"
                    });
                }
            }

            signalService.NewUser += (string obj) =>
            {
                Task.Run(async () =>
                {
                    await signalService.ConnectionSync(User.UserId, userRank);
                });
            };

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
            agentslist.ItemsSource = AgentsList;

            signalService.Ranking += (List<UsersRank> obj) =>
            {
                int index = obj.FindIndex(ag => ag.UserID.Equals(User.UserId, StringComparison.Ordinal));
                User.Rank = obj[index].Position.ToString();
                
                for (var i=0;i< AgentsList.Count;i++){
                    int pos = obj.FindIndex(ag => ag.UserID.Equals(AgentsList[i].UserId, StringComparison.Ordinal));
                    AgentsList[i].Rank = obj[pos].Position;
                }
                for (var i = 0; i < userRank.Count; i++)
                {
                    int pos = obj.FindIndex(ag => ag.UserID.Equals(userRank[i].UserID, StringComparison.Ordinal));
                    userRank[i].Position = obj[pos].Position;
                }
            };



            StatusToggle.Unchecked += (o, e) =>
            {
                status.UserId = User.UserId;
                status.Status = "Unavailable";
                Task.Run(async () => {
                    await signalService.SendStatus(status);
                    await Helpers.UpdateStatus(status.Status, status.UserId);

                    int index = userRank.FindIndex(ag => ag.UserID.Equals(User.UserId, StringComparison.Ordinal));
                    userRank.RemoveAt(index);
                    
                    await signalService.ReorderList("remove",index, userRank);
                    User.Rank = "";
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

                    userRank.Add(new UsersRank
                    {
                        UserID = User.UserId,
                        ConnId = ""
                    });
                    await signalService.ReorderList("add", userRank.Count, userRank);

                });

                User.Status = status.Status;
                
            };

            if (User.Status == "Status: Active")
                StatusToggle.IsChecked = true;
            else
                StatusToggle.IsChecked = false;

            

        }

        

    }

    
}
