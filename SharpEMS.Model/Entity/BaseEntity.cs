using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEMS.Model.Entity
{
    public class BaseEntity
    {
        protected string ClassName { get; set; }

        protected BaseEntity(string className)
        {
            this.ClassName = className;
        }
    }
}
