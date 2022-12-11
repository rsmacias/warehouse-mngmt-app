using System;
using System.Collections.Generic;

namespace warehouseManagementSystem.Infrastructure
{
    public class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        private byte[]? logo;  // backing field
        public byte[]? Logo {
            get {  // Lazy Initialization
                if (logo is null) {
                    logo = LogoService.GetFor(Name);
                }
                return logo;
            } 
            set => logo = value; 
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}