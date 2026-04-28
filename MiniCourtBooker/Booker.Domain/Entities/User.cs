using System;
using System.Collections.Generic;
using System.Text;

namespace Booker.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }

        public User() 
        { 
            Id = Guid.NewGuid();
        }
    }
}
