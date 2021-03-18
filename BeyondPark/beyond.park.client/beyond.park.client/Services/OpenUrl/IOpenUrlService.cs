using System.Threading.Tasks;

namespace beyond.park.client.Services.OpenUrl {
    public interface IOpenUrlService {
        Task<bool> OpenUrlAsync(string url);
    }
}
