using GymMembershipManagement.DAL.Repositories;
using GymMembershipManagement.DATA.Entities;
using GymMembershipManagement.SERVICE.DTOs.Membership;
using GymMembershipManagement.SERVICE.Interfaces;

namespace GymMembershipManagement.SERVICE
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<MembershipDTO> RegisterMembership(RegisterMembershipDTO dto)
        {
            var membership = new Membership
            {
                UserId = dto.UserId,
                MembershipTypeId = dto.MembershipTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Price = dto.Price
            };
            await _membershipRepository.AddAsync(membership);

            return MapToDTO(membership);
        }

        public async Task<bool> RenewMembership(int membershipId)
        {
            var membership = await _membershipRepository.GetByIdAsync(membershipId);
            if (membership == null) return false;

            membership.StartDate = DateTime.UtcNow;
            membership.EndDate = DateTime.UtcNow.AddMonths(1);
            await _membershipRepository.UpdateAsync(membership);
            return true;
        }

        public async Task<MembershipStatusDTO> GetMembershipStatus(int customerId)
        {
            var memberships = await _membershipRepository.GetAllAsync();
            var membership = memberships
                .Where(m => m.UserId == customerId)
                .OrderByDescending(m => m.EndDate)
                .FirstOrDefault();

            if (membership == null)
                return new MembershipStatusDTO { UserId = customerId, IsActive = false };

            return new MembershipStatusDTO
            {
                UserId = customerId,
                IsActive = membership.IsActive,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                MembershipTypeName = membership.MembershipType?.MembershipTypeName ?? ""
            };
        }

        public async Task<IEnumerable<MembershipDTO>> GetMembershipsByUser(int userId)
        {
            var all = await _membershipRepository.GetAllAsync();
            return all.Where(m => m.UserId == userId).Select(MapToDTO);
        }

        public async Task<IEnumerable<MembershipDTO>> GetMembershipsByStatus()
        {
            var all = await _membershipRepository.GetAllAsync();
            return all.Where(m => m.IsActive).Select(MapToDTO);
        }

        private MembershipDTO MapToDTO(Membership m) => new MembershipDTO
        {
            Id = m.Id,
            UserId = m.UserId,
            MembershipTypeId = m.MembershipTypeId,
            MembershipTypeName = m.MembershipType?.MembershipTypeName ?? "",
            StartDate = m.StartDate,
            EndDate = m.EndDate,
            Price = m.Price,
            IsActive = m.IsActive
        };
    }
}
