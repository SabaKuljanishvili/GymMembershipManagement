using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMembershipManagement.DATA.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public DateTime RegistrationDate { get; set; }
        public int PersonId { get; set; }


        // User => Person
        public Person? Person { get; set; }

        // User => Reservations
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // User => Memberships
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

        // User => Schedules 
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    }
}

