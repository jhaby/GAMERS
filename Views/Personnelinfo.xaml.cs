using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Net.Http;
using Newtonsoft.Json;

namespace GAMERS_TECH
{
    public partial class Personnelinfo : Page
    {
        private List<PersonnelData> UserList;
        private UserData user;
        PersonnelInfoViewModel persons;
        private static ConnService signalService;
        private TextBox txtBox;
        private static string msg;
        private Button sendBtn;

        public Personnelinfo(UserData user ,PersonnelInfoViewModel personnel, ConnService Sservice)

        {
            InitializeComponent();
            signalService = Sservice;

            UserList = new List<PersonnelData>();
            this.user = user;
            persons = personnel ;

            
            Task.Run(async delegate
            {
                var response = await StaticHelpers.httpclient.GetAsync(Environment.GetEnvironmentVariable("GamersServerUri") + "/personnelinfo");
                var result = await response.Content.ReadAsStringAsync();

                Dispatcher.Invoke(() =>
                {
                    UserList = JsonConvert.DeserializeObject<List<PersonnelData>>(result);
                    Users.ItemsSource = UserList;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Users.ItemsSource);
                    view.Filter = UseFilter;
                });
                
            });


            signalService.SendingSuccess += (string response,string view,string sender) =>
              {
                  if(sender == user.UserId && view == "Personnel")
                  {
                      MessageBox.Show(response);
                  }
                  
              };

        }

        public static async Task SendSms(SMSDetails sms)
        {
            
            sms.Message = msg;
            await signalService.SendSMS(sms);
            
        }

        private bool UseFilter(object obj)
        {
            if (String.IsNullOrEmpty(SearchUser.Text))
                return true;
            else
                return ((obj as PersonnelData).Name.IndexOf(SearchUser.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(Users.ItemsSource).Refresh();
        }

        
        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            txtBox = sender as TextBox;
            msg = txtBox.Text;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendBtn = sender as Button;
            sendBtn.IsEnabled = false;

            Task.Run(async delegate
            {
                await Task.Delay(2000);
                Dispatcher.Invoke(() =>
                {
                    txtBox.Text = "";
                    sendBtn.IsEnabled = true;
                });
            });

        }
    }
   
}
