using System;

namespace beyond.park.client.Models.Arguments.BottomTabs {
    internal sealed class SomeBottomTabWasSelectedArgs {
        public SomeBottomTabWasSelectedArgs(Type selectedTabType) {
            if (selectedTabType.IsClass) { }

            SelectedTabType = selectedTabType;
        }

        public Type SelectedTabType { get; private set; }
    }
}
