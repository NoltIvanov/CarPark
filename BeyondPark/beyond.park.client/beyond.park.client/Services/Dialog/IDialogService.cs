using System;
using System.Threading.Tasks;

namespace beyond.park.client.Services.Dialog {
    public interface IDialogService {
        Task ShowAlertAsync(string message, string title, string buttonLabel);

        Task ToastAsync(string message, string actionName, Action action);

        Task ToastAsync(string message);
    }
}
