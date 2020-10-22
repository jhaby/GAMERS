using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using Telerik.Windows.Controls;

namespace GAMERS_TECH.Views
{
    /// <summary>
    /// Interaction logic for PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        public string fee { get; private set; }

        public PaymentPage()
        {
            InitializeComponent();
            var vmodel = new GridViewModel();
            this.DataContext = vmodel;

        }

        private void gridView_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            e.Cancel = true;
        }

        private void SetFee_Click(object sender, RoutedEventArgs e)
        {
            RadWindow.Prompt("Set new fee per case:",this.SetFeeComplete);
        }


        private void SetFeeComplete(object sender, WindowClosedEventArgs e)
        {
            fee = e.PromptResult;
            if (!int.TryParse(e.PromptResult, out int amount))
                return;
            if(!string.IsNullOrEmpty(e.PromptResult) || amount > 0)
                RadWindow.Confirm("You are setting new fee to:"+e.PromptResult+" UGX",this.OnConfirm);
            
        }

        private void OnConfirm(object sender, WindowClosedEventArgs e)
        {
            fee = "failed";
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string extension = "xlsx";

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (.{0})|.{0}|All files (.)|.", extension, "Excel"),
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    gridView.ExportToXlsx(stream,
                        new GridViewDocumentExportOptions()
                        {
                            ShowColumnFooters = true,
                            ShowColumnHeaders = true,
                            ShowGroupFooters = true
                        });
                }
            }
        }
    }

    public class PayRecord : INotifyPropertyChanged
    {
        private string userID;
        private string name;
        private string amount;
        private DateTime creditDate;
        private string status;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserID { get => userID; set { userID = value; OnPropertyChanged("UserID"); } }
        public string Name { get => name; set { name = value; OnPropertyChanged("Name"); } }
        public string Amount { get => amount; set { amount = value; OnPropertyChanged("Amount"); } }
        public DateTime CreditDate { get => creditDate; set { creditDate = value; OnPropertyChanged("CreditDate"); } }
        public string Status { get => status; set { status = value; OnPropertyChanged("Status"); } }

       
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }

    public class GridViewModel : ViewModelBase
    {
        private ObservableCollection<PayRecord> records;

        public ObservableCollection<PayRecord> Records
        {
            get => records; set{records = value; OnPropertyChanged("Records"); }
        }
        public GridViewModel()
        {
            Dispatcher.CurrentDispatcher.Invoke(async () => Records = await CreateRecord());
        }
        private async Task<ObservableCollection<PayRecord>> CreateRecord()
        {
            ObservableCollection<PayRecord> records = new ObservableCollection<PayRecord>();

            var response = await StaticHelpers.httpclient.GetAsync(StaticHelpers.ServerBaseAddress + "/payments?load=true");
            var result = await response.Content.ReadAsStringAsync();

            records = JsonConvert.DeserializeObject<ObservableCollection<PayRecord>>(result);

            
            return records;
        }
    }
}
