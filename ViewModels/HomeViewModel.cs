using System.Windows.Controls;
using System.Windows.Input;
using ChanceForHappiness.Helpers;
using ChanceForHappiness.Services;

namespace ChanceForHappiness.ViewModels
{
    /// <summary>
    /// ViewModel для головного екрану додатку.
    /// Надає інформацію про притулок та основні команди для навігації.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        private string _shelterName;
        private string _greeting;
        private string _address;
        private string _phoneNumber;
        private string _email;
        private string _bannerImagePath;
        private string _operatingHours;

        public string ShelterName
        {
            get => _shelterName;
            set => SetProperty(ref _shelterName, value);
        }

        public string Greeting
        {
            get => _greeting;
            set => SetProperty(ref _greeting, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
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

        public string BannerImagePath
        {
            get => _bannerImagePath;
            set => SetProperty(ref _bannerImagePath, value);
        }

        public string OperatingHours
        {
            get => _operatingHours;
            set => SetProperty(ref _operatingHours, value);
        }

        public ICommand ViewAnimalsCommand { get; }
        public ICommand BecomeVolunteerCommand { get; }

        /// Конструктор ViewModel головного екрану.
        /// Ініціалізує дані притулку та команди.
        public HomeViewModel()
        {
            ViewAnimalsCommand = new RelayCommand(param => App.NavigationService.NavigateTo(ViewType.AnimalList));
            BecomeVolunteerCommand = new RelayCommand(param => App.NavigationService.NavigateTo(ViewType.VolunteerForm));

            ShelterName = "Шанс на щастя";
            Greeting = "Ласкаво просимо до київського притулку для тварин 'Шанс на щастя', де кожна тварина заслуговує на люблячий дім!";
            Address = "Столичне шосе, 150, Київ, Україна, 03131";
            PhoneNumber = "+380 68 131 0580";
            Email = "novomlynetsp@gmail.com";
            OperatingHours = "Понеділок - П’ятниця: 10:00 - 20:00\nСубота: 10:00 - 18:00\nНеділя: Вихідний";

            App.LoggingService.Log("HomeViewModel ініціалізовано");
        }

        public static implicit operator UserControl(HomeViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}