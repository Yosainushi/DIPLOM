using Microsoft.EntityFrameworkCore;
using Диплом2.Controllers;

namespace Диплом2.Models
{
    public class AppDBContext : DbContext 
    {
        public DbSet<User> User { get; set; }
        public DbSet<Chat> Chat { get; set; }

        public DbSet<FileModel> Files { get; set; }

        public DbSet<Letter> Letter { get; set; }
        public AppDBContext() : base(new DbContextOptionsBuilder().UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = diplom2; Trusted_Connection = True;").Options) { }
        public AppDBContext(DbContextOptions<AppDBContext> options)
          : base((DbContextOptions)options)
          => this.Database.EnsureCreated();
    }
}