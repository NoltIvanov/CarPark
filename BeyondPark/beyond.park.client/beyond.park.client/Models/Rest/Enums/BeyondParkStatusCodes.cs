namespace beyond.park.client.Models.Rest.Enums {
    public enum BeyondParkStatusCodes {
        InternalServerError = 500,
        UnAuthorised = 4001,
        IncompatibleParameters = 4002,
        MissingRequiredParameters = 4003,
        InsufficientPermissions = 4004,
        AlreadyRegistered = 4005,
        InActiveUser = 4006,
        UnMatchedPassword = 4009,
        UnSuccessfulRegistration = 4010,
        UnRegisteredUser = 4011,
        InCorrectDataRequest = 4012,
        VehicleAlreadyRegistered = 4013,
        InvalidPasswordFormat = 4014,
        Success = 200
    }
}
