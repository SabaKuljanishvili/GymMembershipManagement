using GymMembershipManagement.DAL.Repositories;
using GymMembershipManagement.DATA.Entities;
using GymMembershipManagement.SERVICE.DTOs.User;
using GymMembershipManagement.SERVICE.Interfaces;

namespace GymMembershipManagement.SERVICE
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        public AdminService(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<UserDTO> AddUser(UserRegisterModel model)
        {
            var person = new Person
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Address = model.Address
            };
            await _personRepository.AddAsync(person);

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email,
                RegistrationDate = DateTime.UtcNow,
                PersonId = person.PersonId
            };
            await _userRepository.AddAsync(user);

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate,
                FirstName = user.Person?.FirstName,
                LastName = user.Person?.LastName
            };
        }

        public async Task UpdateUserDetails(int userId, UpdateUserModel model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (model.Username != null) user.Username = model.Username;
            if (model.Email != null) user.Email = model.Email;

            if (user.Person != null)
            {
                if (model.FirstName != null) user.Person.FirstName = model.FirstName;
                if (model.LastName != null) user.Person.LastName = model.LastName;
                if (model.Phone != null) user.Person.Phone = model.Phone;
                if (model.Address != null) user.Person.Address = model.Address;
                await _personRepository.UpdateAsync(user.Person);
            }

            await _userRepository.UpdateAsync(user);
        }

        public async Task RemoveUser(int userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<IEnumerable<UserDTO>> GetAllMembers()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                RegistrationDate = u.RegistrationDate,
                FirstName = u.Person?.FirstName,
                LastName = u.Person?.LastName
            });
        }

        public async Task<IEnumerable<UserDTO>> GetAllTrainers()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                RegistrationDate = u.RegistrationDate,
                FirstName = u.Person?.FirstName,
                LastName = u.Person?.LastName
            });
        }
    }
}
