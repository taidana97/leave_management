using Microsoft.AspNetCore.Identity; // :Employee
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Data
{
    // Employee ke thua Employee
    public class Employee:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
      
    }
}
