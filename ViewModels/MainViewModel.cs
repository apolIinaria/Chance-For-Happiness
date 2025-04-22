using System.Windows.Input;
using ChanceForHappiness.Helpers;
using ChanceForHappiness.Services;

namespace ChanceForHappiness.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand NavigateToHomeCommand { get; private set; }
        public ICommand NavigateToAnimalListCommand { get; private set; }
        public ICommand NavigateToAdoptionFormCommand { get; private set; }
        public ICommand NavigateToVolunteerFormCommand { get; private set; }

        public MainViewModel()
        {
            InitializeCommands();
            CurrentView = new HomeViewModel();
            App.LoggingService.Log("MainViewModel ініціалізовано");
        }

        private void InitializeCommands()
        {
            NavigateToHomeCommand = new RelayCommand(param => Navigate(ViewType.Home));
            NavigateToAnimalListCommand = new RelayCommand(param => Navigate(ViewType.AnimalList));
            NavigateToAdoptionFormCommand = new RelayCommand(param => Navigate(ViewType.AdoptionForm));
            NavigateToVolunteerFormCommand = new RelayCommand(param => Navigate(ViewType.VolunteerForm));
        }
        private void Navigate(ViewType viewType)
        {
            try
            {
                if (App.NavigationService != null && App.NavigationService.IsInitialized)
                {
                    App.NavigationService.NavigateTo(viewType);
                }
                else
                {
                    switch (viewType)
                    {
                        case ViewType.Home:
                            CurrentView = new HomeViewModel();
                            break;
                        case ViewType.AnimalList:
                            CurrentView = new AnimalListViewModel();
                            break;
                        case ViewType.AdoptionForm:
                            CurrentView = new AdoptionFormViewModel();
                            break;
                        case ViewType.VolunteerForm:
                            CurrentView = new VolunteerFormViewModel();
                            break;
                    }

                    App.LoggingService?.Log($"Використання альтернативної навігації до {viewType} - сервіс не ініціалізовано");
                }
            }
            catch (System.Exception ex)
            {
                App.LoggingService?.LogError($"Навігаційна помилка: {ex.Message}", ex);
                CurrentView = new HomeViewModel();
            }
        }
    }
}