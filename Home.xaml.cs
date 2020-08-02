using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
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

namespace GAMERS_TECH
{
    
    public partial class Home : Window
    {
        UserData User;
        HubConnection connection;
        ConnService signalService;
        PersonnelInfoViewModel persons;
        StatusModel stat;

        public  Home(UserData Userinfo)
        {
            InitializeComponent();
            User = Userinfo;
            Loaded += Home_Loaded;
            NameChip.Content = "Hi, "+ User.Firstname+" "+ User.Surname;

            stat = new StatusModel
            {
                UserId = User.UserId,
                Status = "Active"
            };

            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5000/updates")
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
                await signalService.SendStatus(stat);
            };

           
            signalService = new ConnService(connection);
            signalService.StatusReceived += Cos_StatusReceived;

            Task.Run(async()=> await ServerConnect());
   
            persons = new PersonnelInfoViewModel();

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

        private void Cos_StatusReceived(StatusModel obj)
        {
            stat.Status = obj.Status;
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Dashboard(User,stat,signalService));
        }

        private void DashboardClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Dashboard(User,stat,signalService));
            heading.Text = "Dashboard";
        }

        private void MapClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new IndexPage());
            heading.Text = "Map";
        }

        private void HisotryClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new HistoryPage());
            heading.Text = "History";
        }

        private void PersonnelinfoClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Personnelinfo(persons, signalService));
            heading.Text = "Personnel info";
        }

        private void CasesClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new CasesPage());
            heading.Text = "Alerts ledger";
        }

        private void LibraryClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new LibraryPage());
            heading.Text = "Library";
        }
    }
}
