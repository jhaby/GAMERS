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
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            List<HistoryList> Items = new List<HistoryList>()
            {
                new HistoryList
                {
                    Date = DateTime.Now.ToLongDateString(),
                    Code = "#349",
                    Description="Labour code",
                    Agent = "John Willock",
                    VHT = "453",
                    Status = "Closed"
                }
            };
            history.ItemsSource = Items;
        }
    }
    class HistoryList
    {
        public string Date { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Agent { get; set; }
        public string VHT { get; set; }
        public string Status { get; set; }
    }
}
