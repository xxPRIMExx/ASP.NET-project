using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Autor : Person
    {
        public Autor(string name, string lastName)
        {
            this.Name = name;
            this.LastName = lastName;
            Books = new List<Book>();
        }

        public virtual ICollection<Book> Books { get; set; }

        public Autor()
        {
            Books = new List<Book>();
        }
    }
}
