using FluentValidation;
using FluentValidation.Results;

namespace vector_unitech_core.ValueObjects
{
    public abstract class ValueObject<T> : AbstractValidator<T>
    {
        public ValidationResult ValidationResult { get; protected set; }

        protected ValueObject()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract bool EhValido();
    }
}
