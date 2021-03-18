using beyond.park.client.ViewModels.Items;
using System.Collections.Generic;

namespace beyond.park.client.Builders.DataItems.FilterItems {
    public class FilterItemBuilder : IFilterItemBuilder {
        public List<FilterItemViewModel> BuildItems() {
            List<FilterItemViewModel> filterItemViewModels = new List<FilterItemViewModel> {
                new FilterItemViewModel { Title = "Beyond Park" },
                new FilterItemViewModel { Title = "$0 - $10" },
                new FilterItemViewModel { Title = "Height"},
                new FilterItemViewModel { Title = "Toilets" },
            };
            return filterItemViewModels;
        }
    }
}
