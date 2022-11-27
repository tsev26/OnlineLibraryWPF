using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    [BsonDiscriminator("Librarian")]
    public class Librarian : User
    {
        public Librarian(string loginName, string password) : base(loginName, password)
        {
        }


    }
}
