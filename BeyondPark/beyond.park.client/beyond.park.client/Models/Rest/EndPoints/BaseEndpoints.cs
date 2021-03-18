namespace beyond.park.client.Models.Rest.EndPoints {
    public abstract class BaseEndpoints {

        string _baseEndpoint;
        public string BaseEndpoint {
            get { return _baseEndpoint; }
            set {
                _baseEndpoint = value;
                UpdateEndpoint(_baseEndpoint);
            }
        }

        public BaseEndpoints(string defaultEndPoint) {
            BaseEndpoint = defaultEndPoint;
        }

        public abstract void UpdateEndpoint(string baseEndpoint);
    }
}
