using GAMERS_TECH.Views;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GAMERS_TECH
{
    
    public partial class Home : Window
    {
        UserData User;
        HubConnection connection;
        ConnService signalService;
        PersonnelInfoViewModel persons;
        StatusModel stat;
        public List<AgentsModel> AgentsList;
        List<UsersRank> Usersrank;
        private int count=30;
        private DispatcherTimer timer;

        public  Home(UserData Userinfo)
        {
            InitializeComponent();
            User = Userinfo;
            Loaded += Home_Loaded;
            string NameChip = "Hi, "+ User.Firstname+" "+ User.Surname;

            Usersrank = new List<UsersRank>();
            stat = new StatusModel
            {
                UserId = User.UserId,
                Status = "Active"
            };
            string BaseUri = String.Format("http://{0}:{1}/updates", Environment.GetEnvironmentVariable("GamersServerUri"), Environment.GetEnvironmentVariable("CommPort"));
            connection = new HubConnectionBuilder()
               .WithUrl(BaseUri)
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                 await ServerConnect();
                await signalService.SendStatus(stat);
            };
           
            CasesPage.AlertEvent += CasesPage_AlertEvent;
           
            signalService = new ConnService(connection);
            signalService.StatusReceived += Cos_StatusReceived;
            

            Task.Run(async()=> await ServerConnect());

            LoadAgents();
            persons = new PersonnelInfoViewModel();
           
            signalService.DisconnectUser += (string obj) =>
              {
                  
                  Task.Run(async() =>
                  {
                      await signalService.SendStatus(stat);
                      await Helpers.UpdateStatus(stat.Status, stat.UserId);
                  });
              };
            LoadMap();

            HistoryPage.historyItem += (HistoryModel obj) =>
            {
                MainHolder.Visibility = Visibility.Visible;
                browser.Visibility = Visibility.Collapsed;
                Dispatcher.Invoke(() => MainHolder.NavigationService.Navigate(new CaseDetails(obj)));
            };

            

            this.Closing += MainWindow_Closing;
            

        }

        private void CasesPage_AlertEvent(UserData arg1, CasesModel arg2, List<CasesModel> arg3)
        {

            if (User.Rank == "1")
            {
                int index = arg3.FindIndex(agent => agent.CaseId.Equals(arg2.CaseId, StringComparison.Ordinal));
                Sender sender = new Sender()
                {
                    UserId = User.UserId,
                    CaseId = arg2.CaseId,
                    Response = "accept"
                };

                
                string[] details = { arg2.CaseId, User.UserId, arg3[index].Location, arg3[index].Village, arg3[index].VHTCode, arg3[index].DateTime.ToString(), arg3[index].Description };
                Dispatcher.Invoke(() => alertDialog.Visibility = Visibility.Visible);
                AcceptBtn.Click += delegate
                {
                    Dispatcher.Invoke(async () =>
                    {
                        alertDialog.Visibility = Visibility.Collapsed;
                        ResponseView.Visibility = Visibility.Visible;
                        browser.Visibility = Visibility.Collapsed;
                        Response.NavigationService.Navigate(new ResponsePage(User,signalService,details));
                        

                        await CasesPage.SendHandledAlert(sender,details);
                    });
                };
                Decline.Click += delegate
                {
                    alertDialog.Visibility = Visibility.Collapsed;
                };
                Dispatcher.Invoke(() =>
                {
                    timer = new DispatcherTimer(DispatcherPriority.Background);
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                });

                

            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            count -= 1;
            counter.Text = count.ToString() + "s";
            if (count == 0)
            {
                timer.Stop();
                alertDialog.Visibility = Visibility.Collapsed;
            }
               
        }

        
        private async void LoadMap()
        {
            await Task.Delay(500);
            browser.IsJavaScriptEnabled = true;
            browser.Navigate("https://www.google.com/maps");

            browser.DOMContentLoaded += delegate
             {
                 loadingMap.Visibility = Visibility.Collapsed;
             };
        }

        public void DragEvent(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        protected override void OnClosed(EventArgs e)
        {
            browser.Dispose();
            base.OnClosed(e);
        }


        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
          Task.Run(() =>
             {
                 Dispatcher.Invoke(() =>
                 {
                     signalService.Disconnect();
                 });

             });
            
            stat.Status = "Unavailable";
            await signalService.SendStatus(stat);
            await Helpers.UpdateStatus(stat.Status, stat.UserId);

        }

        

        public async Task ServerConnect()
        {
            try
            {
                await signalService.Connect();
                await signalService.SendStatus(stat);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void LoadAgents()
        {
            AgentsList = Helpers.LoadAgents().Result;
            
            foreach (var ag in AgentsList)
            {
                if (ag.Status == "Status: Active")
                    ag.Background = Colors.LightGreen;
                else
                    ag.Background = Colors.LightPink;
            }
        }

        private void Cos_StatusReceived(StatusModel obj)
        {
            stat.Status = obj.Status;
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new Dashboard(User,stat,signalService,AgentsList));
        }

        private void DashboardClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new Dashboard(User,stat,signalService,AgentsList));
        }

        private void HisotryClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new HistoryPage(signalService));
        }

        private void PersonnelinfoClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(500);
            body.NavigationService.Navigate(new Personnelinfo(User,persons, signalService));
        }

        private void CasesClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new CasesPage(User, signalService));
            CasesPage.Respond += (string[] details) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ResponseView.Visibility = Visibility.Visible;
                    browser.Visibility = Visibility.Collapsed;
                    var rpage = new ResponsePage(User,signalService, details);
                    Response.NavigationService.Navigate(rpage);
                    

                    rpage.BtnClicked += (string obj) =>
                      {
                          switch (obj)
                          {
                              case "minimise":
                                  ResponseView.Visibility = Visibility.Collapsed;
                                  browser.Visibility = Visibility.Visible;
                                  break;
                          }
                      };
                });

            };

        }


        private void LibraryClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(600);
            body.NavigationService.Navigate(new LibraryPage());
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainHolder.Visibility = Visibility.Collapsed;
            browser.Visibility = Visibility.Visible;
        }
        
    }
}
