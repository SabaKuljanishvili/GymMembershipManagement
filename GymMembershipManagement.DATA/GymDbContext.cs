using GymMembershipManagement.DATA.Configuration;
using GymMembershipManagement.DATA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMembershipManagement.DATA
{
    public class GymDbContext : DbContext
    {

        public GymDbContext()
        {

        }


        public GymDbContext(DbContextOptions<GymDbContext> context) : base(context)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Uncomment and configure your connection string if needed
            // if (!optionsBuilder.IsConfigured)
            // {
            //     var configuration = new ConfigurationBuilder()
            //         .SetBasePath(AppContext.BaseDirectory)
            //         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //         .Build();
            //
            //     var connectionString = configuration.GetConnectionString("GymDbConnection");
            //     optionsBuilder.UseSqlServer(connectionString);
            // }
        }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        public DbSet<MembershipType> MembershipTypes { get; set; } = null!;
        public DbSet<GymClass> GymClasses { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply Configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new MembershipConfiguration());
            modelBuilder.ApplyConfiguration(new MembershipTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GymClassConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            // Roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    RoleName = "Admin"
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "Customer"
                }
            );

            // MembershipTypes
            modelBuilder.Entity<MembershipType>().HasData(
                new MembershipType
                {
                    Id = 1,
                    MembershipTypeName = "Monthly"
                },
                new MembershipType
                {
                    Id = 2,
                    MembershipTypeName = "Yearly"
                },
                new MembershipType
                {
                    Id = 3,
                    MembershipTypeName = "VIP"
                }
            );

            // GymClasses
            modelBuilder.Entity<GymClass>().HasData(
                new GymClass
                {
                    Id = 1,
                    GymClassName = "Wrestling"
                },
                new GymClass
                {
                    Id = 2,
                    GymClassName = "Judo"
                },
                new GymClass
                {
                    Id = 3,
                    GymClassName = "Karate"
                },
                new GymClass
                {
                    Id = 4,
                    GymClassName = "Boxing"
                }
            );



        }
    }
}
