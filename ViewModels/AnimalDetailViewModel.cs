using System.Windows.Input;
using ChanceForHappiness.Helpers;
using ChanceForHappiness.Models;
using ChanceForHappiness.Models.Enums;
using ChanceForHappiness.Services;
using Microsoft.IdentityModel.Logging;

namespace ChanceForHappiness.ViewModels
{
    public class AnimalDetailViewModel : ViewModelBase
    {
        private readonly int _animalId;
        private Animal _animal;
        private bool _isLoading;
        private string _notFoundMessage;

        public Animal Animal
        {
            get => _animal;
            set
            {
                SetProperty(ref _animal, value);
                OnPropertyChanged(nameof(IsAnimalAvailable));
                OnPropertyChanged(nameof(IsAnimalUnavailable));
            }

        }

        public bool IsAnimalAvailable
        {
            get { return Animal != null && Animal.Status == AnimalStatus.Available; }
        }

        public bool IsAnimalUnavailable
        {
            get { return Animal != null && Animal.Status != AnimalStatus.Available; }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string NotFoundMessage
        {
            get => _notFoundMessage;
            set => SetProperty(ref _notFoundMessage, value);
        }

        public ICommand ApplyForAdoptionCommand { get; }

        public ICommand BackToListCommand { get; }

        public AnimalDetailViewModel(int animalId)
        {
            _animalId = animalId;

            ApplyForAdoptionCommand = new RelayCommand(param =>
            {
                App.NavigationService.NavigateToAdoptionForm(_animalId);
            });

            BackToListCommand = new RelayCommand(param =>
            {
                App.NavigationService.NavigateTo(ViewType.AnimalList);
            });

            Load();

            App.LoggingService.Log($"AnimalDetailViewModel ініціалізовано для: {animalId}");
        }

        public override void Load()
        {
            IsLoading = true;
            NotFoundMessage = string.Empty;

            try
            {
                Animal = App.DataService.GetAnimalById(_animalId);

                if (Animal == null)
                {
                    NotFoundMessage = "Тварину не знайдено. Можливо, її прилаштували або видалили з системи.";
                    App.LoggingService.LogWarning($"{_animalId} не знайдено");
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}