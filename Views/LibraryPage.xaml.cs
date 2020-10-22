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

namespace GAMERS_TECH
{
    public partial class LibraryPage : Page
    {
        public LibraryPage()
        {
            InitializeComponent();
        }
        int trigger = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if(trigger == 0)
            {
                filter.Visibility = Visibility.Visible;
                trigger = 1;
            }
            else
            {
                filter.Visibility = Visibility.Collapsed;
                trigger = 0;
            }
        }
    }
}
