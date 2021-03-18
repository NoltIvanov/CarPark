using beyond.park.client.Models.HelpSystem;
using System.Collections.Generic;

namespace beyond.park.client.Builders.DataItems.HelpItems {
    public sealed class HelpItemBuilder : IHelpItemBuilder {
        public LinkedList<HelpItem> BuildItems() {
            LinkedList<HelpItem> helpItems = new LinkedList<HelpItem>();
            helpItems.AddLast(new HelpItem { Title = "Hi I’m Barry. looks like this is your first time here. Click on me to see the next tip, or click on the ‘x’ in the top right of this box if you don’t want tips right now now.", HelpPopup = HelpPopups.First });
            helpItems.AddLast(new HelpItem { Title = "The first thing you’ll notice are the carparks closest to your location.", HelpPopup = HelpPopups.ClosestLocation });
            helpItems.AddLast(new HelpItem { Title = "And here are filters to sort out what’s most important for you.", HelpPopup = HelpPopups.Filter });
            helpItems.AddLast(new HelpItem { Title = "And when you click from one of the list below, you’ll be rerouted to that carspace. Try doing it yourself!", HelpPopup = HelpPopups.Reroute });
            helpItems.AddLast(new HelpItem { Title = "Or you can click on the waypoints directly on the map if you prefer.", HelpPopup = HelpPopups.MapPin });
            helpItems.AddLast(new HelpItem { Title = "No worries! I’ll be here if you need me, and occasionally I’ll pop up for more tips", HelpPopup = HelpPopups.MoreTips });

            return helpItems;
        }
    }
}
