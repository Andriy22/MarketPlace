using MarketPlace.Entities.DBEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class DBContext : IdentityDbContext<User>
    {
        public DBContext(DbContextOptions options)
       : base(options)
        {
        }
        public new DbSet<User> Users { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<ChatMassage> ChatMassages { get; set; }
    }
}