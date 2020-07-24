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
    /// Interaction logic for CasesPage.xaml
    /// </summary>
    public partial class CasesPage : Page
    {
        public CasesPage()
        {
            InitializeComponent();
            List<Emergencies> Items = new List<Emergencies>()
            {
                new Emergencies
                {
                    Date = DateTime.Now.ToShortDateString(),
                    Code = "329",
                    Time = DateTime.Now.ToShortTimeString(),
                    Village = "Nadunget",
                    Description="Malaria"
                },
                new Emergencies
                {
                    Date = DateTime.Now.ToShortDateString(),
                    Code = "948",
                    Time = DateTime.Now.ToShortTimeString(),
                    Village = "Najur",
                    Description="Diarrhoea"
                },
                new Emergencies
                {
                    Date = DateTime.Now.ToShortDateString(),
                    Code = "433",
                    Time = DateTime.Now.ToShortTimeString(),
                    Village = "Liaben",
                    Description="Malaria"
                }
            };
            cases.ItemsSource = Items;
        }
    }
    class Emergencies
    {
        public string Date { get; set; }
        public string Code { get; set; }
        public string Time { get; set; }
        public string Village { get; set; }
        public string Description { get; set; }
    }
}
