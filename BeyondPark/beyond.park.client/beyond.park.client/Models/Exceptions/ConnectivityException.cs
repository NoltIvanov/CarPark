using System;

namespace beyond.park.client.Models.Exceptions {
    internal class ConnectivityException : Exception {
        public ConnectivityException(string error) : base(error) {
        }
    }
}
