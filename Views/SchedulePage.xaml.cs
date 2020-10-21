using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
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

    public class MyViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<Appointment> appointments;
        private ObservableCollection<Appointment> currentApps;

        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return this.appointments;
            }
            set
            {
                appointments = value;
                OnpropertyChange("Appointments");
            }
        }
        public ObservableCollection<Appointment> CurrentApps { get => currentApps; 
            set { currentApps = value; OnpropertyChange("CurrentApps"); } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChange(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

        public MyViewModel()
        {
            Dispatcher.CurrentDispatcher.Invoke(async () => Appointments = await CreateAppointments());

        }
        private async Task<ObservableCollection<Appointment>> CreateAppointments()
        {
            ObservableCollection<Appointment> apps = new ObservableCollection<Appointment>();

            var response = await StaticHelpers.httpclient.GetAsync(Environment.GetEnvironmentVariable("GamersServerUri") + "/loadappointments");
            var result = await response.Content.ReadAsStringAsync();
            var appointment = JsonConvert.DeserializeObject<List<AppointmentModel>>(result);

            foreach (var a in appointment)
            {
                var app1 = new Appointment()
                {
                    Subject = "CaseID:" + a.CaseId + " " + a.Subject,
                    Start = a.Start,
                    End = Convert.ToDateTime(a.Start).AddHours(7),
                    Body = a.Body,
                    Location = a.Location
                };
                apps.Add(app1);
            }


            return apps;
        }
    }
}
