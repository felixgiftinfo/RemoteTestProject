using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Domains
{
    public class EntityBase
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonDateTimeOptions]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [BsonDateTimeOptions]
        public DateTime DateModified { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
