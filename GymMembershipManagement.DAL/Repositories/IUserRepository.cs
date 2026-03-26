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
    public interface IUserRepository : IBaseRepository<User>
    {
    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly GymDbContext _context;
        public UserRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
