using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        bool RegisterUser(UsersEntity user);
        UsersEntity LoginUser(string email);
    }
}
