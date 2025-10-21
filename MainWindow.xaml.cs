using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace CS2TotalCasePriceCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SteamCaseValueCalculator _calculator = new SteamCaseValueCalculator();

        public ObservableCollection<CaseItem> CaseItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            CaseItems = new ObservableCollection<CaseItem>
            {
             new CaseItem { CaseName = "Austin 2025 Contenders Autograph Capsule", Quantity = 1 },
             new CaseItem { CaseName = "Austin 2025 Contenders Sticker Capsule", Quantity = 7 },
             new CaseItem { CaseName = "Austin 2025 Legends Autograph Capsule", Quantity = 8 },
             new CaseItem { CaseName = "Dreams & Nightmares Case", Quantity = 15 },
             new CaseItem { CaseName = "Fever Case", Quantity = 1 },
             new CaseItem { CaseName = "Fracture Case", Quantity = 64 },
             new CaseItem { CaseName = "Killowatt Case", Quantity = 18 },
             new CaseItem { CaseName = "Recoil Case", Quantity = 16 },
             new CaseItem { CaseName = "Revolution Case", Quantity = 8 },
             new CaseItem { CaseName = "Revolver Case", Quantity = 1 },
             new CaseItem { CaseName = "Paris 2023 Legends Sticker Capsule", Quantity = 2 },

            };

            CaseGrid.ItemsSource = CaseItems;
        }

        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            ResultText.Text = "Fetching prices...";

            var dict = CaseItems
                .Where(c => !string.IsNullOrWhiteSpace(c.CaseName) && c.Quantity > 0)
                .ToDictionary(c => c.CaseName, c => c.Quantity);

            decimal total = await _calculator.CalculateTotalAsync(dict);

            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string logFilePath = Path.Combine(documentsPath, "CaseValueHistory.txt");
                string logEntry = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - Total Value: €{total:F2}{Environment.NewLine}";
                await File.AppendAllTextAsync(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Value: €{total:F2} (Failed to save log)";
            }

            ProgressBar.Visibility = Visibility.Collapsed;

            if (ResultText.Text.StartsWith("Fetching"))
            {
                ResultText.Text = $"Your total case value: €{total:F2}";
            }
        }

       
    }

    public class CaseItem
    {
        public string CaseName { get; set; }
        public int Quantity { get; set; }
    }
}
