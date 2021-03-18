using beyond.park.client.Models.Validations.Contracts;
using beyond.park.client.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace beyond.park.client.Models.Validations {
    public class ValidatableObject<T> : ExtendedBindableObject, IValidity {
		public List<IValidationRule<T>> Validations { get; }

		List<string> _errors;
		public List<string> Errors {
			get { return _errors; }
			set { SetProperty(ref _errors, value); }
		}

		T _value;
		public T Value {
			get { return _value; }
			set { SetProperty(ref _value, value); }
		}

		bool _isValid;
		public bool IsValid {
			get { return _isValid; }
			set { SetProperty(ref _isValid, value); }
		}

		/// <summary>
		///     ctor().
		/// </summary>
		public ValidatableObject() {
			_isValid = false;
			_errors = new List<string>();
			Validations = new List<IValidationRule<T>>();
		}

		public bool Validate() {
			Errors.Clear();

			IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
				.Select(v => v.ValidationMessage);

			Errors = errors.ToList();
			IsValid = !Errors.Any();

			return this.IsValid;
		}
	}
}
