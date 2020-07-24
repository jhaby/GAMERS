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
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();

            Loaded += Home_Loaded;
            
            
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Dashboard());
        }

        private void DashboardClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Dashboard());
        }

        private void MapClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new IndexPage());
        }

        private void HisotryClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new HistoryPage());
        }

        private void PersonnelinfoClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new Personnelinfo());
        }

        private void CasesClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new CasesPage());
        }

        private void LibraryClicked(object sender, RoutedEventArgs e)
        {
            body.NavigationService.Navigate(new LibraryPage());
        }
    }
}
