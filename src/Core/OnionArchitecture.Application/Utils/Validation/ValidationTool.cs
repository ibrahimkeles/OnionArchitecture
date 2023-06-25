using FluentValidation;
using FluentValidation.Results;

namespace OnionArchitecture.Application.Utils.Validation
{
    public static class ValidationTool
    {
        public static ValidationResult Validate(Type type, object item)
        {
            if (!typeof(IValidator).IsAssignableFrom(type))
                throw new Exception("Hata: Validator tipi geçersiz!");
            var validator = (IValidator)Activator.CreateInstance(type);
            return validator.Validate(new ValidationContext<object>(item));
        }

        public static void Validate(Type type, object[] entities)
        {
            if (!typeof(IValidator).IsAssignableFrom(type))
                throw new Exception("Hata: Validator tipi geçersiz!");

            var validator = (IValidator)Activator.CreateInstance(type);

            foreach (var item in entities)
            {
                if (validator.CanValidateInstancesOfType(item.GetType()))
                {
                    var result = validator.Validate(new ValidationContext<object>(item));

                    if (!result.IsValid)
                        throw new ValidationException(result.Errors);
                }
            }

        }
    }
}
