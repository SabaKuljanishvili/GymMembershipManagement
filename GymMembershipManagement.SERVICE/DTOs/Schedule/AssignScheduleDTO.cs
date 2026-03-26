namespace GymMembershipManagement.SERVICE.DTOs.Schedule
{
    public class AssignScheduleDTO
    {
        public int UserId { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public int Duration { get; set; }
        public int GymClassId { get; set; }
    }
}
