using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required] 
        public string Email { get; set; }
        [Required] 
        public long ContactNumber { get; set; }
        [Required] 
        public int Age { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required] 
        public string City { get; set; }
        [Required] 
        public long PinCode { get; set; }
        [Required] 
        public string UserType { get; set; }
        public List<string> Skills { get; set; }
    }
}