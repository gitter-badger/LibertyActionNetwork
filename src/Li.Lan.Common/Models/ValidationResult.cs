using System.Collections.Generic;

namespace Li.Lan.Common.Models
{
    public class ValidationResult<T>
    {
        public ValidationResult()
        {
            this.ValidationMessages = new List<ValidationMessage>();
        }

        public T Original { get; set; }

        public T Result { get; set; }

        public bool IsValid { get; set; }

        public bool IsCorrected { get; set; }

        public List<ValidationMessage> ValidationMessages { get; set; }
    }
}