using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRT.Models;

namespace BRT.Models
{
    public class User 
    {
        public long UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string CNIC { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public bool? Status { get; set; }

        public string IdentityUserId { get; set; }

        public IdentityUser IdentityUser { get; set; }

        public long? ShippingAgentId { get; set; }

      

        public long? ShippingAgentExportId { get; set; }

      

        public long? CompanyId { get; set; }

       
    }
}
