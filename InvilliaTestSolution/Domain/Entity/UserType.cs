using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Entity
{
    public class UserType
    {
        public UserType()
        {
        }
        public UserType(int typeId)
        {
            TypeId = typeId;
        }
        public int TypeId { get; set; }
        public string Type { get; set; }
    }
}
