using GymMembershipManagement.DATA;
using GymMembershipManagement.DATA.Entities;
using GymMembershipManagement.DATA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMembershipManagement.DAL.Repositories
{
    public interface IMembershipRepository : IBaseRepository<Membership>
    {
    }
    public class MembershipRepository : BaseRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _context;
        public MembershipRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
