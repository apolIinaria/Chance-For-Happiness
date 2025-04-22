using System.Windows;
using ChanceForHappiness.Services;
using ChanceForHappiness.ViewModels;

namespace ChanceForHappiness
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainViewModel();
            this.DataContext = viewModel;

            try
            {
                App.NavigationService.Initialize(viewModel);
                App.LoggingService?.Log("Navigation service успішно ініціалізовано");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка ініціалізації навігації: {ex.Message}",
                              "Помилка ініціалізації",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                App.LoggingService?.LogError("Помилка ініціалізації навігації", ex);
            }

            App.LoggingService?.Log("Main window ініціалізовано");
        }
    }
}