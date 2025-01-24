using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using ToDoList.Migrations;

namespace ToDoList.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Category> Categories { get; set; }

        // seed data

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", StatusName = "Open" },
                new Status { StatusId = "closed", StatusName = "Completed" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = "work", CategoryName = "Work" },
                new Category { CategoryId = "home", CategoryName = "Home" },
                new Category { CategoryId = "ex", CategoryName = "Exercise" },
                new Category { CategoryId = "shop", CategoryName = "Shopping" },
                new Category { CategoryId = "call", CategoryName = "Contact" }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "d480e7d1-6f02-4f05-a3c5-470d5b6d8e3d", 
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "a6b6c5d4-1234-5678-abcd-1a2b3c4d5e6f", 
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

        }



    }
}
