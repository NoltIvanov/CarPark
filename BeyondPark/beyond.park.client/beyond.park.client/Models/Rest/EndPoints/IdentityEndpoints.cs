namespace beyond.park.client.Models.Rest.EndPoints {
    public sealed class IdentityEndpoints : BaseEndpoints {

        private const string SIGNIN_API_KEY = "user/authenticate";

        private const string REGISTER_API_KEY = "user/register";

        private const string EMAIL_CONFIRMATION_API_KEY = "user/VerifyEmailConfirmationToken";

        public string SignInEndPoint { get; private set; }

        public string RegisterEndPoint { get; private set; }

        public string EmailConfirmationEndPoint { get; private set; }

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="defaultEndPoint"></param>
        public IdentityEndpoints(string defaultEndPoint) : base(defaultEndPoint) { }

        public override void UpdateEndpoint(string baseEndpoint) {
            SignInEndPoint = $"{baseEndpoint}/{SIGNIN_API_KEY}";
            RegisterEndPoint = $"{baseEndpoint}/{REGISTER_API_KEY}";
            EmailConfirmationEndPoint = $"{baseEndpoint}/{EMAIL_CONFIRMATION_API_KEY}";
        }
    }
}
