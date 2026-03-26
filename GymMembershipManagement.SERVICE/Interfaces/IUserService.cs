using GymMembershipManagement.SERVICE.DTOs.User;

namespace GymMembershipManagement.SERVICE.Interfaces
{
    public interface IUserService
    {
        Task UserRegistration(UserRegisterModel model);
        Task<UserDTO> Login(string username, string password);
        Task<UserDTO> GetProfile(int userId);
        Task UpdateProfile(int userId, UpdateUserModel model);
        Task DeleteProfile(int userId);
        void Logout();
    }
}
