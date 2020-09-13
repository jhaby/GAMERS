using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls.ScheduleView;

namespace GAMERS_TECH.Views
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
            MyViewModel schedule = new MyViewModel();
            this.DataContext = schedule;
        }
    }

    public class MyViewModel
    {
        private ObservableCollection<Appointment> appointments;

        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                if (this.appointments == null)
                {
                    this.appointments = this.CreateAppointments();
                }
                return this.appointments;
            }
        }

        private ObservableCollection<Appointment> CreateAppointments()
        {
            ObservableCollection<Appointment> apps = new ObservableCollection<Appointment>();

            var app1 = new Appointment()
            {
                Subject = "Front-End Meeting",
                Start = DateTime.Today.AddHours(9),
                End = DateTime.Today.AddHours(10),

            };
            apps.Add(app1);

            var app2 = new Appointment()
            {
                Subject = "Planning Meeting",
                Start = DateTime.Today.AddHours(11),
                End = DateTime.Today.AddHours(12)
            };
            apps.Add(app2);

            return apps;
        }
    }
}
