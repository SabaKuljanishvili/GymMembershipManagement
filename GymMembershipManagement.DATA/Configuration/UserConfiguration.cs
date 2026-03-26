using GymMembershipManagement.DATA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMembershipManagement.DATA.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users")
                .HasKey(u => u.UserId);
            builder.Property(u => u.Username)
                        .HasMaxLength(30)
                        .IsRequired();

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired();

            builder.Property(u => u.RegistrationDate)
                   .IsRequired();

            // User => Person 
            builder.HasOne(u => u.Person)
                   .WithOne(p => p.User)
                   .HasForeignKey<User>(u => u.PersonId);

            // User => Reservations 
            builder.HasMany(u => u.Reservations)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.NoAction); 

            // User => Memberships 
            builder.HasMany(u => u.Memberships)
                   .WithOne(m => m.User)
                   .HasForeignKey(m => m.UserId)
                   .OnDelete(DeleteBehavior.Cascade); 

            // User => Schedules 
            builder.HasMany(u => u.Schedules)
                   .WithOne(s => s.User)
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
