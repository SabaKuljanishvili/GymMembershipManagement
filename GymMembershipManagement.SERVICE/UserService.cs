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
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password are required");

            var user = await _userRepository.GetByUsernameAsync(username);

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
            if (user == null) 
                throw new KeyNotFoundException($"User with ID {userId} not found");

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
            if (user == null) 
                throw new KeyNotFoundException($"User with ID {userId} not found");

            bool userChanged = false;
            bool personChanged = false;

            if (!string.IsNullOrWhiteSpace(model.Username) && model.Username != user.Username)
            {
                user.Username = model.Username;
                userChanged = true;
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                user.Email = model.Email;
                userChanged = true;
            }

            if (user.Person != null)
            {
                if (!string.IsNullOrWhiteSpace(model.FirstName) && model.FirstName != user.Person.FirstName)
                {
                    user.Person.FirstName = model.FirstName;
                    personChanged = true;
                }

                if (!string.IsNullOrWhiteSpace(model.LastName) && model.LastName != user.Person.LastName)
                {
                    user.Person.LastName = model.LastName;
                    personChanged = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) && model.Phone != user.Person.Phone)
                {
                    user.Person.Phone = model.Phone;
                    personChanged = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Address) && model.Address != user.Person.Address)
                {
                    user.Person.Address = model.Address;
                    personChanged = true;
                }

                if (personChanged)
                    await _personRepository.UpdateAsync(user.Person);
            }

            if (userChanged)
                await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteProfile(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) 
                throw new KeyNotFoundException($"User with ID {userId} not found");

            await _userRepository.DeleteAsync(userId);
        }

        public void Logout()
        {
            // JWT-ის შემთხვევაში client-side token წაშლა
        }
    }
}
