using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GAMERS_TECH
{
   
    public partial class ResponseWindow : Window
    {
        private ResponseViewModel viewModel;
        private string vhtCode;
        private List<EMTInfo> EMTList;
        private List<MedicalInfo> MedList;
        private ConnService _sService;

        public ResponseWindow(string[] info,ConnService sService)
        {
            InitializeComponent();

            viewModel = new ResponseViewModel();
            this.DataContext = viewModel;
            _sService = sService;

            location.Text = "Location: " + info[2] + ", " + info[3];

            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Starting emergency protocol",Brushes.Gray);
            vhtCode = info[4];

            string[] coords = info[2].Split(",");
            Location gpscoords = new Location(Convert.ToDouble(coords[0]), Convert.ToDouble(coords[1]));
            pinned.Location = gpscoords;
            map.Center = gpscoords;
            aerialview.Click += (o, e) =>
            {
                map.Mode = new AerialMode(true);
            };
            roadview.Click += (o, e) =>
            {
                map.Mode = new RoadMode();
            };
            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Location loaded", Brushes.Gray);
            Task.Run(delegate { Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Loading VHT info", Brushes.MediumBlue)); });
            
            LoadVHTinfo();

            DateTime CurTime = DateTime.Now.ToLocalTime();
            Ctime.Text = CurTime.ToLongTimeString();
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                Ctime.Text = DateTime.Now.ToLongTimeString();
            };
            
            timer.IsEnabled = true;
        }

        private void LoadVHTinfo()
        {
            try
            {
                Task.Run(async () => {
                    ResponseViewModel viewdata = await Helpers.LoadVHT(vhtCode);
                    viewModel.Fullname = viewdata.Fullname;
                    viewModel.Kin_phone = "Kins phone: " + viewdata.Kin_phone;
                    viewModel.Phone = "Phone: " + viewdata.Phone;
                    viewModel.VHTVillage = "Village: " + viewdata.VHTVillage;

                    Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully loaded VHT info", Brushes.Green));

                    EMTList = new List<EMTInfo>();
                    MedList = new List<MedicalInfo>();

                    await Dispatcher.Invoke(async () =>
                    {
                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Assigning response team/transporter", Brushes.MediumBlue);
                        await Task.Delay(500);
                        EMTList = await Helpers.LoadEMT(viewdata.VHTVillage);
                        teamId.Text += EMTList[0].TeamId;
                        teamMeans.Text += EMTList[0].Type;
                        teamphone.Text += EMTList[0].Phone;

                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully assigned case", Brushes.Green);
                        SMSDetails sms = new SMSDetails()
                        {
                            Number = EMTList[0].Phone,
                            Message = "Emmergency alert! You have been assigned a pick up case from " + viewdata.VHTVillage + ". Do you accept this case. (reply yes)",
                            View = "Case"

                        };
                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Contacting transporter", Brushes.MediumBlue);
                        await _sService.SendSMS(sms);


                        await Dispatcher.Invoke(async () =>
                        {
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Assigning case to community health worker", Brushes.MediumBlue);
                            await Task.Delay(500);
                            MedList = await Helpers.LoadMedical(viewdata.VHTVillage);
                            medicalId.Text += MedList[0].ID;
                            medicalStat.Text += MedList[0].Status;
                            medicalContact.Text += MedList[0].Phone;
                            medicalLocation.Text += MedList[0].Region;

                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Successfully assigned case", Brushes.Green);
                            SMSDetails sms = new SMSDetails()
                            {
                                Number = MedList[0].Phone,
                                Message = "Emmergency alert! You have been assigned a case in " + viewdata.VHTVillage + ". Do you accept this case. (reply yes)",
                                View = "CHW"

                            };
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Contacting transporter", Brushes.MediumBlue);
                            await _sService.SendSMS(sms);

                            _sService.SendingSuccess += (string response, string view) =>
                            {
                                if (view == "CHW")
                                {
                                    if (response.Length > 25)
                                    {

                                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter contacted", Brushes.MediumBlue);
                                    }
                                    else
                                    {

                                        ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed, trying again", Brushes.Red);
                                        Dispatcher.Invoke(async () => await _sService.SendSMS(sms));
                                    }
                                }
                                else if (view == "Yes")
                                {
                                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transported accepted case", Brushes.Green);
                                }

                            };

                        });



                        _sService.SendingSuccess += (string response, string view) =>
                        {
                            if(view == "Case")
                            {
                                if(response.Length > 25)
                                {

                                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transporter contacted", Brushes.MediumBlue);
                                }
                                else
                                {

                                    ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed, trying again", Brushes.Red);
                                   Dispatcher.Invoke(async()=> await _sService.SendSMS(sms));
                                }
                            }
                            else if(view == "Yes")
                            {
                                ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Transported accepted case", Brushes.Green);
                            }

                        };



                    });


                    if (EMTList.Count < 1)
                    {
                        Dispatcher.Invoke(() => {
                            ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to load info", Brushes.Red);
                        });
                        
                    }

                    
                   

                });

                
            }
            catch (Exception)
            {
                Dispatcher.Invoke(() => ProgressUpdate($"{DateTime.Now.ToLongTimeString()}: Failed to load info", Brushes.Red));
                LoadVHTinfo();
            }
        }

     public async Task LoadCHW(string location)
        {
           
        }

        public void ProgressUpdate(string prog, Brush color)
        {
            TextBlock text = new TextBlock();
            text.FontStyle = FontStyles.Italic;
            text.TextWrapping = TextWrapping.Wrap;
            text.Foreground = color;
            text.Text = prog;

            progresslog.Children.Add(text);
        }

        
    }
}
