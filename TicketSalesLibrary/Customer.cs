using System;

namespace TicketSalesLibrary
{
    [Serializable]
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Customer(string name
            , string address
            , string phone
            , string email)
        {
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
        }
    }
}
