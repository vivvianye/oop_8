using RealEstateAgency.Models;

namespace RealEstateAgency.ConsoleApp
{
    class Program
    {
        static RealtorAgency agency = new RealtorAgency();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            SeedData();

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n========== РІЕЛТОРСЬКА ФІРМА ==========");
                Console.WriteLine("1. Управління клієнтами");
                Console.WriteLine("2. Управління даними про нерухомість");
                Console.WriteLine("3. Управління пропозиціями");
                Console.WriteLine("4. Пошук");
                Console.WriteLine("0. Вихід");
                Console.Write("Оберіть розділ: ");

                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": ClientMenu(); break;
                    case "2": PropertyMenu(); break;
                    case "3": OffersMenu(); break;
                    case "4": SearchMenu(); break;
                    case "0": isRunning = false; break;
                    default: Console.WriteLine("Невідома команда."); break;
                }
            }
        }

        /// <summary>1. Меню управління клієнтами</summary>
        static void ClientMenu()
        {
            Console.WriteLine("\n--- Управління клієнтами ---");
            Console.WriteLine("1 Додати клієнта");
            Console.WriteLine("2 Видалити клієнта");
            Console.WriteLine("3 Змінювати дані клієнтів");
            Console.WriteLine("4 Переглянути дані конкретного клієнта");
            Console.WriteLine("5 Переглянути всіх (сорт. по імені)");
            Console.WriteLine("6 Переглянути всіх (сорт. по прізвищу)");
            Console.WriteLine("7 Переглянути всіх (сорт. по цифрі рахунку)");
            Console.Write("Вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Ім'я: "); string fname = Console.ReadLine();
                    Console.Write("Прізвище: "); string lname = Console.ReadLine();
                    Console.Write("Рахунок: "); string acc = Console.ReadLine();
                    Console.WriteLine("Бажаний тип (0: 1-кімн, 1: 2-кімн, 2: 3-кімн, 3: Ділянка): ");
                    PropertyType pType = (PropertyType)int.Parse(Console.ReadLine());
                    Console.Write("Бажана максимальна ціна: "); decimal mPrice = decimal.Parse(Console.ReadLine());
                    agency.AddClient(new Client(fname, lname, acc, pType, mPrice));
                    Console.WriteLine("Додано.");
                    break;
                case "2":
                    var cToDel = SelectClient();
                    if (cToDel != null) { agency.RemoveClient(cToDel); Console.WriteLine("Видалено."); }
                    break;
                case "3":
                    var cToUpd = SelectClient();
                    if (cToUpd != null)
                    {
                        Console.Write("Нове ім'я: "); string nFname = Console.ReadLine();
                        Console.Write("Нове прізвище: "); string nLname = Console.ReadLine();
                        Console.Write("Новий рахунок: "); string nAcc = Console.ReadLine();
                        Console.WriteLine("Новий бажаний тип (0-3): "); PropertyType nType = (PropertyType)int.Parse(Console.ReadLine());
                        Console.Write("Нова ціна: "); decimal nPrice = decimal.Parse(Console.ReadLine());
                        agency.UpdateClient(cToUpd, nFname, nLname, nAcc, nType, nPrice);
                        Console.WriteLine("Оновлено.");
                    }
                    break;
                case "4":
                    var cToShow = SelectClient();
                    if (cToShow != null) Console.WriteLine(cToShow);
                    break;
                case "5": PrintList(agency.GetClientsSortedByName()); break;
                case "6": PrintList(agency.GetClientsSortedByLastName()); break;
                case "7": PrintList(agency.GetClientsSortedByAccountFirstDigit()); break;
            }
        }

        /// <summary>2. Меню управління нерухомістю</summary>
        static void PropertyMenu()
        {
            Console.WriteLine("\n--- Управління нерухомістю ---");
            Console.WriteLine("1 Додати об'єкт нерухомості");
            Console.WriteLine("2 Видалити об'єкт");
            Console.WriteLine("3 Змінити дані об'єкта (ціна, площа)");
            Console.WriteLine("4 Переглянути дані конкретного об'єкта");
            Console.WriteLine("5 Переглянути всіх (сорт. по типу)");
            Console.WriteLine("6 Переглянути всіх (сорт. по вартості)");
            Console.Write("Вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Тип (0: 1-кімн, 1: 2-кімн, 2: 3-кімн, 3: Ділянка): ");
                    PropertyType t = (PropertyType)int.Parse(Console.ReadLine());
                    Console.Write("Ціна: "); decimal p = decimal.Parse(Console.ReadLine());
                    Console.Write("Площа: "); double a = double.Parse(Console.ReadLine());
                    Console.Write("Місто: "); string c = Console.ReadLine();
                    Console.Write("Вулиця: "); string s = Console.ReadLine();
                    Console.Write("Номер: "); string b = Console.ReadLine();

                    if (t == PropertyType.LandPlot)
                    {
                        Console.Write("Комунікації (true/false): "); bool u = bool.Parse(Console.ReadLine());
                        agency.AddProperty(new LandPlot(p, a, u, c, s, b));
                    }
                    else
                    {
                        Console.Write("Поверх: "); int f = int.Parse(Console.ReadLine());
                        agency.AddProperty(new Apartment(t, p, a, f, c, s, b));
                    }
                    Console.WriteLine("Додано.");
                    break;
                case "2":
                    var pToDel = SelectProperty();
                    if (pToDel != null) { agency.RemoveProperty(pToDel); Console.WriteLine("Видалено."); }
                    break;
                case "3":
                    var pToUpd = SelectProperty();
                    if (pToUpd != null)
                    {
                        Console.Write("Нова ціна: "); decimal nP = decimal.Parse(Console.ReadLine());
                        Console.Write("Нова площа: "); double nA = double.Parse(Console.ReadLine());
                        agency.UpdatePropertyBasicData(pToUpd, nP, nA);
                        Console.WriteLine("Оновлено.");
                    }
                    break;
                case "4":
                    var pToShow = SelectProperty();
                    if (pToShow != null) Console.WriteLine(pToShow);
                    break;
                case "5": PrintList(agency.GetPropertiesSortedByType()); break;
                case "6": PrintList(agency.GetPropertiesSortedByPrice()); break;
            }
        }

        /// <summary>3. Меню управління пропозиціями</summary>
        static void OffersMenu()
        {
            Console.WriteLine("\n--- Управління пропозиціями ---");
            Console.WriteLine("1. Додати об'єкт до списку пропозицій клієнта (n<5)");
            Console.WriteLine("2. Визначити доступні об'єкти за вимогами клієнта");
            Console.WriteLine("3. Клієнт відхиляє пропозицію");
            Console.Write("Вибір дії: ");

            string choice = Console.ReadLine();

            Console.WriteLine("\n--- Оберіть клієнта для цієї дії ---");
            var client = SelectClient();
            
            if (client == null) return; 

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n--- Оберіть об'єкт нерухомості для пропозиції ---");
                    var propToAdd = SelectProperty();
                    if (propToAdd != null)
                    {
                        client.AddOffer(propToAdd);
                        Console.WriteLine("Пропозицію успішно додано клієнту.");
                    }
                    break;

                case "2":
                    var matches = agency.FindAvailablePropertiesForClient(client).ToList();
                    Console.WriteLine($"\nЗнайдено {matches.Count} об'єктів, що підходять клієнту:");
                    PrintList(matches);
                    break;

                case "3":
                    Console.WriteLine("\n--- Поточні пропозиції клієнта ---");
                    var offers = client.Offers.ToList();
                    
                    if (offers.Count == 0)
                    {
                        Console.WriteLine("У цього клієнта поки немає пропозицій.");
                        break;
                    }

                    for (int i = 0; i < offers.Count; i++) 
                        Console.WriteLine($"{i}. {offers[i]}");
                    
                    Console.Write("Оберіть номер пропозиції для відхилення: ");
                    int idx = int.Parse(Console.ReadLine());
                    
                    client.RejectOffer(offers[idx]);
                    Console.WriteLine("Пропозицію відхилено (видалено зі списку).");
                    break;
            }
        }

        /// <summary>4. Меню пошуку</summary>
        static void SearchMenu()
        {
            Console.WriteLine("\n--- Пошук ---");
            Console.WriteLine("1 Пошук серед клієнтів");
            Console.WriteLine("2 Пошук серед об'єктів");
            Console.WriteLine("3 Пошук всюди");
            Console.WriteLine("4 Розширений пошук клієнта");
            Console.Write("Вибір: ");

            string choice = Console.ReadLine();
            
            if (choice == "4")
            {
                Console.Write("Введіть прізвище: "); string lName = Console.ReadLine();
                Console.WriteLine("Бажаний тип (0-3): "); PropertyType pt = (PropertyType)int.Parse(Console.ReadLine());
                PrintList(agency.AdvancedClientSearch(lName, pt));
                return;
            }

            Console.Write("Введіть ключове слово: ");
            string keyword = Console.ReadLine();

            switch (choice)
            {
                case "1": PrintList(agency.SearchClients(keyword)); break;
                case "2": PrintList(agency.SearchProperties(keyword)); break;
                case "3": PrintList(agency.SearchAll(keyword)); break;
            }
        }

        /// <summary>Допоміжний метод вибору клієнта зі списку</summary>
        static Client SelectClient()
        {
            var clients = agency.Clients.ToList();
            if (clients.Count == 0) { Console.WriteLine("Клієнтів немає."); return null; }
            
            for (int i = 0; i < clients.Count; i++)
                Console.WriteLine($"{i}. {clients[i].FirstName} {clients[i].LastName}");
            
            Console.Write("Оберіть номер клієнта: ");
            int index = int.Parse(Console.ReadLine());
            return clients[index];
        }

        /// <summary>Допоміжний метод вибору нерухомості зі списку</summary>
        static Property SelectProperty()
        {
            var props = agency.Properties.ToList();
            if (props.Count == 0) { Console.WriteLine("Об'єктів немає."); return null; }
            
            for (int i = 0; i < props.Count; i++)
                Console.WriteLine($"{i}. {props[i]}");
            
            Console.Write("Оберіть номер об'єкта: ");
            int index = int.Parse(Console.ReadLine());
            return props[index];
        }

        /// <summary>Допоміжний метод виводу колекції</summary>
        static void PrintList<T>(IEnumerable<T> items)
        {
            var list = items.ToList();
            if (list.Count == 0) Console.WriteLine("Нічого не знайдено.");
            foreach (var item in list) Console.WriteLine($"- {item}");
        }

        /// <summary>Початкові дані для полегшення тестування</summary>
        static void SeedData()
        {
            agency.AddClient(new Client("Іван", "Петренко", "41490000", PropertyType.OneRoomApartment, 50000m));
            agency.AddClient(new Client("Олена", "Коваленко", "53750000", PropertyType.LandPlot, 30000m));
            agency.AddProperty(new Apartment(PropertyType.OneRoomApartment, 45000m, 40.5, 3, "Київ", "Хрещатик", "15"));
            agency.AddProperty(new LandPlot(25000m, 1000.0, true, "Одеса", "Морська", "1"));
        }
    }
}