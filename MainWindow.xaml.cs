using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> modeSelection;
        private ServerFile server;
        private HttpClient client;
        private string result;

        public MainWindow()
        {
            InitializeComponent();
            modeSelection = new List<string>()
            {
                "Office","Remote/Home"
            };
            mode.ItemsSource = modeSelection;

            if (!Directory.Exists(@"C:\Gamers\Server_uri") || !File.Exists(@"C:\Gamers\Server_uri\server.json"))
            {
                Directory.CreateDirectory(@"C:\Gamers\Server_uri");
                if (!File.Exists(@"C:\Gamers\Server_uri\server.json"))
                {
                    File.Create(@"C:\Gamers\Server_uri\server.json").Close();
                    var server = new ServerFile()
                    {
                        Remote = "www.doctorsarch.org",
                        Local = "localhost",
                        CommPort = "5000"
                    };
                    var server_uri = JsonConvert.SerializeObject(server);
                    File.WriteAllText(@"C:\Gamers\Server_uri\server.json", server_uri);
                }
            }
            
           string  serverRaw = File.ReadAllText(@"C:\Gamers\Server_uri\server.json");
            server = JsonConvert.DeserializeObject<ServerFile>(serverRaw);
            Environment.SetEnvironmentVariable("GamersServerUri", server.Local);
            Environment.SetEnvironmentVariable("CommPort", server.CommPort);
            

                
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            string user = Username.Text;
            string Wregion = region.Text;
            string pswd = Password.Password;
            progress.Visibility = Visibility.Visible;
            if (Username.Text ==  "")
            {
                Username.Focus();
            }
            else if(pswd == "")
            {
                Password.Focus();
            }
            else if(Wregion == "")
            {
                region.Focus();
            }
            else
            {
                try
                {
                    client = new HttpClient();
                    var response = await client.GetAsync("http://" + Environment.GetEnvironmentVariable("GamersServerUri"));
                    if (response.IsSuccessStatusCode)
                    {
                        await Task.Run(() => LoginTask(user, pswd));
                    }
                }
                catch(Exception )
                {
                    MessageBox.Show(" Unable to connect to server (https://" + Environment.GetEnvironmentVariable("GamersServerUri") + ")");
                }
            }
            progress.Visibility = Visibility.Collapsed;
            

        }
        private async Task LoginTask(string user, string pass)
            {
                try
                {

                var response = await client.GetAsync("http://"+ Environment.GetEnvironmentVariable("GamersServerUri")+":"+Environment.GetEnvironmentVariable("CommPort")+"/auth?user="+user+"&pswd="+pass);
                result = await response.Content.ReadAsStringAsync();
                if (result != "null")
                {
                    UserData Userdata = JsonConvert.DeserializeObject<UserData>(result);
                    Dispatcher.Invoke(() =>
                    {

                    if (Userdata != null)
                    {
                        Home hm = new Home(Userdata);
                        hm.Show();
                        this.Close();
                    }
                    else
                    {
                        Dispatcher.Invoke(() => MessageBox.Show("Unauthorised user"));
                            
                        }
                    });
                }
                else
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Unauthorised user"));
                }
            }
                catch (Exception)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Unable to connect to server (" + Environment.GetEnvironmentVariable("GamersServerUri") + ")");
                    });
                }
            

        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            modeValue.Text = modeSelection[mode.SelectedIndex];
            if (modeValue.Text == "Office")
                Environment.SetEnvironmentVariable("GamersServerUri",server.Local);
            else
                Environment.SetEnvironmentVariable("GamersServerUri", server.Remote);
        }
    }
    public class ServerFile
    {
        public string Remote { get; set; }
        public string Local { get; set; }
        public string CommPort { get; set; }

    }
    public class LogIn
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }
    
}
