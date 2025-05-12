using System;
using ChanceForHappiness.ViewModels;

namespace ChanceForHappiness.Services
{
    /// <summary>
    /// Перерахування, що визначає основні типи представлень у додатку.
    /// Використовується для навігації між різними екранами.
    /// </summary>
    public enum ViewType
    {
        Home,
        AnimalList,
        AnimalDetail,
        AdoptionForm,
        VolunteerForm
    }

    /// <summary>
    /// Сервіс навігації в додатку.
    /// Забезпечує перехід між різними екранами та обробку їх параметрів.
    /// Реалізує шаблон MVVM для WPF-додатку.
    /// </summary>
    public class NavigationService
    {
        private MainViewModel _mainViewModel;
        private bool _isInitialized = false;

        public void Initialize(MainViewModel mainViewModel)
        {
            if (mainViewModel == null)
            {
                throw new ArgumentNullException(nameof(mainViewModel), "MainViewModel  не може бути нульовим для навігації");
            }

            _mainViewModel = mainViewModel;
            _isInitialized = true;

            App.LoggingService?.Log("Navigation service ініціалізовано");
        }

        /// Здійснює навігацію до вказаного типу представлення з опціональним параметром.
        public void NavigateTo(ViewType viewType, object parameter = null)
        {
            if (!_isInitialized || _mainViewModel == null)
            {
                throw new InvalidOperationException("NavigationService необхідно ініціалізувати перед використанням.");
            }

            App.LoggingService?.Log($"Перехід до {viewType}" + (parameter != null ? $" з параметром: {parameter}" : ""));

            try
            {
                switch (viewType)
                {
                    case ViewType.Home:
                        _mainViewModel.CurrentView = new HomeViewModel();
                        break;

                    case ViewType.AnimalList:
                        _mainViewModel.CurrentView = new AnimalListViewModel();
                        break;

                    case ViewType.AnimalDetail:
                        if (parameter is int animalId)
                        {
                            _mainViewModel.CurrentView = new AnimalDetailViewModel(animalId);
                        }
                        else
                        {
                            App.LoggingService?.LogWarning($"Неправильний параметр для подання AnimalDetail: {parameter}");
                          
                            _mainViewModel.CurrentView = new AnimalListViewModel();
                        }
                        break;

                    case ViewType.AdoptionForm:
                        if (parameter is int selectedAnimalId)
                        {
                            _mainViewModel.CurrentView = new AdoptionFormViewModel(selectedAnimalId);

                            App.LoggingService?.Log($"Створення моделі AdoptionFormViewModel для: {selectedAnimalId}");
                        }
                        else
                        {
                            _mainViewModel.CurrentView = new AdoptionFormViewModel();
                            App.LoggingService?.Log("Створення моделі AdoptionFormViewModel без ідентифікатора тварини");
                        }
                        break;

                    case ViewType.VolunteerForm:
                        _mainViewModel.CurrentView = new VolunteerFormViewModel();
                        break;

                    default:
                        App.LoggingService?.LogWarning($"Невідомий тип подання: {viewType}");
                        _mainViewModel.CurrentView = new HomeViewModel();
                        break;
                }
            }
            catch (Exception ex)
            {
                App.LoggingService?.LogError($"Помилка при переході до {viewType}: {ex.Message}", ex);

                _mainViewModel.CurrentView = new HomeViewModel();
            }
        }

        public void NavigateToAnimalDetails(int animalId)
        {
            NavigateTo(ViewType.AnimalDetail, animalId);
        }
        public void NavigateToAdoptionForm(int animalId)
        {
            NavigateTo(ViewType.AdoptionForm, animalId);
        }
        public bool IsInitialized => _isInitialized && _mainViewModel != null;
    }
}