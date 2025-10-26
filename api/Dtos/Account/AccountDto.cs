using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class AccountDto
    {
        public string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}