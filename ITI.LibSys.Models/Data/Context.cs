using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.LibSys.Models.Data
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //For Configure Attributes of Columns in Database
            new BookConfig().Configure(modelBuilder.Entity<Book>());
            new AuthorConfig().Configure(modelBuilder.Entity<Author>());
            new BookImageConfig().Configure(modelBuilder.Entity<BookImage>());

            //For Create Relations Between Tables and Configure the Constains
            modelBuilder.MapRelations();
            //To configure tables of identity
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set;}
        public DbSet<BookImage> BookImages { get; set; }
    }
}
