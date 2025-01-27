using Azure.Core;
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
        public DbSet<Domain.Entities.Request> Requests { get; set; }
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

            modelBuilder.Entity<NotificationEvent>()
                .HasOne(ne => ne.Request)
                .WithMany()
                .HasForeignKey(ne => ne.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotificationEvent>()
                .HasOne(ne => ne.Certificate)
                .WithMany()
                .HasForeignKey(ne => ne.CertificateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotificationEvent>()
                .Property(ne => ne.CreateTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Certificates)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Domain.Entities.Request>()
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
            //For passwordHash we have BCrypt.Net.BCrypt.HashPassword("password123") = $2a$11$TjtUsIOhCW5WXaboB4T5sO0io6ogMD.T68yeKDvii20EkcsPX.xNm

            // Seed data for Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Ivan Petrov", IsIndividual = true, Username = "ivan.petrov", PasswordHash = "$2a$11$TjtUsIOhCW5WXaboB4T5sO0io6ogMD.T68yeKDvii20EkcsPX.xNm" },
                new Client { Id = 2, Name = "Ivana Ivanova", IsIndividual = true, Username = "ivana.ivanova", PasswordHash = "$2a$11$TjtUsIOhCW5WXaboB4T5sO0io6ogMD.T68yeKDvii20EkcsPX.xNm" },
                new Client { Id = 3, Name = "Art Soft OOD", IsIndividual = false, Username = "art.soft", PasswordHash = "$2a$11$TjtUsIOhCW5WXaboB4T5sO0io6ogMD.T68yeKDvii20EkcsPX.xNm" },
                new Client { Id = 4, Name = "D&N Soft EOOD", IsIndividual = false, Username = "dandn.soft", PasswordHash = "$2a$11$TjtUsIOhCW5WXaboB4T5sO0io6ogMD.T68yeKDvii20EkcsPX.xNm" }
            );

            // Seed data for NotificationEvents            
            modelBuilder.Entity<NotificationEvent>().HasData(
               new NotificationEvent { Id = 1, EventName = "New Company Feature Launch", TargetGroup = TargetGroup.AllCompany, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 1) },
               new NotificationEvent { Id = 2, EventName = "New Individual Feature Launch", TargetGroup = TargetGroup.AllIndividual, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 1) },
               new NotificationEvent { Id = 3, CertificateId = 4, EventName = "Certificate is expired", TargetGroup = TargetGroup.Individual, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 2)},
               new NotificationEvent { Id = 4, RequestId = 1, EventName = "Request status changed: Created - > Pending", TargetGroup = TargetGroup.Individual, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 2) },
               new NotificationEvent { Id = 5, RequestId = 2, EventName = "Request status changed: Created - > Pending", TargetGroup = TargetGroup.Company, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 2) },
               new NotificationEvent { Id = 6, RequestId = 2, EventName = "Request status changed: Pending - > InProgress", TargetGroup = TargetGroup.Company, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 3) },
               new NotificationEvent { Id = 7, EventName = "System Maintenance", TargetGroup = TargetGroup.All, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 27) },
               new NotificationEvent { Id = 8, RequestId = 2, EventName = "Request status changed: InProgress - > Done", TargetGroup = TargetGroup.Company, Channel = Channel.Web , CreateTime = new DateTime(2025, 1, 27) },
               new NotificationEvent { Id = 9, CertificateId = 2, EventName = "Certificate is expired", TargetGroup = TargetGroup.Individual, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 27) },
               new NotificationEvent { Id = 10, CertificateId = 3, EventName = "Certificate is expired", TargetGroup = TargetGroup.Individual, Channel = Channel.Web, CreateTime = new DateTime(2025, 1, 27) } 
            );

            // Seed data for Certificates
            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, ClientId = 1, SerialNumber = "1Q1w2e3r4", ValidFrom = new DateTime(2025, 1, 1), ValidTo = new DateTime(2025, 6, 1)},
                new Certificate { Id = 2, ClientId = 2, SerialNumber = "2Q1w2e3r4", ValidFrom = new DateTime(2025, 1, 1), ValidTo = new DateTime(2025, 1, 26) },
                new Certificate { Id = 3, ClientId = 3, SerialNumber = "3Q1w2e3r4", ValidFrom = new DateTime(2025, 1, 1), ValidTo = new DateTime(2025, 1, 26) },
                new Certificate { Id = 4, ClientId = 4, SerialNumber = "4Q1w2e3r4", ValidFrom = new DateTime(2025, 1, 1), ValidTo = new DateTime(2025, 1, 1) }
            );

            // Seed data for Requests
            modelBuilder.Entity<Domain.Entities.Request>().HasData(
                new Domain.Entities.Request { Id = 1, ClientId = 1, RequestType = "Upgrade Plan", RequestDate = DateTime.Today.AddDays(-10), Status = "Pending" },
                new Domain.Entities.Request { Id = 2, ClientId = 3, RequestType = "Upgrade Plan", RequestDate = DateTime.Today.AddDays(-10), Status = "Done" }
            );

            // Seed data for Notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification { Id = 1, ClientId = 4, EventId = 1, Message = "Check out our new feature for companies!", IsRead = true, CreatedAt = new DateTime(2025, 1, 1) },
                new Notification { Id = 2, ClientId = 3, EventId = 1, Message = "Check out our new feature for companies!", IsRead = true, CreatedAt = new DateTime(2025, 1, 1) },
                new Notification { Id = 3, ClientId = 1, EventId = 2, Message = "Check out our new feature!", IsRead = true, CreatedAt = new DateTime(2025, 1, 1) },
                new Notification { Id = 4, ClientId = 2, EventId = 2, Message = "Check out our new feature!", IsRead = true, CreatedAt = new DateTime(2025, 1, 1) },
                new Notification { Id = 5, ClientId = 4, EventId = 3, Message = "Your certificate No: 4 is expired. Please contact support.", IsRead = false, CreatedAt = new DateTime(2025, 1, 2) },
                new Notification { Id = 6, ClientId = 1, EventId = 4, Message = "Request No: 1 /for Upgrade Plan/ - status changed: Created - > Pending", IsRead = false, CreatedAt = new DateTime(2025, 1, 2) },
                new Notification { Id = 7, ClientId = 3, EventId = 5, Message = "Request No: 2 /for Upgrade Plan/ - status changed: Created - > Pending", IsRead = true, CreatedAt = new DateTime(2025, 1, 2) },
                new Notification { Id = 8, ClientId = 3, EventId = 6, Message = "Request No: 2 /for Upgrade Plan/ - status changed: Pending - > InProgress", IsRead = true, CreatedAt = new DateTime(2025, 1, 3) },
                new Notification { Id = 9, ClientId = 1, EventId = 7, Message = "System maintenance scheduled for tomorrow.", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) },
                new Notification { Id = 10, ClientId = 2, EventId = 7, Message = "System maintenance scheduled for tomorrow.", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) },
                new Notification { Id = 11, ClientId = 3, EventId = 7, Message = "System maintenance scheduled for tomorrow.", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) },
                new Notification { Id = 12, ClientId = 4, EventId = 7, Message = "System maintenance scheduled for tomorrow.", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) },
                new Notification { Id = 13, ClientId = 3, EventId = 8, Message = "Request No: 2 /for Upgrade Plan/ - status changed: InProgress - > Done", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) },
                new Notification { Id = 14, ClientId = 2, EventId = 9, Message = "Your certificate No:2 is expired. Please contact support.", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) },
                new Notification { Id = 15, ClientId = 3, EventId = 10, Message = "Your certificate No: 3 is expired. Please contact support.", IsRead = false, CreatedAt = new DateTime(2025, 1, 27) }
            );
        }
        #endregion

    }
}