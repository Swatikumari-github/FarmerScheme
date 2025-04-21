using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        // Method for registering a user
        bool RegisterUser(RegisterDTO registerDTO);

        // Method for logging in a user and returning a string status
        string LoginUser(LoginDTO loginDTO);
    }
}
