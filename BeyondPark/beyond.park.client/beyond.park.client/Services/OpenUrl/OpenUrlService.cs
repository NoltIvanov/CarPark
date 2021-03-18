using System.Threading.Tasks;
using Xamarin.Essentials;

namespace beyond.park.client.Services.OpenUrl {
    public sealed class OpenUrlService : IOpenUrlService {
        public async Task<bool> OpenUrlAsync(string url) {
            return await Launcher.TryOpenAsync(url);
        }
    }
}
