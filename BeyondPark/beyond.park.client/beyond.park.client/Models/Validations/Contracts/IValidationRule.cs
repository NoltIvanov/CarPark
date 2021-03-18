namespace beyond.park.client.Models.Validations.Contracts {
    public interface IValidationRule<T> {
		string ValidationMessage { get; set; }

		bool Check(T value);
	}
}
