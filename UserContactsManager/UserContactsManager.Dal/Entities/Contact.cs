using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserContactsManager.Dal.Entities;

public class Contact
{
    public long ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; } 
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public long UserId { get; set; }
    public User User { get; set; } 


}
