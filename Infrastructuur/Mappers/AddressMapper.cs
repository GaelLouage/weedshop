using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Mappers
{
    public static class AddressMapper
    {
        public static AddressEntity MapToAddress(this AddressEntity address)
        {
            return new AddressEntity
            {
                Address = address.Address,
                AddressNumber = address.AddressNumber,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode
            };
        }
    }
}
