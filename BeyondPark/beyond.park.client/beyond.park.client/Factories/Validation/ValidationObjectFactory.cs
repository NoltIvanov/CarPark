using beyond.park.client.Models.Validations;

namespace beyond.park.client.Factories.Validation {
    public class ValidationObjectFactory : IValidationObjectFactory {
        public ValidatableObject<T> GetValidatableObject<T>() {
            return new ValidatableObject<T>();
        }
    }
}
