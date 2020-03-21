using API.Data;
using API.Data.Entity;
using API.Data.Entity;
using System.Threading.Tasks;

namespace API.Contracts
{
    public interface IApiAuthenticationService
    {
        Task<GeneralUser> Authenticate(ApiUser user);        
    }
}