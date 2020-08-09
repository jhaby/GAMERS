using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
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
    public partial class CasesPage : Page
    {
        private static ConnService sService;
        private CasesViewModel Items;
        private UserData User;
        private List<CasesModel> list;

        public CasesPage(UserData userinfo, ConnService signalService)
        {
            InitializeComponent();
            Items = new CasesViewModel(userinfo);
            User = userinfo;
            list = Items.ItemList(userinfo);
            this.DataContext = Items;
            sService = signalService;
            cases.ItemsSource = list;

            sService.HandleEventReceived += SService_HandleEventReceived;
        }

        private void SService_HandleEventReceived(Sender obj)
        {
            int index = list.FindIndex(ag => ag.CaseId.Equals(obj.CaseId, StringComparison.OrdinalIgnoreCase));
            list = list.Except(list.Where(x => x.CaseId == obj.CaseId)).ToList();
            
            
            
        }

        public static async Task UserID(Sender s)
        {
            
            await sService.HandleAlert(s);
        }
    }




    public class CasesViewModel
    {
        List<CasesModel> Items;

        public CasesViewModel(UserData user)
        {
            Items = Helpers.LoadCases().Result;

            foreach(var s in Items)
            {
                s.AgentId = user.UserId;
                
            }
        }

        public List<CasesModel> ItemList(UserData user)
        {
            Items = Helpers.LoadCases().Result;

            foreach(var s in Items)
            {
                s.AgentId = user.UserId;
                
            }

            return Items;

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
        private ICommand mycommand;

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
        public string CaseId { get => caseId; set { caseId = value; OnPropertyChanged("CaseId"); } }
        public string Location { get => location; set { location = value; OnPropertyChanged("Location"); } }
        public string Village { get => village; set { village = value; OnPropertyChanged("Village"); } }
        public string Description { get => description; set { description = value; OnPropertyChanged("Description"); } }
        public string VHTCode { get => vHTCode; set { vHTCode = value; OnPropertyChanged("VHTCode"); } }
        public string Status { get => status; set { status = value; OnPropertyChanged("Status"); } }
        public string Category { get => category; set { category = value; OnPropertyChanged("Category"); } }

        public ICommand Mycommand
        {
            get
            {
                if (mycommand == null)
                    mycommand = new RespondCommand(ExecuteCommand,CanExecuteCommand);
                return mycommand;
            }
        }

        private bool CanExecuteCommand(object arg)
        {
            return true;
        }

        private async void ExecuteCommand(object obj)
        {
            Sender sender = new Sender()
            {
                UserId = AgentId,
                CaseId = caseId,
                Response = "accept"
            };
            await CasesPage.UserID(sender);
            string[] details = { caseId, AgentId, Location, village, vHTCode, dateTime.ToString(), description };
            ResponseWindow response = new ResponseWindow(details);
            response.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
