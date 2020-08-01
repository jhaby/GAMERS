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
using System.Windows.Shapes;

namespace GAMERS_TECH
{
    
    public partial class Home : Window
    {
        UserData User;
        public Home(UserData Userinfo)
        {
            InitializeComponent();
            User = Userinfo;
            Loaded += Home_Loaded;
            NameChip.Content = "Hi, "+ User.Username;
            
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Dashboard());
        }

        private void DashboardClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Dashboard());
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
            body.NavigationService.Navigate(new Personnelinfo());
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
