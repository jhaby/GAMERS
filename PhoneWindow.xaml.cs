using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for PhoneWindow.xaml
    /// </summary>
    public partial class PhoneWindow : Window
    {
        private MediaPlayer media = new MediaPlayer();
        public PhoneWindow(string phone)
        {
            InitializeComponent();
            phonetitle.Text = "Dailing: " + phone;

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                Dispatcher.Invoke(() => phonetitle.Text = "Failed to connect to PBX");
            });

        }

        private void Endcall_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            this.Close();
        }
    }
}
