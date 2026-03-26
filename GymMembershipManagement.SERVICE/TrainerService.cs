using GymMembershipManagement.DAL.Repositories;
using GymMembershipManagement.DATA.Entities;
using GymMembershipManagement.SERVICE.DTOs.Schedule;
using GymMembershipManagement.SERVICE.Interfaces;

namespace GymMembershipManagement.SERVICE
{
    public class TrainerService : ITrainerService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public TrainerService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<bool> AssignSchedule(AssignScheduleDTO dto)
        {
            var schedule = new Schedule
            {
                UserId = dto.UserId,
                ScheduledDateTime = dto.ScheduledDateTime,
                Duration = dto.Duration,
                GymClassId = dto.GymClassId
            };
            await _scheduleRepository.AddAsync(schedule);
            return true;
        }

        public async Task<IEnumerable<ScheduleDTO>> GetSchedulesByTrainer(int trainerId)
        {
            var all = await _scheduleRepository.GetAllAsync();
            return all.Where(s => s.UserId == trainerId).Select(s => new ScheduleDTO
            {
                Id = s.Id,
                UserId = s.UserId,
                ScheduledDateTime = s.ScheduledDateTime,
                Duration = s.Duration,
                GymClassId = s.GymClassId,
                GymClassName = s.GymClass?.GymClassName
            });
        }
    }
}
