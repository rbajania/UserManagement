
using MongoDB.Bson;

namespace Events;
public class UserCreated
{
    public ObjectId _id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public long ContactNumber { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public long PinCode { get; set; }
    public string UserType { get; set; }
    public List<string> Skills { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
