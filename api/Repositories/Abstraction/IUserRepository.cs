using api.Models;

namespace api.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<InsertUpdateUserRequest> InsertUpdate(InsertUpdateUserRequest member);

    }
}

