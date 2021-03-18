using System;

namespace beyond.park.client.Models.Arguments.Registration {
    public sealed class WelcomeBackArgs {
        public string Name { get; set; }

        public Action Callback { get; set; }
    }
}
