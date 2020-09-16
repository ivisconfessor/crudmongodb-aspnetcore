using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudMongoDB.Models
{
    public class Book
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string YearPublish { get; set; }
        public string Description { get; set; }
    }
}
