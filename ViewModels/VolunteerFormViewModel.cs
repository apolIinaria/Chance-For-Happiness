using System;
using System.Windows.Input;
using ChanceForHappiness.Helpers;
using ChanceForHappiness.Models;
using ChanceForHappiness.Services;

namespace ChanceForHappiness.ViewModels
{
    public class VolunteerFormViewModel : ViewModelBase
    {
        private string _name;
        private string _phoneNumber;
        private string _email;
        private string _address;
        private int _age;
        private string _availability;
        private string _preferredActivities;
        private string _previousExperience;
        private string _motivation;
        private string _skills;
        private string _additionalInformation;
        private string _emergencyContact;
        private bool _agreedToTerms;
        private bool _isSubmitting;
        private string _submissionMessage;
        private string _errorMessage;
        private bool _isSubmissionSuccessful;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public string Availability
        {
            get => _availability;
            set => SetProperty(ref _availability, value);
        }

        public string PreferredActivities
        {
            get => _preferredActivities;
            set => SetProperty(ref _preferredActivities, value);
        }

        public string PreviousExperience
        {
            get => _previousExperience;
            set => SetProperty(ref _previousExperience, value);
        }
 
        public string Motivation
        {
            get => _motivation;
            set => SetProperty(ref _motivation, value);
        }

        public string Skills
        {
            get => _skills;
            set => SetProperty(ref _skills, value);
        }

        public string AdditionalInformation
        {
            get => _additionalInformation;
            set => SetProperty(ref _additionalInformation, value);
        }

        public string EmergencyContact
        {
            get => _emergencyContact;
            set => SetProperty(ref _emergencyContact, value);
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

        public VolunteerFormViewModel()
        {
            SubmitApplicationCommand = new RelayCommand(param => SubmitApplication(), CanSubmitApplication);
            CancelCommand = new RelayCommand(param => App.NavigationService.NavigateTo(ViewType.Home));

            Age = 16;

            App.LoggingService.Log("VolunteerFormViewModel ініціалізовано");
        }

        private bool CanSubmitApplication(object param)
        {
            return !IsSubmitting &&
                   !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Address) &&
                   Age >= 16 &&
                   !string.IsNullOrWhiteSpace(Availability) &&
                   !string.IsNullOrWhiteSpace(PreferredActivities) &&
                   !string.IsNullOrWhiteSpace(Motivation) &&
                   !string.IsNullOrWhiteSpace(EmergencyContact) &&
                   AgreedToTerms;
        }

        private void SubmitApplication()
        {
            IsSubmitting = true;
            SubmissionMessage = string.Empty;
            ErrorMessage = string.Empty;
            IsSubmissionSuccessful = false;

            try
            {
                var volunteer = new Volunteer
                {
                    Name = Name,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Address = Address,
                    Age = Age,
                    Availability = Availability,
                    PreferredActivities = PreferredActivities,
                    PreviousExperience = PreviousExperience ?? string.Empty,
                    Motivation = Motivation,
                    Skills = Skills ?? string.Empty,
                    AdditionalInformation = AdditionalInformation ?? string.Empty,
                    EmergencyContact = EmergencyContact,
                    AgreedToTerms = AgreedToTerms,
                    SubmissionDate = DateTime.Now
                };

                App.DataService.SubmitVolunteer(volunteer);

                IsSubmissionSuccessful = true;
                SubmissionMessage = "Ваша заявка на волонтерство була успішно подана! " +
                    "Наша команда розгляне її та зв'яжеться з вами найближчим часом.";

                App.LoggingService.Log($"Заявку на участь у програмі подано на {Name}");

                ClearForm();
            }
            catch (Exception ex)
            {
                IsSubmissionSuccessful = false;
                ErrorMessage = "Виникла помилка під час відправлення вашої заявки. Будь ласка, спробуйте пізніше.";

                App.LoggingService.LogError("Помилка при заповненні анкети волонтера", ex);
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private void ClearForm()
        {
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            Age = 16;
            Availability = string.Empty;
            PreferredActivities = string.Empty;
            PreviousExperience = string.Empty;
            Motivation = string.Empty;
            Skills = string.Empty;
            AdditionalInformation = string.Empty;
            EmergencyContact = string.Empty;
            AgreedToTerms = false;
        }
    }
}