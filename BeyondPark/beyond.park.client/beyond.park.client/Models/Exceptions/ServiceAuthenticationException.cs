using System;

namespace beyond.park.client.Models.Exceptions {
    public class ServiceAuthenticationException : Exception {

        public string Content { get; }

        public ServiceAuthenticationException() {
        }

        public ServiceAuthenticationException(string content) {
            Content = content;
        }
    }
}
