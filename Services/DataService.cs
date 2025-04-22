using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChanceForHappiness.Data;
using ChanceForHappiness.Models;
using ChanceForHappiness.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ChanceForHappiness.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _context;

        public DataService()
        {
            _context = new ApplicationDbContext();
            
            if (!_context.Animals.Any())
            {
                SeedInitialData();
            }

            App.LoggingService.Log("Сервіс даних ініціалізовано");
        }

        #region Animal Methods

        public List<Animal> GetAllAnimals()
        {
            return _context.Animals.ToList();
        }

        public List<Animal> GetAnimalsByType(AnimalType type)
        {
            return _context.Animals.Where(a => a.Type == type).ToList();
        }

        public List<Animal> GetAnimalsByStatus(AnimalStatus status)
        {
            return _context.Animals.Where(a => a.Status == status).ToList();
        }

        public Animal GetAnimalById(int id)
        {
            var animal = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                App.LoggingService.LogWarning($"Тварину з айді {id} не знайдено");
            }
            else
            {
                App.LoggingService.Log($"Повернута тварина: {animal.Name} (ID: {animal.Id})");
            }
            Console.WriteLine($"GetAnimalById({id}) повернуто: {(animal != null ? animal.Name : "null")}");

            return animal;
        }

        public void AddAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            _context.SaveChanges();
            App.LoggingService.Log($"Додано нову тварини: {animal.Name} (ID: {animal.Id})");
        }

        public bool UpdateAnimal(Animal animal)
        {
            try
            {
                _context.Animals.Update(animal);
                _context.SaveChanges();
                App.LoggingService.Log($"Оновлено тварину: {animal.Name} (ID: {animal.Id})");
                return true;
            }
            catch (Exception ex)
            {
                App.LoggingService.LogWarning($"Не вдалося оновити тварину {animal.Id} - не знайдено. Помилка: {ex.Message}");
                return false;
            }
        }

        public bool UpdateAnimalStatus(int animalId, AnimalStatus newStatus)
        {
            var animal = _context.Animals.Find(animalId);

            if (animal != null)
            {
                animal.Status = newStatus;
                _context.Animals.Update(animal);
                _context.SaveChanges();
                App.LoggingService.Log($"Оновлено статус тварини: {animal.Name} (ID: {animal.Id}) на {newStatus}");
                return true;
            }

            App.LoggingService.LogWarning($"Не вдалося оновити статус тварини {animalId} - не знайдено");
            return false;
        }

        #endregion

        #region Adoption Methods

        public List<Adoption> GetAllAdoptions()
        {
            return _context.Adoptions.ToList();
        }

        public List<Adoption> GetAdoptionsByAnimal(int animalId)
        {
            return _context.Adoptions
                .Include(a => a.Animal)
                .Where(a => a.AnimalId == animalId)
                .ToList();
        }
        public Adoption GetAdoptionById(int id)
        {
            return _context.Adoptions
                .Include(a => a.Animal)
                .FirstOrDefault(a => a.Id == id);
        }

        public void SubmitAdoption(Adoption adoption)
        {
            adoption.SubmissionDate = DateTime.Now;
            adoption.Status = "На розгляді";

            var animal = GetAnimalById(adoption.AnimalId);
            if (animal != null)
            {
                animal.Status = AnimalStatus.Reserved;
                _context.Animals.Update(animal);

                _context.Adoptions.Add(adoption);
                _context.SaveChanges();
                App.LoggingService.Log($"Подано нову заяву на прихист {animal.Name} від {adoption.ApplicantName}");
            }
            else
            {
                App.LoggingService.LogError($"Не вдалося подати документи на прихист тварини {adoption.AnimalId} - не знайдено");
                throw new ArgumentException($"Тварини {adoption.AnimalId} не знайдено");
            }
        }
        public bool UpdateAdoptionStatus(int adoptionId, string newStatus)
        {
            var adoption = _context.Adoptions.Find(adoptionId);

            if (adoption != null)
            {
                adoption.Status = newStatus;

                if (newStatus.Equals("Схвалено", StringComparison.OrdinalIgnoreCase))
                {
                    var animal = _context.Animals.Find(adoption.AnimalId);
                    if (animal != null)
                    {
                        animal.Status = AnimalStatus.Adopted;
                        _context.Animals.Update(animal);
                    }
                }

                else if (newStatus.Equals("Відхилено", StringComparison.OrdinalIgnoreCase))
                {
                    var animal = _context.Animals.Find(adoption.AnimalId);
                    if (animal != null)
                    {
                        animal.Status = AnimalStatus.Available;
                        _context.Animals.Update(animal);
                    }
                }

                _context.Adoptions.Update(adoption);
                _context.SaveChanges();
                App.LoggingService.Log($"Оновлений статус заяви на прихист {adoptionId} на {newStatus}");
                return true;
            }

            App.LoggingService.LogWarning($"Не вдалося оновити статус заяви на прихист {adoptionId} - не знайдено");
            return false;
        }

        #endregion

        #region Volunteer Methods

        public List<Volunteer> GetAllVolunteers()
        {
            return _context.Volunteers.ToList();
        }

        public Volunteer GetVolunteerById(int id)
        {
            return _context.Volunteers.Find(id);
        }

        public void SubmitVolunteer(Volunteer volunteer)
        {
            volunteer.SubmissionDate = DateTime.Now;
            volunteer.Status = "На розгляді";

            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();
            App.LoggingService.Log($"Нова волонтерська заявка подана від {volunteer.Name}");
        }

        public bool UpdateVolunteerStatus(int volunteerId, string newStatus)
        {
            var volunteer = _context.Volunteers.Find(volunteerId);

            if (volunteer != null)
            {
                volunteer.Status = newStatus;
                _context.Volunteers.Update(volunteer);
                _context.SaveChanges();

                App.LoggingService.Log($"Оновлений статус заявки волонтера {volunteerId} на {newStatus}");
                return true;
            }

            App.LoggingService.LogWarning($"Не вдалося оновити статус волонтера для {volunteerId} - не знайдено");
            return false;
        }

        #endregion

        #region Private Methods

        private void SeedInitialData()
        {
            string baseImagePath = "C:\\Users\\User\\Documents\\Uni\\OOP\\Sem_2\\ChanceForHappiness\\Resources\\Images\\";
            _context.Animals.Add(new Animal
            {
                Name = "Джек",
                Type = AnimalType.Dog,
                Breed = "Метис (дворняга)",
                AgeYears = 0,
                AgeMonths = 8,
                Gender = "Хлопчик",
                Weight = 6.5,
                Description = "Приємно познайомитись, я Джек. Мене разом з іншими цуценятами викинули у підвал багатоповерхівки. Майже всі цуценята знайшли свої родини, а я ще у пошуку... Виживав на вулиці, а зараз на перетримці серед дорослих собак. Я дуже контактний та грайливий. Маю ветпаспорт.",
                IsNeutered = true,
                Status = AnimalStatus.Available,
                ArrivalDate = DateTime.Now.AddMonths(-4),
                PhotoUrl = baseImagePath + "Jack.jpg",
                MedicalInfo = "Чіпований та оброблений від паразитів. Готовий до переїзду в нову родину",
                BehavioralNotes = "Швидко прив'язуюсь, активний із щінячим запалом, потребую любові та стабільності",
                GoodWithChildren = true,
                GoodWithOtherAnimals = true,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Бекслі",
                Type = AnimalType.Dog,
                Breed = "Сибірська хаскі",
                AgeYears = 1,
                AgeMonths = 8,
                Gender = "Дівчинка",
                Weight = 24.5,
                Description = "Привіт, я Бекслі. Мене знайшли на околиці міста — змучену, з пораненою лапою, але з вогнем в очах. Потребую людину, яка розуміє особливості породи. Мені потрібні навантаження, чіткі правила й повага до мого простору.",
                IsNeutered = false,
                Status = AnimalStatus.Adopted,
                ArrivalDate = DateTime.Now.AddMonths(-7),
                PhotoUrl = "C:\\Users\\User\\Documents\\Uni\\OOP\\Sem_2\\ChanceForHappiness\\Resources\\Images\\Baksli.jpg",
                MedicalInfo = "Чипована, оброблена від паразитів, можливий виїзд за кордон",
                BehavioralNotes = "Енергійна, допитлива й трохи нестримна",
                GoodWithChildren = false,
                GoodWithOtherAnimals = true,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Мона",
                Type = AnimalType.Cat,
                Breed = "Американска керл",
                AgeYears = 2,
                AgeMonths = 0,
                Gender = "Дівчинка",
                Weight = 3.7,
                Description = "Привіт, я Мона. У великому притулку мені трохи лячно — я тиха й непомітна серед натовпу хвостиків. Але глибоко в серці я мрію про своє затишне місце — окрему мисочку, м’якеньку лежаночку й теплі руки, що обіймають із ніжністю. Щодня я виглядаю у віконце, сподіваючись, що саме моя людина нарешті прийде.",
                IsNeutered = false,
                Status = AnimalStatus.Available,
                ArrivalDate = DateTime.Now.AddMonths(-6),
                PhotoUrl = "/Resources/Images/Mona.jpg",
                MedicalInfo = "Оброблена від бліх і гельмінтів. Готовий до переїзду в нову родину",
                BehavioralNotes = "Дуже тактильна й ніжна, проте обережна",
                GoodWithChildren = true,
                GoodWithOtherAnimals = false,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Пончик",
                Type = AnimalType.Dog,
                Breed = "Поткейк-дог",
                AgeYears = 3,
                AgeMonths = 0,
                Gender = "Хлопчик",
                Weight = 8.4,
                Description = "Я — Пончик. Жив біля магазину, де мене підгодовували — хтось кісточку дасть, хтось ковбаску. Я звик довіряти людям. Але одного разу мені дали щось, що мало все закінчити. Мене намагалися отруїти. На щастя, мене встигли врятувати, і зараз я проходжу лікування в притулку. Сподіваюся, попереду — тільки добро.",
                IsNeutered = true,
                Status = AnimalStatus.InTreatment,
                ArrivalDate = DateTime.Now.AddMonths(-2),
                PhotoUrl = "/Resources/Images/Ponchik.jpg",
                MedicalInfo = "Оброблений від паразитів, наразі під наглядом ветеринара отримую необхідні препарати",
                BehavioralNotes = "Остережливий, витривалий, лояльний і відданий",
                GoodWithChildren = true,
                GoodWithOtherAnimals = false,
                IsHouseTrained = false,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Хомі",
                Type = AnimalType.Cat,
                Breed = "Шартрез (Картезіанський)",
                AgeYears = 0,
                AgeMonths = 10,
                Gender = "Хлопчик",
                Weight = 4.4,
                Description = "Привіт, я — Хомі, молодий котик. Одного дня я потрапив у неприємну ситуацію: мене підкинули до будинку, де я випадково зустрівся з іншими котами. Вони були агресивними до мене. На щастя, мене помітили і забрали до притулку. Зараз я на карантині, оскільки я трохи побоююся нових знайомств і потребую спостереження, щоб бути впевненим, що я здоровий і готовий до нового дому.",
                IsNeutered = false,
                Status = AnimalStatus.Quarantine,
                ArrivalDate = DateTime.Now.AddMonths(-1),
                PhotoUrl = "/Resources/Images/Homi.jpg",
                MedicalInfo = "Чіпований, оброблений, відходжу від агресії з боку інших котів",
                BehavioralNotes = "Грайливий, коли почуваюсь у безпеці, спокійний, обожнюю ніжність",
                GoodWithChildren = true,
                GoodWithOtherAnimals = false,
                IsHouseTrained = true,
                IsVaccinated = false
            });

            _context.Animals.Add(new Animal
            {
                Name = "Норд",
                Type = AnimalType.Cat,
                Breed = "Као-мані",
                AgeYears = 1,
                AgeMonths = 1,
                Gender = "Хлопчик",
                Weight = 5.7,
                Description = "Я — Норд, котик з неймовірною особливістю — у мене різні очі! Одне з них блакитне, а інше — янтарне, і це робить мене справжнім унікальним чарівником. Я народився у маленькому містечку, де люди часто зупинялися, щоб подивитися на мене, але ніхто про мене не піклувався. Я почувався самотнім і шукав теплий дім. Я дуже чекаю на тебе, щоб стати твоїм найкращим другом і принести радість у твій дім!",
                IsNeutered = true,
                Status = AnimalStatus.Available,
                ArrivalDate = DateTime.Now.AddMonths(-3),
                PhotoUrl = "/Resources/Images/Nord.jpg",
                MedicalInfo = "Чіпований, оброблений від паразитів і маю всі необхідні профілактичні заходи. Готовий до переїзду в нову родину",
                BehavioralNotes = "Дружелюбний та енергійний. Я люблю гратися і досліджувати нові місця, із задоволенням проводжу час на колінах, насолоджуючись увагою",
                GoodWithChildren = true,
                GoodWithOtherAnimals = true,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Власта",
                Type = AnimalType.Dog,
                Breed = "Нідерландська тульпхонд (Маркіз'є)",
                AgeYears = 6,
                AgeMonths = 4,
                Gender = "Дівчинка",
                Weight = 15.8,
                Description = "Привіт, мене звати Власта. Моє життя змінилося через війну. Мої господарі, яких я дуже любила, змушені були покинути свою домівку і не змогли взяти мене з собою. Це було важко, але я трималася, адже знаю, що життя триває. Я зараз шукаю нову родину, яка прийме мене такою, якою я є, і дасть мені можливість знову відчути себе потрібною. Я дуже люблю людей і готова віддати всю свою любов тій родині, яка відкриє своє серце для мене.",
                IsNeutered = true,
                Status = AnimalStatus.Available,
                ArrivalDate = DateTime.Now.AddMonths(-4),
                PhotoUrl = "/Resources/Images/Vlasta.jpg",
                MedicalInfo = "Чіпована, оброблена від паразитів і здорова. Я готова до переїзду в нову родину",
                BehavioralNotes = "Я енергійна, весела і завжди готова до гри та довгих прогулянок",
                GoodWithChildren = true,
                GoodWithOtherAnimals = true,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Олівка",
                Type = AnimalType.Cat,
                Breed = "Бомбейська",
                AgeYears = 7,
                AgeMonths = 0,
                Gender = "Дівчинка",
                Weight = 6.2,
                Description = "Мене звати Олівка. Одного разу, я була дуже захоплена грою і не помітила, як за мною полетіла пташка. Я невдало спробувала зловити її, і через необережність отримала травму ока — утворилось невеличке більмо. Але не хвилюйтесь, я на лікуванні і поступово одужую. Моїм найбільшим бажанням є знайти люблячу родину, яка б допомогла мені повністю відновитися та подарувала б тепло і турботу.",
                IsNeutered = false,
                Status = AnimalStatus.InTreatment,
                ArrivalDate = DateTime.Now.AddMonths(-12),
                PhotoUrl = "/Resources/Images/Olivka.jpg",
                MedicalInfo = "Чіпована, оброблена від паразитів і на шляху до повного одужання",
                BehavioralNotes = "Ніжна і ласкава, люблю, коли мене погладжують і цілують",
                GoodWithChildren = true,
                GoodWithOtherAnimals = false,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Зіон",
                Type = AnimalType.Dog,
                Breed = "Метис (Лабрадор-ретривер)",
                AgeYears = 3,
                AgeMonths = 3,
                Gender = "Хлопчик",
                Weight = 14.9,
                Description = "Привіт, я Зіон! Я виріс на вулиці, спостерігаючи за світом із затишного куточка. Я не шукав пригод, а просто чекав на свій шанс. І ось він настав — мене знайшли добрі люди, і тепер я готовий знайти свою нову родину. Я не люблю зайвого галасу, але я вірний друг.",
                IsNeutered = true,
                Status = AnimalStatus.Adopted,
                ArrivalDate = DateTime.Now.AddMonths(-10),
                PhotoUrl = "/Resources/Images/Zion.jpg",
                MedicalInfo = "Здоровий, чіпований, оброблений від паразитів",
                BehavioralNotes = "Спокійний, дуже відданий собака. Чудовий компаньйон для будь-якої родини",
                GoodWithChildren = true,
                GoodWithOtherAnimals = true,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.Animals.Add(new Animal
            {
                Name = "Мейсон",
                Type = AnimalType.Dog,
                Breed = "Ханаанський",
                AgeYears = 2,
                AgeMonths = 8,
                Gender = "Хлопчик",
                Weight = 17.5,
                Description = "Я — Мейсон, і, здається, я завжди привертаю увагу. Моя активність і незвичайна зовнішність не можуть залишити байдужими нікого. Я не знаю, чому так сталося, але одного дня я просто опинився сам — красивий, сильний пес, без домівки. Ніхто не знає, що сталося з моїми попередніми господарями, але я залишився сам, і це стало новим початком. В притулку я швидко знайшов друзів, став слухняним і тепер готовий знайти свій справжній дім.",
                IsNeutered = true,
                Status = AnimalStatus.Available,
                ArrivalDate = DateTime.Now.AddMonths(-8),
                PhotoUrl = "/Resources/Images/Maison.jpg",
                MedicalInfo = "Здоровий, чіпований, оброблений від паразитів. Готовий до переїзду в нову родину",
                BehavioralNotes = "Енергійний та сильний характер. Я потребує активного господаря, який зможе забезпечити мені правильний догляд і увагу",
                GoodWithChildren = true,
                GoodWithOtherAnimals = true,
                IsHouseTrained = true,
                IsVaccinated = true
            });

            _context.SaveChanges();
            App.LoggingService.Log("Початкові дані для тварин додано");

            #endregion
        }
    }
}