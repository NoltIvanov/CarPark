using beyond.park.client.Models.Rest.Carpark;
using System.Collections.Generic;

namespace beyond.park.client.Builders.DataItems.CarparkItems {
    public sealed class CarparkItemBuilder : ICarparkItemBuilder {
        public List<CarparkBody> BuildItems() {
            return new List<CarparkBody> {
                    new CarparkBody{
                        Name = "Beven Street Carpark",
                        Address = "28 Bevan St,\r\nSouth Melbourne",
                        Distance = 5,
                        Fee = 15
                    },
                     new CarparkBody{
                        Name = "St Vincent St Carpark",
                        Address = "131 Elizabeth St,\r\nMelbourne",
                        Distance = 8,
                        Fee = 9
                    }
                };
        }
    }
}
