using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEMS.Model.Entity
{
    public class UserEntry : BaseEntity
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public UserEntry():base("UserEntity"){}
    }
}
