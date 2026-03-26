namespace GymMembershipManagement.SERVICE.DTOs.Membership
{
    public class RegisterMembershipDTO
    {
        public int UserId { get; set; }
        public int MembershipTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}
