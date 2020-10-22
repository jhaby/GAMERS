using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for UserManagementPage.xaml
    /// </summary>
    public partial class UserManagementPage : Page
    {
        public UserManagementPage()
        {
            InitializeComponent();
            var viewModel = new UserViewModel();
            this.DataContext = viewModel;
        }
    }

    public class UserViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<UserMgtModel> userinfo;
        public ObservableCollection<UserMgtModel> Userinfo { get => userinfo; set { userinfo = value; OnpropertyChanged("Userinfo"); } }
        public UserViewModel()
        {
            Dispatcher.CurrentDispatcher.Invoke(async()=> Userinfo = await GetData());
        }

        private async Task<ObservableCollection<UserMgtModel>> GetData()
        {
            ObservableCollection<UserMgtModel> Userdata = new ObservableCollection<UserMgtModel>();
            try
                {
                    var response = await StaticHelpers.httpclient.GetAsync(Environment.GetEnvironmentVariable("GamersServerUri") + "/loaduserdata");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Userdata = JsonConvert.DeserializeObject<ObservableCollection<UserMgtModel>>(result);
                        
                    }
                }
                catch(Exception ex)
                {
                Userdata.Add(new UserMgtModel { AgentName = ex.Message });
                }
            
            return Userdata;
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

        
    }
    public class UserMgtModel
    {
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string TotalCases { get; set; }
        public string Target { get; set; }
        public string Status { get; set; }
    }
}
