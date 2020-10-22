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

namespace GAMERS_TECH.Views
{
    /// <summary>
    /// Interaction logic for CaseDetails.xaml
    /// </summary>
    public partial class CaseDetails : Page
    {
        public CaseDetails(HistoryModel history)
        {
            InitializeComponent();

            List<HistoryModel> Items = new List<HistoryModel>();
            Items.Add(history);
            Details.ItemsSource = Items;
            Details.IsEnabled = false;

            
        }
    }
    
}
