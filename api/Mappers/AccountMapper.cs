using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;

namespace api.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this User userModel)
        {
            var userName = userModel.UserName ?? throw new InvalidOperationException("User record missing username.");
            return new AccountDto
            {
                UserName = userName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };
        }
    }
}
