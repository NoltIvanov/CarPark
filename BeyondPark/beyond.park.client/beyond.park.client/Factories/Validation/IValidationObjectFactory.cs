using beyond.park.client.Models.Validations;

namespace beyond.park.client.Factories.Validation {
    public interface IValidationObjectFactory {
        ValidatableObject<T> GetValidatableObject<T>();
    }
}
