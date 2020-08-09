using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GAMERS_TECH
{
   
    public partial class ResponseWindow : Window
    {
        public ResponseWindow(string[] info)
        {
            InitializeComponent();
            location.Text = "Location: " + info[2] + ", " + info[3];
            string[] coords = info[2].Split(",");
            Location gpscoords = new Location(Convert.ToDouble(coords[0]), Convert.ToDouble(coords[1]));
            pinned.Location = gpscoords;
            aerialview.Click += (o, e) =>
            {
                map.Mode = new AerialMode(true);
            };
            roadview.Click += (o, e) =>
            {
                map.Mode = new RoadMode();
            };

            DateTime CurTime = DateTime.Now.ToLocalTime();
            Ctime.Text = CurTime.ToShortTimeString();
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                Ctime.Text = DateTime.Now.ToLongTimeString();
            };
            
            timer.IsEnabled = true;
        }

      

        
    }
}
