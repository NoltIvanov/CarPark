using beyond.park.client.Models.Registration;
using System;

namespace beyond.park.client.Models.Arguments.Registration {
    public class ConfirmationArgs {
        public Action<Confirmation> Callback { get; internal set; }
    }
}
