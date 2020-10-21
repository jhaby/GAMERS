using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace GAMERS_TECH.Views
{

    public partial class ResponsePage : Page
    {
        private UserData user;
        private ConnService sService;
        private string[] info;
        private string vhtCode;
        private EMTInfo EMTList;
        private MedicalInfo MedList;
        private ResponseViewModel viewdata;
        public event Action<string> BtnClicked;
        public static Action<string> PlaceCall;
        List<string> detailsList;

        public ResponsePage(UserData user ,ConnService sService, string[] details)
        {
            InitializeComponent();
            this.Loaded += ResponsePage_Loaded;
            this.user = user;
            this.sService = sService;
            this.info = details;

            detailsList = new List<string>();
            EMTList = new EMTInfo();
            MedList = new MedicalInfo();
            caseno.Text += info[0];

            StartProtocol();

            foreach(string i in details)
            {
                detailsList.Add(i);
            }

            Home.CloseHomeWindow += delegate
             {
                 TrackingMap.Close();
                 TrackingMap.Dispose();
             };

            sService.SendingSuccess += SService_SendingSuccess;

            sService.ProgressReportA += SService_ProgressReportA;
            sService.ProgressReportB += SService_ProgressReportB;
            sService.ProgressReportC += SService_ProgressReportC;
            sService.CallSuccess += SService_CallSuccess;
            sService.Restarted += (string obj) =>
             {
                 Dispatcher.Invoke(()=> RadWindow.Alert("Response protocol has been restarted"));
             };

            sService.ResponseSuccess += (string sender, string response) =>
            {
                if (sender == EMTList.Phone)
                {
                    if (response == "Yes")
                    {
                        Dispatcher.Invoke(delegate
                        {
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter accepted case, Starting tracking...", Brushes.Green);
                        });
                    }
                    else if (response == "No")
                    {
                        Dispatcher.Invoke(()=>  ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter declined case , re-assigning case", Brushes.Black));
                        

                    }
                }

            };

        }

        private void SService_CallSuccess(string arg1, string arg2, string arg3)
        {
            if (arg2 == EMTList.TeamID && arg3 == "Case")
            {
                Dispatcher.Invoke(() =>
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter has been called", Brushes.Blue);
                });
            }
            else if(arg2 == MedList.ID && arg3 == "CHW")
            {
                Dispatcher.Invoke(() =>
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Community health worker has been called", Brushes.Blue);
                });
            }
        }

        private void SService_ProgressReportC(MedicalInfo obj,string caseid)
        {
            if (obj != null && caseid == info[0])
            {
                Dispatcher.Invoke(()=>
                {
                    MedList = obj;

                    medicalId.Text += MedList.ID;
                    medicalStat.Text += MedList.Status;
                    medicalContact.Text += MedList.Phone;

                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully assigned community health worker", Brushes.Green);

                });
            }
            else if (obj == null && caseid == info[0])
            {
                Dispatcher.Invoke(delegate
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to loaded health worker info", Brushes.Green);
                });
            }
        }

        private void SService_ProgressReportB(EMTInfo obj,string caseid)
        {
            if (obj!=null && caseid == info[0])
            {
                Dispatcher.Invoke(()=>
                {
                    EMTList = obj;

                    teamId.Text += EMTList.TeamID;
                    teamMeans.Text += EMTList.Type;
                    teamphone.Text += EMTList.Phone;

                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully assignedTransporter", Brushes.Green);

                });
            }
            else if (obj == null && caseid == info[0])
            {
                Dispatcher.Invoke(delegate
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to loaded transporter info", Brushes.Green);
                });
            }

            
            
        }

        private void SService_ProgressReportA(ResponseViewModel obj,string caseid)
        {
            if (obj != null && caseid == info[0])
            {
                Dispatcher.Invoke(() =>
                {
                    viewdata = obj;

                    VHTinfo.Content = viewdata.Fullname;
                    Kinsphone.Text = "Kins phone: " + viewdata.Kin_phone;
                    VHTphone.Text = "Phone: " + viewdata.Phone;
                    VHTvillage.Text = "Village: " + viewdata.VHTVillage;

                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully loaded VHT info", Brushes.Green);


                });
            }
            else if (obj == null && caseid == info[0])
            {
                Dispatcher.Invoke(delegate
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to loaded VHT info", Brushes.Green);
                });
            }
        }


        private void SService_SendingSuccess(string response, string view, string sender)
        {
                if (sender == info[0] && view == "Case" && response != "failed")
                {
                    Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter contacted, waiting for response", Brushes.MediumBlue));
                }
                else if (sender == info[0] && view == "Case" && response == "failed")
                {
                    Dispatcher.Invoke(() =>
                    {
                       
                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to contact transporter.", Brushes.Red);
                       
                    });
                }
                else if (sender == info[0] && view == "CHW" && response != "failed")
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Community health worker contacted.", Brushes.Green);

                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Scheduling check up call...", Brushes.MediumBlue);

                    });
                }
                else if (sender == info[0] && view == "CHW" && response == "failed")
                {
                    Dispatcher.Invoke(() =>
                    {
                       ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to contact health worker.", Brushes.Red);
                        

                    });
                }

        }

        private void StartProtocol()
        {
            DateTime CurTime = DateTime.Now.ToLocalTime();
            Ctime.Text = CurTime.ToLongTimeString();
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                Ctime.Text = DateTime.Now.ToLongTimeString();
            };

            timer.IsEnabled = true;

            location.Text = "Location: " + info[2] + ", " + info[3];
            string[] coords = info[2].Split(",");

            vhtCode = info[4];
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(async()=> await LoadVHTinfo()));
            tasks.Add(Task.Run(async () => await LoadEMTInfo()));
            tasks.Add(Task.Run(async () => await LoadMEDInfo()));
            tasks.Add(Task.Run(async () => await LoadCaseLogs()));


        }

        private async Task LoadMEDInfo()
        {
            var response = await StaticHelpers.httpclient.GetAsync(String.Format(StaticHelpers.ServerBaseAddress + "/loadactivecase?caseid={0}&user={1}", info[0], "MED"));
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Dispatcher.Invoke(() =>
                    {
                        MedList = JsonConvert.DeserializeObject<MedicalInfo>(result);
                        medicalId.Text += MedList.ID;
                        medicalStat.Text += MedList.Status;
                        medicalContact.Text += MedList.Phone;

                    });
            }
            
        }

        private async Task LoadEMTInfo()
        {
            var response = await StaticHelpers.httpclient.GetAsync(String.Format(StaticHelpers.ServerBaseAddress + "/loadactivecase?caseid={0}&user={1}", info[0], "EMT"));
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Dispatcher.Invoke(() =>
                {
                    EMTList = JsonConvert.DeserializeObject<EMTInfo>(result);
                    teamId.Text += EMTList.TeamID;
                    teamMeans.Text += EMTList.Type;
                    teamphone.Text += EMTList.Phone;

                });
            }
        }

        private void ResponsePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingWindow loadingwindow = new LoadingWindow();
            loadingwindow.Show();
            string[] coords = info[2].Split(",");
            TrackingMap.Navigate(String.Format("https://gamers2.pagekite.me/maps?lat={0}&lon={1}", coords[0],coords[1]));
            TrackingMap.NavigationCompleted += TrackingMap_NavigationCompleted;
        }

        private void TrackingMap_NavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Location loaded", Brushes.Gray);
                Home.CloseLoadingWindow?.Invoke("close");
            }
            else
            {
                ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Internet error", Brushes.Gray);
                Home.CloseLoadingWindow?.Invoke("close");
            }
        }

        private async Task LoadVHTinfo()
        {
            var response = await StaticHelpers.httpclient.GetAsync(String.Format(StaticHelpers.ServerBaseAddress + "/loadactivecase?caseid={0}&user={1}", info[4], "VHT"));
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Dispatcher.Invoke(() =>
                {
                    viewdata = JsonConvert.DeserializeObject<ResponseViewModel>(result);
                    VHTinfo.Content = viewdata.Fullname;
                    Kinsphone.Text = "Kins phone: " + viewdata.Kin_phone;
                    VHTphone.Text = "Phone: " + viewdata.Phone;
                    VHTvillage.Text = "Village: " + viewdata.VHTVillage;

                });
            }
        }

        private async Task LoadCaseLogs()
        {
            var response = await StaticHelpers.httpclient.GetAsync(String.Format(StaticHelpers.ServerBaseAddress + "/loadactivecase?caseid={0}&user={1}", info[0], "logs"));
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Dispatcher.Invoke(() =>
                {
                    var sarray = result.Split(";");
                    foreach (var i in sarray)
                    {
                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: {i}", Brushes.CadetBlue);
                    }

                });
            }
        }

        public void ProgressUpdate(string prog, Brush color)
        {
            TextBlock text = new TextBlock
            {
                FontStyle = FontStyles.Italic,
                TextWrapping = TextWrapping.Wrap,
                Foreground = color,
                Text = prog
            };
            Separator sep = new Separator();

            progresslog.Children.Add(text);
            progresslog.Children.Add(sep);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Content)
            {
                case "Minimise":
                    BtnClicked?.Invoke("Close");
                    break;
                case "False Alarm":
                    BtnClicked?.Invoke("Cancel");
                    break;
                case "Restart":
                    BtnClicked?.Invoke("restart");
                    break;
                case "Completed":
                    BtnClicked?.Invoke("completed");
                    break;
            }
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            PhoneWindow phone = new PhoneWindow(viewdata.Phone);
            phone.Show();
        }

        private void ListViewItem2_Selected(object sender, RoutedEventArgs e)
        {
            PhoneWindow phone = new PhoneWindow(EMTList.Phone);
            phone.Show();
        }

        private void ListViewItem3_Selected(object sender, RoutedEventArgs e)
        {
            PhoneWindow phone = new PhoneWindow(MedList.Phone);
            phone.Show();
        }
    }

   
}
