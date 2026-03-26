using GymMembershipManagement.DAL.Repositories;
using GymMembershipManagement.DATA.Entities;
using GymMembershipManagement.SERVICE.DTOs.User;
using GymMembershipManagement.SERVICE.Interfaces;

namespace GymMembershipManagement.SERVICE
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        public UserService(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task UserRegistration(UserRegisterModel model)
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
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

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

        public async Task<UserDTO> GetProfile(int userId)
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

        public async Task UpdateProfile(int userId, UpdateUserModel model)
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

        public async Task DeleteProfile(int userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public void Logout()
        {
            // JWT-ის შემთხვევაში client-side token წაშლა
        }
    }
}
