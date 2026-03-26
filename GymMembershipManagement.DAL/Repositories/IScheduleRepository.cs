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
    public interface IScheduleRepository : IBaseRepository<Schedule>
    {
    }
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        private readonly GymDbContext _context;
        public ScheduleRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
