using Microsoft.EntityFrameworkCore;
using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Enums;
namespace NotificationCenter.Infrastructure.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<NotificationEvent> NotificationEvents { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationEvent>()
            .Property(e => e.TargetGroup)
            .HasConversion<string>(); // Store as string in the database

            modelBuilder.Entity<NotificationEvent>()
                .Property(e => e.Channel)
                .HasConversion<string>(); // Store as string in the database

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Certificates)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(cl => cl.Requests)
                .HasForeignKey(r => r.ClientId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Client)
                .WithMany(cl => cl.Notifications)
                .HasForeignKey(n => n.ClientId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Event)
                .WithMany(e => e.Notifications)
                .HasForeignKey(n => n.EventId);


            AddTestData(modelBuilder);
        }

        #region Test Data
        private static void AddTestData(ModelBuilder modelBuilder)
        {
            // Seed data for Clients
              modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Ivan Petrov", IsIndividual = true, Username = "ivan.petrov", PasswordHash = "hashed_password_1" },
                new Client { Id = 2, Name = "DayAndNight Soft EOOD", IsIndividual = false, Username = "dayAndNight.soft", PasswordHash = "hashed_password_2" }
            );


            // Seed data for NotificationEvents
            
            modelBuilder.Entity<NotificationEvent>().HasData(
               new NotificationEvent { Id = 1, EventName = "System Maintenance", TargetGroup = TargetGroup.All, Channel = Channel.Web },
               new NotificationEvent { Id = 2, EventName = "New Feature Launch", TargetGroup = TargetGroup.Individual, Channel = Channel.Mobile }
            );

            // Seed data for Certificates
            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, ClientId = 1, SerialNumber = "Q1w2e3r4", ValidFrom = new DateTime(2025, 1, 1), ValidTo = new DateTime(2025, 6, 1)}
            );

            // Seed data for Requests
            modelBuilder.Entity<Request>().HasData(
                new Request { Id = 1, ClientId = 1, RequestType = "Upgrade Plan", RequestDate = DateTime.Today.AddDays(-10), Status = "Pending" }
            );

            // Seed data for Notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification { Id = 1, ClientId = 1, EventId = 1, Message = "System maintenance scheduled for tomorrow.", IsRead = false, CreatedAt = new DateTime(2025, 1, 2) },
                new Notification { Id = 2, ClientId = 2, EventId = 2, Message = "Check out our new feature!", IsRead = true, CreatedAt = new DateTime(2025, 1, 1) }
            );
        }
        #endregion

    }
}