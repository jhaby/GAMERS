using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

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
        private int Emttrial = 0;
        private int Chwtrial = 0;
        private HttpClient _client;
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
            _client = new HttpClient();

            StartProtocol();

            foreach(string i in details)
            {
                detailsList.Add(i);
            }

            sService.SendingSuccess += SService_SendingSuccess;

            sService.ProgressReportA += SService_ProgressReportA;
            sService.ProgressReportB += SService_ProgressReportB;
            sService.ProgressReportC += SService_ProgressReportC;

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

        private void SService_ProgressReportC(MedicalInfo obj)
        {
            if (obj != null)
            {
                Dispatcher.Invoke(async()=>
                {
                    MedList = obj;

                    medicalId.Text += MedList.ID;
                    medicalStat.Text += MedList.Status;
                    medicalContact.Text += MedList.Phone;

                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully assigned case", Brushes.Green);

                    await ContactCHW();
                });
            }
            else if (obj == null)
            {
                Dispatcher.Invoke(delegate
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to loaded health worker info", Brushes.Green);
                });
            }
        }

        private void SService_ProgressReportB(EMTInfo obj)
        {
            if (obj!=null)
            {
                Dispatcher.Invoke(async()=>
                {
                    EMTList = obj;

                    teamId.Text += EMTList.TeamID;
                    teamMeans.Text += EMTList.Type;
                    teamphone.Text += EMTList.Phone;

                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully assigned case", Brushes.Green);

                    await ContactEMT();
                });
            }
            else if (obj == null)
            {
                Dispatcher.Invoke(delegate
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to loaded transporter info", Brushes.Green);
                });
            }

            
            
        }

        private void SService_ProgressReportA(ResponseViewModel obj)
        {
            if (obj != null)
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
            else if (obj == null)
            {
                Dispatcher.Invoke(delegate
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to loaded VHT info", Brushes.Green);
                });
            }
        }


        private void SService_SendingSuccess(string response, string view, string sender)
        {
                if (sender == user.UserId && view == "Case" && response != "failed")
                {
                    Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter contacted, waiting for response", Brushes.MediumBlue));
                }
                else if (sender == user.UserId && view == "Case" && response == "failed")
                {
                    Dispatcher.Invoke(async () =>
                    {
                        if (Emttrial == 0)
                        {
                            Emttrial += 1;
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to contact transporter, trying again...", Brushes.Red);
                            await Task.Delay(1000);
                            await ContactEMT();
                        }
                        else if (Emttrial > 0)
                        {
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to contact transporter, attempting to call...", Brushes.Red);
                            await Task.Delay(1000);
                        }
                        Emttrial += 1;
                    });
                }
                else if (sender == user.UserId && view == "CHW" && response != "failed")
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Community health worker contacted.", Brushes.Green);

                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Scheduling check up call...", Brushes.MediumBlue);

                    });
                }
                else if (sender == user.UserId && view == "CHW" && response == "failed")
                {
                    Dispatcher.Invoke(async () =>
                    {
                        if (Chwtrial == 0)
                        {
                            Chwtrial += 1;
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to contact health worker, trying again...", Brushes.Red);
                            await Task.Delay(1000);
                            await ContactCHW();
                        }
                        else if (Chwtrial > 0)
                        {
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to contact health worker, attempting to call...", Brushes.Red);
                            await Task.Delay(1000);
                        }

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

            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Starting emergency protocol", Brushes.Gray);
            vhtCode = info[4];
           
            Task.Run(delegate { Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Loading VHT info", Brushes.MediumBlue)); });

            LoadVHTinfo("new");

            LoadEMTInfo("new");

            LoadMEDInfo("new");

        }

        private void LoadMEDInfo(string v)
        {
            List<string> villa = new List<string>() { info[3] };
            Task.Run(async () =>
            {
                await Dispatcher.Invoke(async () =>
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Assigning case to community health worker", Brushes.MediumBlue);
                    if (v == "new" || MedList == null)
                        await sService.ResponseCalls("MED",user, villa);
                });
            });
        }

        private void LoadEMTInfo(string v)
        {
            List<string> villa = new List<string>() { info[3]};
            Task.Run(async () =>
            {
                await Dispatcher.Invoke(async () =>
                {
                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Assigning response team/transporter", Brushes.MediumBlue);

                    if (v == "new" || EMTList == null)
                        await sService.ResponseCalls("EMT",user, villa);
                });
            });
        }

        private void ResponsePage_Loaded(object sender, RoutedEventArgs e)
        {
            string[] coords = info[2].Split(",");
            TrackingMap.Navigate(String.Format("https://www.doctorsarch.org/gamers_assets/?map=true&lat={0}&lon={1}",coords[0],coords[1]));
            TrackingMap.Loaded += delegate
             {
                 ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Location loaded", Brushes.Gray);
             };
        }

        private void LoadVHTinfo(string kind)
        {
                Task.Run(async () => {
                await Dispatcher.Invoke(async() =>
                {
                    if (kind == "new" || viewdata == null)
                        await sService.ResponseCalls("VHT", user, detailsList);

                });

                });


            
        }

        public async Task ContactEMT()
        {
            Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Contacting transporter", Brushes.MediumBlue));

            SMSDetails sms = new SMSDetails()
            {
                Number = EMTList.Phone,
                Message = "Emmergency alert! You have been assigned a pick up case from " + viewdata.VHTVillage + ". Do you accept this case. (reply yes/no)",
                View = "Case",
                Sender = user.UserId

            };

            await Dispatcher.Invoke(async () => await sService.SendSMS(sms));
            
        }
        public async Task ContactCHW()
        {
            Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Contacting Community health worker", Brushes.MediumBlue));

            SMSDetails sms = new SMSDetails()
            {
                Number = MedList.Phone,
                Message = $"NEW CASE! (#{info[0]})  You have been assigned a case in {viewdata.VHTVillage }. Check app for more information.",
                View = "CHW",
                Sender = user.UserId

            };
            await Dispatcher.Invoke(async () => await sService.SendSMS(sms));
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
                    BtnClicked?.Invoke("minimise");
                    break;
                case "Cancel":
                    BtnClicked?.Invoke("Cancel");
                    break;

            }
        }
    }

   
}
