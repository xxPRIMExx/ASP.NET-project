using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : Person
    {
        public virtual ICollection<Book> Books { get; set; }
        public User()
        {
            Books = new List<Book>();
        }
    
    
    }
}
