namespace RealEstateAgency.Models
{
    /// <summary>
    /// Власне виключення для ситуації, коли перевищено ліміт пропозицій (n >= 5).
    /// </summary>
    public class OfferLimitExceededException : Exception
    {
        public OfferLimitExceededException(string message) : base(message) { }
    }

    /// <summary>
    /// Типи нерухомості згідно з завданням.
    /// </summary>
    public enum PropertyType
    {
        OneRoomApartment,
        TwoRoomApartment,
        ThreeRoomApartment,
        LandPlot
    }

    /// <summary>
    /// Відношення "Реалізація" (Realization).
    /// </summary>
    public interface ISearchable
    {
        bool MatchesKeyword(string keyword);
    }

    /// <summary>
    /// Клас адреси для демонстрації відношення "Композиція".
    /// </summary>
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }

        public Address(string city, string street, string building)
        {
            City = city;
            Street = street;
            Building = building;
        }

        public override string ToString() => $"{City}, {Street}, {Building}";
    }

    /// <summary>
    /// Базовий абстрактний клас нерухомості.
    /// </summary>
    public abstract class Property : ISearchable
    {
        public Guid Id { get; private set; }
        public decimal Price { get; set; }
        public double Area { get; set; }
        public PropertyType Type { get; protected set; }

        /// <summary>
        /// Відношення "Композиція" (Composition). 
        /// </summary>
        public Address Location { get; private set; }

        protected Property(decimal price, double area, string city, string street, string building)
        {
            Id = Guid.NewGuid();
            Price = price;
            Area = area;
            Location = new Address(city, street, building);
        }

        public bool MatchesKeyword(string keyword)
        {
            // Залишено лише перевірку на null, щоб .Contains не викликав помилку
            if (keyword == null) return false;
            
            return Location.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                   Type.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"[{Type}] Ціна: {Price} | Площа: {Area} | Адреса: {Location}";
        }
    }

    /// <summary>
    /// Відношення "Узагальнення" (Generalization).
    /// </summary>
    public class Apartment : Property
    {
        public int Floor { get; set; }

        public Apartment(PropertyType type, decimal price, double area, int floor, string city, string street, string building) 
            : base(price, area, city, street, building)
        {
            Type = type;
            Floor = floor;
        }
    }

    /// <summary>
    /// Відношення "Узагальнення" (Generalization).
    /// </summary>
    public class LandPlot : Property
    {
        public bool HasUtilities { get; set; }

        public LandPlot(decimal price, double area, bool hasUtilities, string city, string street, string building) 
            : base(price, area, city, street, building)
        {
            Type = PropertyType.LandPlot;
            HasUtilities = hasUtilities;
        }
    }

    /// <summary>
    /// Клас клієнта.
    /// </summary>
    public class Client : ISearchable
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccountNumber { get; set; }
        public PropertyType DesiredType { get; set; }
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Відношення "Агрегація" (Aggregation).
        /// </summary>
        private List<Property> _offers = new List<Property>();

        public IReadOnlyList<Property> Offers => _offers.AsReadOnly();

        public Client(string firstName, string lastName, string bankAccount, PropertyType desiredType, decimal maxPrice)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            BankAccountNumber = bankAccount;
            DesiredType = desiredType;
            MaxPrice = maxPrice;
        }

        public void AddOffer(Property property)
        {
            // Залишено перевірку бізнес-логіки згідно з ТЗ (n < 5)
            if (_offers.Count >= 4)
                throw new OfferLimitExceededException("Клієнт не може мати більше 4 пропозицій (n < 5).");
            
            if (property != null && !_offers.Contains(property))
                _offers.Add(property);
        }

        public void RejectOffer(Property property)
        {
            if (property != null)
                _offers.Remove(property);
        }

        public bool MatchesKeyword(string keyword)
        {
            // Залишено лише перевірку на null
            if (keyword == null) return false;

            return (FirstName != null && FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                   (LastName != null && LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} | Рахунок: {BankAccountNumber} | Бажає: {DesiredType} (до {MaxPrice})";
        }
    }

    /// <summary>
    /// Відношення "Залежність" (Dependency).
    /// </summary>
    public static class PropertyMatcher
    {
        public static bool IsMatch(Client client, Property property)
        {
            if (client == null || property == null) return false;
            return property.Type == client.DesiredType && property.Price <= client.MaxPrice;
        }
    }

    /// <summary>
    /// Головний клас агентства.
    /// </summary>
    public class RealtorAgency
    {
        private List<Client> _clients = new List<Client>();
        private List<Property> _properties = new List<Property>();

        public IReadOnlyList<Client> Clients => _clients.AsReadOnly();
        public IReadOnlyList<Property> Properties => _properties.AsReadOnly();

        public void AddClient(Client client)
        {
            if (client != null) _clients.Add(client);
        }

        public void RemoveClient(Client client)
        {
            if (client != null) _clients.Remove(client);
        }

        public void UpdateClient(Client client, string firstName, string lastName, string bankAccount, PropertyType desiredType, decimal maxPrice)
        {
            if (client == null) return; // Перевірка на null
            
            client.FirstName = firstName;
            client.LastName = lastName;
            client.BankAccountNumber = bankAccount;
            client.DesiredType = desiredType;
            client.MaxPrice = maxPrice;
        }

        public IEnumerable<Client> GetClientsSortedByName() => _clients.OrderBy(c => c.FirstName);

        public IEnumerable<Client> GetClientsSortedByLastName() => _clients.OrderBy(c => c.LastName);

        public IEnumerable<Client> GetClientsSortedByAccountFirstDigit() 
            => _clients.OrderBy(c => string.IsNullOrEmpty(c.BankAccountNumber) ? '0' : c.BankAccountNumber[0]);

        public void AddProperty(Property property)
        {
            if (property != null) _properties.Add(property);
        }

        public void RemoveProperty(Property property)
        {
            if (property == null) return; // Перевірка на null
            
            _properties.Remove(property);
            foreach (var client in _clients)
            {
                client.RejectOffer(property);
            }
        }

        public void UpdatePropertyBasicData(Property property, decimal newPrice, double newArea)
        {
            if (property == null) return; // Перевірка на null
            
            property.Price = newPrice;
            property.Area = newArea;
        }

        public IEnumerable<Property> GetPropertiesSortedByType() => _properties.OrderBy(p => p.Type);

        public IEnumerable<Property> GetPropertiesSortedByPrice() => _properties.OrderBy(p => p.Price);

        public IEnumerable<Property> FindAvailablePropertiesForClient(Client client)
        {
            if (client == null) return Enumerable.Empty<Property>();
            return _properties.Where(p => PropertyMatcher.IsMatch(client, p));
        }

        public IEnumerable<Client> SearchClients(string keyword) => _clients.Where(c => c.MatchesKeyword(keyword));

        public IEnumerable<Property> SearchProperties(string keyword) => _properties.Where(p => p.MatchesKeyword(keyword));

        public IEnumerable<ISearchable> SearchAll(string keyword)
        {
            var results = new List<ISearchable>();
            results.AddRange(_clients.Where(c => c.MatchesKeyword(keyword)));
            results.AddRange(_properties.Where(p => p.MatchesKeyword(keyword)));
            return results;
        }

        public IEnumerable<Client> AdvancedClientSearch(string lastName, PropertyType desiredType)
        {
            if (lastName == null) return Enumerable.Empty<Client>();
            
            return _clients.Where(c => string.Equals(c.LastName, lastName, StringComparison.OrdinalIgnoreCase) 
                                    && c.DesiredType == desiredType);
        }
    }
}