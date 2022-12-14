using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    [BsonDiscriminator("Customer")]
    public class Customer : User
    {
        public Customer(string loginName, string password,
            string firstName, string lastName,
            string personalIdentificationNumber, Address address
            ) : base(loginName, password)
        {
            FirstName = firstName;
            LastName = lastName;
            PID = personalIdentificationNumber;
            Address = address;
            IsApproved = false;
            IsBanned = false;
        }

        public Customer(string loginName, string password,
                    string firstName, string lastName,
                    string personalIdentificationNumber, Address address,
                    bool isApproved
                    ) : base(loginName, password)
        {
            FirstName = firstName;
            LastName = lastName;
            PID = personalIdentificationNumber;
            Address = address;
            IsApproved = isApproved;
            IsBanned = false;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PID { get; set; }
        public Address Address { get; set; }
        public bool IsApproved { get; set; }
        public bool IsBanned { get; set; }

        public string IsApprovedString => IsApproved ? "Yes" : "No";

        public string IsBannedString => IsBanned ? "Yes" : "No";
    }
}
