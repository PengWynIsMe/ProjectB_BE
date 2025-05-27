using Microsoft.EntityFrameworkCore;
using Project_B.Models;
using System.Data;

namespace Project_B.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetStatus> BudgetStatuses { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventNotification> EventNotifications { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Cấu hình mối quan hệ giữa User và Goal (1-nhiều)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Goals)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa User và Meal (1-nhiều)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Meals)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa User và Location (1-nhiều)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Locations)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa Role và RoleUser (nhiều-nhiều)
            modelBuilder.Entity<RoleUser>()
                .HasKey(ru => new { ru.RoleId, ru.UserId });

            // Cấu hình mối quan hệ giữa RoleUser và Role
            modelBuilder.Entity<RoleUser>()
               .HasOne(ru => ru.Role)
               .WithMany()
               .HasForeignKey(ru => ru.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa RoleUser và User
            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.User)
                .WithMany()
                .HasForeignKey(ru => ru.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Mối quan hệ giữa Budget và User
            modelBuilder.Entity<User>()
                .HasMany(u => u.Budgets)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Cascade);


        }
        public DbSet<Project_B.Models.BudgetNotification> BudgetNotification { get; set; } = default!;
        public DbSet<Project_B.Models.Transaction> Transaction { get; set; } = default!;

    }
}
