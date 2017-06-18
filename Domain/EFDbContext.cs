using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;

namespace Domain
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }
        // new //
        public DbSet<Book> Books { get; set; }
        public DbSet<Autor> Autors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Autor>().HasMany(b => b.Books)
                .WithMany(a => a.Autors)
                .Map(t => t.MapLeftKey("autorID")
                .MapRightKey("bookID")
                .ToTable("AutorBook"));

            modelBuilder.Entity<User>().HasMany(b => b.Books)
                .WithMany(u => u.Users)
                .Map(t => t.MapLeftKey("userID")
                .MapRightKey("bookID")
                .ToTable("UserBook"));
        }
    }
}
