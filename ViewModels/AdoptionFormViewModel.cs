using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ChanceForHappiness.Helpers;
using ChanceForHappiness.Models;
using ChanceForHappiness.Models.Enums;
using ChanceForHappiness.Services;

namespace ChanceForHappiness.ViewModels
{
    public class AdoptionFormViewModel : ViewModelBase
    {
        private int? _selectedAnimalId;
        private Animal _selectedAnimal;
        private ObservableCollection<Animal> _availableAnimals;
        private string _applicantName;
        private string _phoneNumber;
        private string _email;
        private string _address;
        private string _housingType;
        private bool _hasOwnHome;
        private string _livingSituation;
        private string _previousExperience;
        private bool _hasOtherPets;
        private string _otherPetsDescription;
        private int _hoursAwayFromHome;
        private string _adoptionReason;
        private string _veterinarianContact;
        private string _additionalNotes;
        private bool _agreedToTerms;
        private bool _isSubmitting;
        private string _submissionMessage;
        private string _errorMessage;
        private bool _isSubmissionSuccessful;

        public int? SelectedAnimalId
        {
            get => _selectedAnimalId;
            set
            {
                if (SetProperty(ref _selectedAnimalId, value) && value.HasValue)
                {
                    LoadSelectedAnimal();
                }
            }
        }
        public Animal SelectedAnimal
        {
            get => _selectedAnimal;
            set => SetProperty(ref _selectedAnimal, value);
        }
        public ObservableCollection<Animal> AvailableAnimals
        {
            get => _availableAnimals;
            set => SetProperty(ref _availableAnimals, value);
        }
        public string ApplicantName
        {
            get => _applicantName;
            set => SetProperty(ref _applicantName, value);
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string HousingType
        {
            get => _housingType;
            set => SetProperty(ref _housingType, value);
        }
        public bool HasOwnHome
        {
            get => _hasOwnHome;
            set => SetProperty(ref _hasOwnHome, value);
        }
        public string LivingSituation
        {
            get => _livingSituation;
            set => SetProperty(ref _livingSituation, value);
        }
        public string PreviousExperience
        {
            get => _previousExperience;
            set => SetProperty(ref _previousExperience, value);
        }
        public bool HasOtherPets
        {
            get => _hasOtherPets;
            set => SetProperty(ref _hasOtherPets, value);
        }
        public string OtherPetsDescription
        {
            get => _otherPetsDescription;
            set => SetProperty(ref _otherPetsDescription, value);
        }
        public int HoursAwayFromHome
        {
            get => _hoursAwayFromHome;
            set => SetProperty(ref _hoursAwayFromHome, value);
        }
        public string AdoptionReason
        {
            get => _adoptionReason;
            set => SetProperty(ref _adoptionReason, value);
        }
        public string VeterinarianContact
        {
            get => _veterinarianContact;
            set => SetProperty(ref _veterinarianContact, value);
        }
        public string AdditionalNotes
        {
            get => _additionalNotes;
            set => SetProperty(ref _additionalNotes, value);
        }
        public bool AgreedToTerms
        {
            get => _agreedToTerms;
            set => SetProperty(ref _agreedToTerms, value);
        }
        public bool IsSubmitting
        {
            get => _isSubmitting;
            set => SetProperty(ref _isSubmitting, value);
        }
        public string SubmissionMessage
        {
            get => _submissionMessage;
            set => SetProperty(ref _submissionMessage, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public bool IsSubmissionSuccessful
        {
            get => _isSubmissionSuccessful;
            set => SetProperty(ref _isSubmissionSuccessful, value);
        }
        public ICommand SubmitApplicationCommand { get; }

        public ICommand CancelCommand { get; }

        public AdoptionFormViewModel(int? animalId = null)
        {
            _selectedAnimalId = animalId;

            Console.WriteLine($"AdoptionFormViewModel створена з: {animalId}");

            SubmitApplicationCommand = new RelayCommand(param => SubmitApplication(), CanSubmitApplication);
            CancelCommand = new RelayCommand(param => App.NavigationService.NavigateTo(ViewType.AnimalList));

            AvailableAnimals = new ObservableCollection<Animal>();

            HoursAwayFromHome = 0; 

            Load();

            App.LoggingService.Log($"AdoptionFormViewModel ініціалізовано" +
                                   (animalId.HasValue ? $" для: {animalId}" : ""));
        }
        public override void Load()
        {
            var animals = App.DataService.GetAnimalsByStatus(AnimalStatus.Available);

            AvailableAnimals.Clear();
            foreach (var animal in animals)
            {
                AvailableAnimals.Add(animal);
            }

            if (_selectedAnimalId.HasValue)
            {
                LoadSelectedAnimal();
            }
        }

        private void LoadSelectedAnimal()
        {
            if (_selectedAnimalId.HasValue)
            {
                App.LoggingService.Log($"Завантажуємо: {_selectedAnimalId.Value}");

                SelectedAnimal = App.DataService.GetAnimalById(_selectedAnimalId.Value);

                if (SelectedAnimal != null)
                {
                    App.LoggingService.Log($"Успішне завантаження: {SelectedAnimal.Name} (ID: {SelectedAnimal.Id})");
                }
                else
                {
                    App.LoggingService.LogWarning($"{_selectedAnimalId.Value} не знайдено");
                    _selectedAnimalId = null;
                }

                OnPropertyChanged(nameof(SelectedAnimal));
            }
            else
            {
                SelectedAnimal = null;
                OnPropertyChanged(nameof(SelectedAnimal));
            }
        }

        private bool CanSubmitApplication(object param)
        {
            bool hasRequiredFields = !IsSubmitting &&
                       SelectedAnimalId.HasValue &&
                       !string.IsNullOrWhiteSpace(ApplicantName) &&
                       !string.IsNullOrWhiteSpace(PhoneNumber) &&
                       !string.IsNullOrWhiteSpace(Email) &&
                       !string.IsNullOrWhiteSpace(Address) &&
                       AgreedToTerms;

            if (!hasRequiredFields)
            {
                Console.WriteLine($"Форма не може бути надіслана. Проблеми: " +
                                 $"Подається: {IsSubmitting}, " +
                                 $"Вибрана тварина: {(SelectedAnimalId.HasValue ? SelectedAnimalId.Value.ToString() : "null")}, " +
                                 $"ПІБ: {(!string.IsNullOrWhiteSpace(ApplicantName))}, " +
                                 $"Номер телефону: {(!string.IsNullOrWhiteSpace(PhoneNumber))}, " +
                                 $"Email: {(!string.IsNullOrWhiteSpace(Email))}, " +
                                 $"Домашня адреса: {(!string.IsNullOrWhiteSpace(Address))}, " +
                                 $"Погодження з умовами: {AgreedToTerms}");
            }

            return hasRequiredFields;
        }
        private void SubmitApplication()
        {
            IsSubmitting = true;
            SubmissionMessage = string.Empty;
            ErrorMessage = string.Empty;
            IsSubmissionSuccessful = false;

            try
            {
                var adoption = new Adoption
                {
                    AnimalId = SelectedAnimalId.Value,
                    ApplicantName = ApplicantName,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Address = Address,
                    HousingType = HousingType ?? string.Empty,
                    HasOwnHome = HasOwnHome,
                    LivingSituation = LivingSituation ?? string.Empty,
                    PreviousExperience = PreviousExperience ?? string.Empty,
                    HasOtherPets = HasOtherPets,
                    OtherPetsDescription = OtherPetsDescription ?? string.Empty,
                    HoursAwayFromHome = HoursAwayFromHome,
                    AdoptionReason = AdoptionReason ?? string.Empty,
                    VeterinarianContact = VeterinarianContact ?? string.Empty,
                    AdditionalNotes = AdditionalNotes ?? string.Empty,
                    AgreedToTerms = AgreedToTerms,
                    SubmissionDate = DateTime.Now
                };

                App.DataService.SubmitAdoption(adoption);

                IsSubmissionSuccessful = true;
                SubmissionMessage = "Ваша заява на прихист була успішно подана! " +
                    "Наша команда розгляне її та зв'яжеться з вами найближчим часом.";

                App.LoggingService.Log($"Подана заявка на прихист для: {SelectedAnimalId.Value} від {ApplicantName}");

                ClearForm();
            }
            catch (Exception ex)
            {
                IsSubmissionSuccessful = false;
                ErrorMessage = "Виникла помилка під час відправлення вашої заявки. Будь ласка, спробуйте пізніше.";

                App.LoggingService.LogError("Помилка під час подання заяви на прихист", ex);
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private void ClearForm()
        {
            ApplicantName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            HousingType = string.Empty;
            HasOwnHome = false;
            LivingSituation = string.Empty;
            PreviousExperience = string.Empty;
            HasOtherPets = false;
            OtherPetsDescription = string.Empty;
            HoursAwayFromHome = 0;
            AdoptionReason = string.Empty;
            VeterinarianContact = string.Empty;
            AdditionalNotes = string.Empty;
            AgreedToTerms = false;
        }
    }
}