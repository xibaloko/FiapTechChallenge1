using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FiapTechChallenge.Domain.Entities
{
    public class Person : EntityCore
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        //é necessario?
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }

}
