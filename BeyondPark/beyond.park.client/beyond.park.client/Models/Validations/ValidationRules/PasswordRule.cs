using beyond.park.client.Models.Validations.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace beyond.park.client.Models.Validations.ValidationRules {
    public sealed class PasswordRule<T> : IValidationRule<T> {
        public string ValidationMessage { get; set; }

        public bool Check(T value) {
            if (value == null)
                return false;

            string validatedValue = (value as string)?.Trim();

            Regex regex = new Regex(@"^(?=(.*\d){3})(?=(.*[a-zA-Z]))[a-zA-Z0-9]{5,}$");
            Match match = regex.Match(validatedValue);

            return match.Success;
        }
    }
}
