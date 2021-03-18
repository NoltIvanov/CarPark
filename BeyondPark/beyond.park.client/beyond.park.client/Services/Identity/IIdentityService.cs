using beyond.park.client.Models.Rest;
using beyond.park.client.Models.Rest.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace beyond.park.client.Services.Identity {
    public interface IIdentityService {
        Task<BaseResult<bool>> RegisterAsync(RegisterDataContract registerDataContract, CancellationToken cancellationToken = default(CancellationToken));

        Task<BaseResult<object>> EmailConfirmationAsync(ConfirmationDataContract confirmationDataContract, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<BaseResult<SignInBody>> SignInAsync(SignInDataContract signInDataContract, CancellationToken cancellationToken = default(CancellationToken));
    }
}
