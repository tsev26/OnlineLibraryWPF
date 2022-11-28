using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    public class Address : IComparable
    {
        public Address(string street, string city, string postalCode, string country)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return Street + ", " + City + ", " + PostalCode + ", " + Country;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Address address)
            {
                return Street == address.Street && City == address.City && PostalCode == address.PostalCode && Country == address.Country ;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, PostalCode, Country);
        }

        public int CompareTo(object? obj)
        {
            return this.Equals(obj) ? 0 : -1;
        }
    }
}
