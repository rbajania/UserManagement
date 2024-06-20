using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DTOs
{
    public class UpdateUserDto
    {
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


    }
}