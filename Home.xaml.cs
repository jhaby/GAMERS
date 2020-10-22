using GAMERS_TECH.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace GAMERS_TECH
{
    
    public partial class Home : Window
    {
        UserData User;
        private string uri;
        HubConnection connection;
        ConnService signalService;
        PersonnelInfoViewModel persons;
        private SchedulePage schedule;
        private PaymentPage paypage;
        private LoadingWindow loadingWindow;
        StatusModel stat;
        private int count=30;
        private DispatcherTimer timer;
        private static ConnService sService;

        public static event Action<string[]> Respond;
        public static Action<string> CloseLoadingWindow;
        public static Action CloseHomeWindow;

        public  Home(UserData Userinfo,string uri)
        {
            InitializeComponent();
            try
            {

                User = Userinfo;
                this.uri = uri;
                Loaded += Home_Loaded;
                string NameChip = "Hi, " + User.Firstname + " " + User.Surname;
                stat = new StatusModel
                {
                    UserId = User.UserId,
                    Status = "Active"
                };

                connection = new HubConnectionBuilder()
                   .WithUrl(uri + "/updates")
                   .Build();

                connection.Closed += async (error) =>
                {
                    await Task.Delay(5000);
                    await ServerConnect();
                    await signalService.SendStatus(stat);

                    CloseHomeWindow?.Invoke();
                };

                this.StateChanged += Home_StateChanged;


                signalService = new ConnService(connection);

                signalService.StatusReceived += Cos_StatusReceived;

                signalService.AlertReceived += CasesPage_AlertEvent;

                Task.Run(async () =>
                {
                    await ServerConnect();
                    
                });
                
                persons = new PersonnelInfoViewModel();
                schedule = new SchedulePage();
                paypage = new PaymentPage();
                loadingWindow = new LoadingWindow();

                signalService.NewUserSync += async(string id) =>
                 {
                     await signalService.ConnectionSync(User.UserId);
                 };

                signalService.DisconnectUser += (string obj) =>
                  {

                      Task.Run(async () =>
                      {
                          await signalService.SendStatus(stat);
                          await signalService.UpdateStatus(stat.Status, stat.UserId);
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

                sService = signalService;



                Respond += (string[] details) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        ResponseView.Visibility = Visibility.Visible;
                        browser.Visibility = Visibility.Collapsed;
                        var rpage = new ResponsePage(User, signalService, details);
                        Response.NavigationService.Navigate(rpage);


                        rpage.BtnClicked += (string obj) =>
                        {
                            switch (obj)
                            {
                                case "Close":
                                    ResponseView.Visibility = Visibility.Collapsed;
                                    browser.Visibility = Visibility.Visible;
                                    RestoreButton.Visibility = Visibility.Visible;
                                    break;
                                case "restart":
                                    CasesModel alert = new CasesModel()
                                    {
                                        DateTime = Convert.ToDateTime(details[5]),
                                        Location = details[2],
                                        VHTCode = details[4],
                                        Description = details[6],
                                        Village = details[3],
                                        Status = "ongoing",
                                        CaseId = details[0],
                                        Category = details[7]
                                        
                                    };
                                    Dispatcher.Invoke(async () => {
                                        await signalService.RestartResponse(details[0]);
                                    });
                                    break;
                                case "completed":
                                    Dispatcher.Invoke(() => {
                                        RadWindow.Confirm("Are you sure you want to mark case as completed?", async delegate
                                        {
                                            await signalService.CompletedCase(details[0]);
                                            RadWindow.Alert("This case has been marked as completed.", MarkCaseCompleted);
                                        });
                                        
                                    });
                                    break;
                            }
                        };
                    });

                };
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MarkCaseCompleted(object sender, WindowClosedEventArgs e)
        {
            ResponseView.Visibility = Visibility.Collapsed;
            browser.Visibility = Visibility.Visible;
        }

        private void Home_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                if(loadingWindow.IsVisible)
                    CloseLoadingWindow?.Invoke("close");
            }
            else
            {
                if(!browser.IsLoaded)
                    if(!loadingWindow.IsVisible)
                        loadingWindow.Show();
            }
        }

        private void CasesPage_AlertEvent(CasesModel arg2)
        {

            if (User.Rank == "1")
            {

                Sender sender = new Sender()
                {
                    UserId = User.UserId,
                    CaseId = arg2.CaseId,
                    Response = "accept"
                };


                string[] details = { arg2.CaseId, User.UserId, arg2.Location, arg2.Village, arg2.VHTCode, arg2.DateTime.ToString(), arg2.Description };
                Dispatcher.Invoke(() => alertDialog.Visibility = Visibility.Visible);
                AcceptBtn.Click += delegate
                {
                    Dispatcher.Invoke(async () =>
                    {
                        alertDialog.Visibility = Visibility.Collapsed;
                        ResponseView.Visibility = Visibility.Visible;
                        browser.Visibility = Visibility.Collapsed;
                        Response.NavigationService.Navigate(new ResponsePage(User, signalService, details));

                        timer.IsEnabled = false;
                        timer.Stop();
                        await SendHandledAlert(sender, details);
                    });
                };
                Decline.Click += delegate
                {
                    alertDialog.Visibility = Visibility.Collapsed;
                    timer.IsEnabled = false;
                    count = 30;
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
                timer.IsEnabled = false;
                count = 30;
                
            }

        }


        private async void LoadMap()
        {
            await Task.Delay(500);
            browser.IsJavaScriptEnabled = true;
            browser.Navigate("https://www.google.com/maps");
            
            browser.NavigationCompleted += delegate
            {
                CloseLoadingWindow?.Invoke("close");
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

        protected override void OnClosing(CancelEventArgs e)
        {
            signalService.Disconnect();
            
            CloseHomeWindow?.Invoke();

            base.OnClosing(e);
            
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {

            signalService.Disconnect();
                       
            CloseHomeWindow?.Invoke();

        }



        public async Task ServerConnect()
        {
            try
            {
                await signalService.Connect();
                await signalService.SendStatus(stat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        
        public static async Task SendHandledAlert(Sender sender, string[] details)
        {
            CasesModel cases = new CasesModel()
            {
                AgentId = details[1],
                CaseId = details[0],
                Location = details[2],
                Village = details[3],
                VHTCode = details[4],
                Description = details[6]
            };
            await sService.HandleAlert(sender, cases);

            if (sender.Response == "accept")
                Respond?.Invoke(details);

        }

        private void Cos_StatusReceived(StatusModel obj)
        {
            stat.Status = obj.Status;
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new Dashboard(User,stat,signalService,uri));
            loadingWindow.Show();
        }

        private void DashboardClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new Dashboard(User, stat, signalService,uri));
        }

        private void HisotryClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new HistoryPage(signalService));
        }

        private void PersonnelinfoClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(500);
            body.NavigationService.Navigate(new Personnelinfo(User, persons, signalService));

        }

        private void CasesClicked(object sender, RoutedEventArgs e)
        {
            container.MinWidth = Convert.ToDouble(400);
            body.NavigationService.Navigate(new CasesPage(User, signalService));

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

            ListView lv = sender as ListView;

            if (lv.SelectedIndex == 2)
            {
                mainBorder.Visibility = Visibility.Visible;
                browser.Visibility = Visibility.Collapsed;
                mainFrame.NavigationService.Navigate(schedule);
            }
            else if (lv.SelectedIndex == 6)
            {
                mainBorder.Visibility = Visibility.Visible;
                mainFrame.NavigationService.Navigate(paypage);
                browser.Visibility = Visibility.Collapsed;
            }
            else if (lv.SelectedIndex == 7)
            {
                mainBorder.Visibility = Visibility.Visible;
                mainFrame.NavigationService.Navigate(new UserManagementPage());
                browser.Visibility = Visibility.Collapsed;
            }
            else if (lv.SelectedIndex == 10)
            {
                MainWindow login = new MainWindow();
                login.Show();
            }
            else
            {
                mainBorder.Visibility = Visibility.Collapsed;
            }

        }


        private void VisitationSelected(object sender, RoutedEventArgs e)
        {
            
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ResponseView.Visibility = Visibility.Visible;
            browser.Visibility = Visibility.Collapsed;
            RestoreButton.Visibility = Visibility.Collapsed;
        }
    }
}
