namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string firstName, string lastName, string streets, string city, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Streets = streets;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Streets { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}