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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();

            List<History> Items1 = new List<History> {
                new History
                    {
                Date = DateTime.Now.ToShortDateString(),
                Code = "0034",
                Description = "Pnuemonia"
                    },
                 new History
                    {
                Date = DateTime.Now.ToShortDateString(),
                Code = "0027",
                Description = "Labour"
                    }
                };
            history.ItemsSource = Items1;

        }
    }

    class History
    {
        public string Date { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
