using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static void Validate(IValidator validator, object entity)
        {
            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    throw new ValidationException(error.ErrorMessage);
                }
            }
        }
    }
}
