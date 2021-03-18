using System;
using System.Threading.Tasks;

namespace beyond.park.client.ViewModels.Base.Contracts {
    public interface IVisualFiguring {
        string TabHeader { get; }

        Type RelativeViewType { get; }

        void Dispose();

        Task InitializeAsync(object navigationData);
    }
}
