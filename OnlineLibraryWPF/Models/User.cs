using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    [BsonKnownTypes(typeof(Customer), typeof(Librarian))]
    [BsonIgnoreExtraElements]
    public class User
    {
        public User(string loginName, string password)
        {
            LoginName = loginName;
            Password = password;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        /*
        [BsonElement("_t")]
        public string Type { get; set; }
        */

        public string LoginName { get; set; }
        public string Password { get; set; }
    }
}
