using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChanceForHappiness.Helpers;
using ChanceForHappiness.Models;
using ChanceForHappiness.Models.Enums;

namespace ChanceForHappiness.ViewModels
{
    /// <summary>
    /// ViewModel для відображення та фільтрації списку тварин.
    /// Забезпечує функціональність пошуку, фільтрації та навігації до деталей тварин.
    /// </summary>
    public class AnimalListViewModel : ViewModelBase
    {
        private ObservableCollection<Animal> _animals;
        private string _searchTerm;
        private AnimalType? _selectedAnimalType;
        private AnimalStatus? _selectedStatus;
        private bool _isLoading;

        /// Колекція тварин, що відображається в інтерфейсі.
        public ObservableCollection<Animal> Animals
        {
            get => _animals;
            set => SetProperty(ref _animals, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    FilterAnimals();
                }
            }
        }

        public AnimalType? SelectedAnimalType
        {
            get => _selectedAnimalType;
            set
            {
                if (SetProperty(ref _selectedAnimalType, value))
                {
                    FilterAnimals();
                }
            }
        }
        public AnimalStatus? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
                {
                    FilterAnimals();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ViewAnimalDetailsCommand { get; }

        public ICommand ClearFiltersCommand { get; }

        /// Конструктор ViewModel списку тварин.
        /// Ініціалізує команди та завантажує дані.
        public AnimalListViewModel()
        {
            ViewAnimalDetailsCommand = new RelayCommand(param =>
            {
                if (param is int animalId)
                {
                    App.NavigationService.NavigateToAnimalDetails(animalId);
                }
            });

            ClearFiltersCommand = new RelayCommand(param =>
            {
                SearchTerm = string.Empty;
                SelectedAnimalType = null;
                SelectedStatus = null;
            });

            Animals = new ObservableCollection<Animal>();
            Load();

            App.LoggingService.Log("AnimalListViewModel ініціалізовано");
        }

        /// Завантажує всі дані про тварин із сервісу даних.
        public override void Load()
        {
            IsLoading = true;

            try
            {
                var allAnimals = App.DataService.GetAllAnimals();

                Animals.Clear();
                foreach (var animal in allAnimals)
                {
                    Animals.Add(animal);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// Фільтрує список тварин відповідно до вибраних фільтрів.
        private void FilterAnimals()
        {
            IsLoading = true;

            try
            {
                var allAnimals = App.DataService.GetAllAnimals();

                var filteredAnimals = allAnimals.AsQueryable();

                if (SelectedAnimalType.HasValue)
                {
                    filteredAnimals = filteredAnimals.Where(a => a.Type == SelectedAnimalType.Value);
                }

                if (SelectedStatus.HasValue)
                {
                    filteredAnimals = filteredAnimals.Where(a => a.Status == SelectedStatus.Value);
                }

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    string searchLower = SearchTerm.ToLower();
                    filteredAnimals = filteredAnimals.Where(a =>
                        a.Name.ToLower().Contains(searchLower) ||
                        a.Breed.ToLower().Contains(searchLower) ||
                        a.Description.ToLower().Contains(searchLower));
                }

                Animals.Clear();
                foreach (var animal in filteredAnimals)
                {
                    Animals.Add(animal);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}