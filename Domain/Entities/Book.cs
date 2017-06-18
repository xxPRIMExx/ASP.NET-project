using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Domain.Entities
{
    public class Book
    {
        public Book(string title)
        {
            this.Title = title;
            Users = new List<User>();
            Autors = new List<Autor>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Autor> Autors { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public Book()
        {
            Users = new List<User>();
            Autors = new List<Autor>();
        }
    }
}
