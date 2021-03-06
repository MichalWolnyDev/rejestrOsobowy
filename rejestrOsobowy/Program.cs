using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace rejestrOsobowy
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HomeNumber { get; set; }


    }

    public class PersonList
    {
        public List<Person> persons = new List<Person>();

        public bool addPersonData(int id, string name, string surname, int age, string sex, string postal, string city, string street, int homeNumber)
        {

           if (new FileInfo("db.json").Length == 0)
           {
                persons.Add(new Person
                {
                    Id = id,
                    Name = name,
                    Surname = surname,
                    Age = age,
                    Sex = sex,
                    PostalCode = postal,
                    City = city,
                    Street = street,
                    HomeNumber = homeNumber

                });

                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter file = File.CreateText(@"db.json"))
                {
                   serializer.Serialize(file, persons);
                }
            } else
           {

                // overwrite json file

                string jsonString = File.ReadAllText(@"db.json");

                List<Person> personList = JsonConvert.DeserializeObject<List<Person>>(jsonString);

                personList.Add(new Person { 
                    Id = id, 
                    Name = name, 
                    Surname = surname,
                    Age = age,
                    Sex = sex,
                    PostalCode = postal,
                    City = city,
                    Street = street,
                    HomeNumber = homeNumber

                });

                string jsonData = JsonConvert.SerializeObject(personList);
                File.WriteAllText(@"db.json", jsonData);

                Console.WriteLine("Osoba zostala dodana do bazy");
            }


            return true;
        }

        public List<Person> getPersonList()
        {
            
            string jsonString = File.ReadAllText(@"db.json");

            List<Person> personList = JsonConvert.DeserializeObject<List<Person>>(jsonString);

            personList.ForEach(i => {
                Console.WriteLine("{0}\t", 
                    "ID: " + i.Id + "\n" + 
                    "Imie: " + i.Name + "\n" + 
                    "Nazwisko:" + i.Surname + "\n" + 
                    "Wiek: " + i.Age + "\n" + 
                    "Plec: " + i.Sex + "\n" + 
                    "Kod pocztowy: " + i.PostalCode + "\n" + 
                    "Miejscowosc: " + i.City + "\n" + 
                    "Ulica: " + i.Street + "\n" + 
                    "Numer domu: " + i.HomeNumber + "\n");
                Console.WriteLine("###################################################");
            });

            

            return persons;
        }
        public bool checkPersonExists(int id)
        {
            
            string jsonString = File.ReadAllText(@"db.json");

            List<Person> personList = JsonConvert.DeserializeObject<List<Person>>(jsonString);

            try
            {

                if (personList != null)
                {
                    if (personList.Exists(x => x.Id == id))
                    {
                        return true;
                    }
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return false;


        }

        public void deletePerson()
        {
            var json = File.ReadAllText(@"db.json");
            try
            {
                JArray usersArray = JArray.Parse(json);
                Console.Write("Podaj id uzytkownika: ");
                var userId = Convert.ToInt32(Console.ReadLine());

                if (userId > 0)
                {
                    var companyToDeleted = usersArray.FirstOrDefault(obj => obj["Id"].Value<int>() == userId);

                    usersArray.Remove(companyToDeleted);

                    string output = JsonConvert.SerializeObject(usersArray, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"db.json", output);

                    Console.WriteLine("Uzytkownik zostal usuniety");
                }
                else
                {
                    Console.Write("Niepoprawne id, sprobuj ponownie");
                    deletePerson();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    class Program
    {

        static void Main(string[] args)
        {

            bool showMenu = true;

            while (showMenu)
            {
                showMenu = Menu();
            }

           
            Console.ReadKey();
        }
        private static bool Menu()
        {
            Console.Clear();
            Console.WriteLine("Wybierz opcje:");
            Console.WriteLine("1) Dodaj osobe:");
            Console.WriteLine("2) Wyświetl baze osob:");
            Console.WriteLine("3) Usuń osobę z bazy:");
            var pl = new PersonList();

            switch (Console.ReadLine())
            {
                case "1":
                    addPerson();
                    return true;  
                case "2":
                    pl.getPersonList();
                    Console.ReadKey();
                    return true;
                case "3":
                    pl.deletePerson();
                    return true;
                default:
                    return true;
            }
        }

        private static void addPerson()
        {
            var personData = new PersonList();

            if (!File.Exists("db.json"))
            {
                File.CreateText(@"db.json");
                Console.WriteLine("Stworzono plik z baza");

                addPerson();
            }
            else
            {

                Console.WriteLine("Podaj ID");
                int id = Int32.Parse(Console.ReadLine());

                if (personData.checkPersonExists(id))
                {
                    Console.WriteLine("Osoba o podanym id juz istnieje");
                    Console.ReadKey();

                    addPerson();
                }

                Console.WriteLine("Podaj imie");
                string name = Console.ReadLine();

                Console.WriteLine("Podaj nazwisko");
                string surname = Console.ReadLine();

                Console.WriteLine("Podaj wiek");
                int age = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Podaj plec");
                string sex = Console.ReadLine();

                Console.WriteLine("Podaj kod pocztowy");
                string postal = Console.ReadLine();

                Console.WriteLine("Podaj miasto");
                string city = Console.ReadLine();

                Console.WriteLine("Podaj ulica");
                string street = Console.ReadLine();

                Console.WriteLine("Podaj numer domu");
                int homeNumber = Int32.Parse(Console.ReadLine());


                personData.addPersonData(id, name, surname, age, sex, postal, city, street, homeNumber);

            }




            Console.ReadKey();
        }
        
    }
}
