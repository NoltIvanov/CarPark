using beyond.park.client.Models.Validations.Contracts;

namespace beyond.park.client.Models.Validations.ValidationRules {
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T> {

		public string ValidationMessage { get; set; }

		public bool Check(T value) {

			if ((value is string)) {
				var validatedValue = value as string;

				return (!string.IsNullOrWhiteSpace(validatedValue) && !string.IsNullOrEmpty(validatedValue));
			} else {
				return value != null;
			}
		}
	}
}
