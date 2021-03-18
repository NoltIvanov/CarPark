using System.Windows.Input;

namespace beyond.park.client.ViewModels.Base.Contracts {
    public interface IActionBottomBarTab {
        ICommand TabActionCommand { get; }
    }
}
