using beyond.park.client.Builders.Contracts;
using beyond.park.client.Helpers;
using beyond.park.client.Models.Registration;
using System.Collections.Generic;

namespace beyond.park.client.Builders.DataItems.TourItems {
    public sealed class TourItemBuilder : ITourItemBuilder {
        List<TourItem> IItemBuilder<List<TourItem>>.BuildItems() {
            return new List<TourItem> {
                new TourItem {
                    IconPath = IconPath.IMAGE_ONE,
                    Title="Beyond your\r\nexpectations",
                    Description = "The exciting new way to go in and out of a carpark without taking a ticket or reaching out of your car."
                },
                new TourItem {
                    IconPath = IconPath.IMAGE_TWO,
                    Title="Beyond convenient\r\ncarparking",
                    Description = "Beyond Parking takes the stress out of finding a convenient and affordable carpark, leaving you more time to enjoy life."
                },
                 new TourItem {
                    IconPath = IconPath.IMAGE_THREE,
                    Title="Beyond contactless\r\nParking",
                    Description = "Beyond park is contactless, no need to worry about pay station queues, lost tickets or paper receipts. Just drive in, park and drive out."
                },
                new TourItem {
                    IconPath = IconPath.IMAGE_FOUR,
                    Title="Beyond a\r\nguiding hand",
                    Description = "Beyond lets you know where parking is available and the price you’ll pay, it will also guide you there."
                }
            };
        }
    }
}
