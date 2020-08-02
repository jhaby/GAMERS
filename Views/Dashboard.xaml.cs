using System;
using System.Collections.Generic;
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

namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        UserData User;
        StatusModel status;

        public Dashboard(UserData Userinfo,StatusModel stat,ConnService signalService)
        {
            InitializeComponent();

            User = Userinfo;

            this.DataContext = User;
            status = stat;

            signalService.StatusReceived += (StatusModel obj) =>
            {
                if(obj.UserId == status.UserId)
                {
                    status.Status = obj.Status;
                    User.Status = obj.Status;
                }
            };
            LoadHistory();
            StatusToggle.Unchecked += (o, e) =>
            {
                status.UserId = User.UserId;
                status.Status = "Unavailable";
                User.Status = status.Status;
                Task.Run(async () => {
                    await signalService.SendStatus(status);
                    await Helpers.UpdateStatus(status.Status, status.UserId);
                });
            };

            StatusToggle.Checked += (o, e) =>
            {
                status.UserId = User.UserId;
                status.Status = "Active";
                User.Status = status.Status;
                Task.Run(async () => {
                    await signalService.SendStatus(status);
                    await Helpers.UpdateStatus(status.Status, status.UserId);
                    });
            };

        }

        private void LoadHistory()
        {
            
        }
    }

    
}
