using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Windows.Threading;

namespace GAMERS_TECH
{
    public partial class CasesPage : Page
    {
        private static ConnService sService;
        private CasesViewModel Items;
        private UserData User;
       
        public CasesPage (UserData userinfo, ConnService signalService)
        {
            InitializeComponent();
            sService = signalService;

            Items = new CasesViewModel(userinfo,sService);
            User = userinfo;

            this.DataContext = Items;

            sService.AlertReceived += SService_AlertReceived;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(cases.ItemsSource);

            
            sService.HandleEventReceived += SService_HandleEventReceived;
        }

        
        /*refresh cases ledger on alert broadcast receive*/
        private void SService_AlertReceived(CasesModel obj)
        {
            Items.Reload(User);
            CollectionViewSource.GetDefaultView(cases.ItemsSource).Refresh();
        }
        

        private void SService_HandleEventReceived(Sender obj)
        {
            Items.Reload(User);
            CollectionViewSource.GetDefaultView(cases.ItemsSource).Refresh();
        }
       

        //private void Decline_Click(object sender, RoutedEventArgs e)
        //{
        //}

        //private async void Handle_Click(object sender, RoutedEventArgs e)
        //{
        //    Button handle = sender as Button;

        //    int index = Items.Items.FindIndex(ag => ag.CaseId.Equals(handle.Tag.ToString(), StringComparison.OrdinalIgnoreCase));

        //    Sender agent = new Sender()
        //    {
        //        UserId = Items.Items[index].AgentId,
        //        CaseId = Items.Items[index].CaseId,
        //        Response = "accept"
        //    };

        //    string[] details = { Items.Items[index].CaseId, Items.Items[index].AgentId, Items.Items[index].Location, Items.Items[index].Village, Items.Items[index].VHTCode, Items.Items[index].DateTime.ToString(), Items.Items[index].Description };
        //    await Home.SendHandledAlert(agent, details);
        //}
    }




    public class CasesViewModel : INotifyPropertyChanged
    {
        private List<CasesModel> items;
        private ConnService sService;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

        public List<CasesModel> Items { get => items;

            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public CasesViewModel(UserData user, ConnService signal)
        {
            sService = signal;
            Items = new List<CasesModel>();
            Items.Clear();
            Dispatcher.CurrentDispatcher.Invoke(async() =>  await ItemList(user, signal));
            
        }

        public async Task ItemList(UserData user, ConnService signal)
        {
            
            try
            {
                var response = await StaticHelpers.httpclient.GetAsync(Environment.GetEnvironmentVariable("GamersServerUri") + "/loadcases");
                var result = await response.Content.ReadAsStringAsync();

                Items = JsonConvert.DeserializeObject<List<CasesModel>>(result);

                foreach (var s in Items)
                {
                    s.AgentId = user.UserId;
                    s.signalService = signal;

                }

               
            }
            catch
            {
                MessageBox.Show(Environment.GetEnvironmentVariable("GamersServerUri"));
            }

        }


        public async void Reload(UserData user)
        {
             await ItemList(user,sService);
        }

    }




    public class CasesModel : INotifyPropertyChanged
    {
        private DateTime dateTime;
        private string caseId;
        private string location;
        private string village;
        private string description;
        private string vHTCode;
        private string status;
        private string category;
        private ICommand sendcommand;
        private ICommand falseAlarm;

        public ConnService signalService { get; set; }

        public string AgentId { get; set; }

        public string Date
        {
            get
            {
                return dateTime.ToShortDateString();
            }
        }
        public string Time
        {
            get
            {
                return dateTime.ToShortTimeString();
            }
        }
        public DateTime DateTime { get { return dateTime; } set { dateTime = value; OnPropertyChanged("DateTime"); } }
        public string CaseId { get => "CaseId: " + caseId; set { caseId = value; OnPropertyChanged("CaseId"); } }
        public string Location { get => location; set { location = value; OnPropertyChanged("Location"); } }
        public string Village { get => "Village: " + village; set { village = value; OnPropertyChanged("Village"); } }
        public string Description { get => "Description: " + description; set { description = value; OnPropertyChanged("Description"); } }
        public string VHTCode { get => vHTCode; set { vHTCode = value; OnPropertyChanged("VHTCode"); } }
        public string Status { get => status; set { status = value; OnPropertyChanged("Status"); } }
        public string Category { get => category; set { category = value; OnPropertyChanged("Category"); } }

        public ICommand Sendcommand
        {
            get
            {
                if (sendcommand == null)
                    sendcommand = new RespondCommand(ExecuteCommand);
                return sendcommand;
            }
        }

        public ICommand FalseAlarm {
            get
            {
                if (falseAlarm == null)
                    falseAlarm = new RespondCommand(ExecuteFalseAlarm);
                return falseAlarm;
            }
        }

        private async void ExecuteFalseAlarm(object obj)
        {
            Sender sender = new Sender()
            {
                UserId = AgentId,
                CaseId = caseId,
                Response = "false"
            };

            string[] details = { caseId, AgentId, Location, village, vHTCode, dateTime.ToString(), description };
            await Home.SendHandledAlert(sender, details);
        }

        private async void ExecuteCommand(object obj)
        {
            Sender sender = new Sender()
            {
                UserId = AgentId,
                CaseId = caseId,
                Response = "accept"
            };

            string[] details = { caseId, AgentId, Location, village, vHTCode, dateTime.ToString(), description,category };
            await Home.SendHandledAlert(sender, details);


        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

    }
    public class AlertEventArgs: EventArgs
    {
        public List<CasesModel> CaseList { get; set; }
    }
}
