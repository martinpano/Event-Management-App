using Microsoft.EntityFrameworkCore;

namespace EventManager.Db
{
    public class EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : DbContext(options)
    {

        // DbSets (Tables)
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        //public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Email Constraint for Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // User-Booking Relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Event-Booking Relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem Relationship
            //modelBuilder.Entity<CartItem>()
            //    .HasOne<User>()
            //    .WithMany()
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<CartItem>()
            //    .HasOne<Event>()
            //    .WithMany()
            //    .HasForeignKey(c => c.EventId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// Audit Log Relationship
            //modelBuilder.Entity<AuditLog>()
            //    .HasOne(a => a.PerformedBy)
            //    .WithMany()
            //    .HasForeignKey(a => a.PerformedByUserId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
