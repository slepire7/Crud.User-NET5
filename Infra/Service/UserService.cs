using Core.DTO;
using Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCrypt.Net.BCrypt;
namespace Infra.Service
{
    public interface IUserService
    {
        Task NewUser(Core.DTO.UserDTO newUser);
        Task UpdateUser(Core.DTO.UserDTO dTO, Guid UserId);
        Task DeleteUser(Guid Userid);
        Task<IEnumerable<UserView>> GetAllUserView();
        Task<UserView> GetUser(Guid IdUser);
    }
    public class UserService : IUserService
    {
        public IRepository<Core.Models.User> _userRepository;
        public UserService(IRepository<Core.Models.User> _userRepository)
        {
            this._userRepository = _userRepository;
        }
        public async Task NewUser(UserDTO newUser)
        {
            _userRepository.Add(new()
            {
                Name = newUser.Name,
                PassWord = HashPassword(newUser.PassWord)
            });
            await _userRepository.Complete();
        }
        public async Task<IEnumerable<UserView>> GetAllUserView()
        {
            return from user in await _userRepository.GetAll()
                   select new UserView()
                   {
                       UserId = user.Id,
                       Name = user.Name,
                       CreateDate = user.CreateDate
                   };
        }
        public async Task<UserView> GetUser(Guid IdUser)
        {
            var user = await _userRepository.Get(IdUser);
            return new UserView()
            {
                UserId = user.Id,
                Name = user.Name,
                CreateDate = user.CreateDate
            };
        }
        public async Task UpdateUser(UserDTO dTO, Guid UserId)
        {
            var user = await _userRepository.Get(UserId);
            user.Name = dTO.Name;
            user.PassWord = HashPassword(dTO.PassWord);
            _userRepository.Update(user);
            await _userRepository.Complete();
        }

        public async Task DeleteUser(Guid Userid)
        {
            var user = await _userRepository.Get(Userid);
            if (user is null)
                throw new NullReferenceException("Usuario não existe");

            _userRepository.Delete(user);
            await _userRepository.Complete();
        }
    }
}
