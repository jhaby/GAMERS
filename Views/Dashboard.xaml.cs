using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
    public partial class Dashboard : Page
    {
        
        StatusModel status;
        private readonly string uri;
        private UserData User;
        private HttpClient _client;
        UsersRank userRank;
        private List<AgentsModel> AgentsList;

        public Dashboard(UserData Userinfo,StatusModel stat,ConnService signalService,string uri)
        {
            InitializeComponent();
            
            status = stat;
            this.uri = uri;
            User = Userinfo;
            this.DataContext = User;
            _client = new HttpClient();

            userRank = new UsersRank();

            LoadAgents();

            signalService.StatusReceived += (StatusModel obj) =>
            {
                int index = AgentsList.FindIndex(agent => agent.UserId.Equals(obj.UserId, StringComparison.Ordinal));
                if (obj.UserId == status.UserId)
                {
                    status.Status = obj.Status;
                    User.Status = obj.Status;
                    AgentsList[index].Status = obj.Status;
                    if (AgentsList[index].Status == "Active")
                        AgentsList[index].Background = Colors.LightGreen;
                    else
                        AgentsList[index].Background = Colors.LightPink;
                }
                else
                {
                    
                    AgentsList[index].Status = obj.Status;
                    if (AgentsList[index].Status == "Active")
                        AgentsList[index].Background = Colors.LightGreen;
                    else
                        AgentsList[index].Background = Colors.LightPink;
                }
            };
            

            signalService.Ranking += (List<UsersRank> obj) =>
            {
                

                int index = obj.FindIndex(ag => ag.UserID.Equals(User.UserId, StringComparison.Ordinal));
                User.Rank = obj[index].Position.ToString();
                
                for (var i=0;i< AgentsList.Count;i++){
                    int pos = obj.FindIndex(ag => ag.UserID.Equals(AgentsList[i].UserId, StringComparison.Ordinal));
                    AgentsList[i].Rank = obj[pos].Position;
                }
            };



            StatusToggle.Unchecked += (o, e) =>
            {
                status.UserId = User.UserId;
                status.Status = "Unavailable";
                Task.Run(async () => {
                    await signalService.SendStatus(status);
                    await signalService.UpdateStatus(status.Status, status.UserId);

                    userRank.Position = 0;
                    await signalService.ReorderList("remove", User.UserId);
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
                    await signalService.UpdateStatus("Active", status.UserId);
                    
                    
                    await signalService.ReorderList("add", User.UserId);

                });

                User.Status = status.Status;
                
            };

            if (User.Status == "Active")
                StatusToggle.IsChecked = true;
            else
                StatusToggle.IsChecked = false;

            

        }
        private async void LoadAgents()
        {
            var response = await _client.GetAsync(uri + "/loadagents");
            var result = await response.Content.ReadAsStringAsync();
            AgentsList = JsonConvert.DeserializeObject<List<AgentsModel>>(result);

            foreach (var ag in AgentsList)
            {
                if (ag.Status == "Active")
                {

                     ag.Background = Colors.LightGreen;
                    
                }
                else
                    ag.Background = Colors.LightPink;
            }

            agentslist.ItemsSource = AgentsList;

        }


    }

    
}
