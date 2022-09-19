using System.Text.Json.Serialization;

namespace DomainLayer.DomainModels;

public class Customer
{
        // public int? CustomerId { get; set; }
        public bool NameStyle { get; set; } 
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = null!;
        // Pass without encription
        public string PasswordSalt { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        
        public Customer()
        {
                this.ModifiedDate = DateTime.Today;
        }
    
}    
