using System.Collections.Generic;
using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildStepValidators
{
    public class SqlStepValidator : IBuildStepValidator
    {
        public IValidationError[] Validate(IBuildStep step)
        {
            var errors = new List<IValidationError>();
            var sqlStep = (ISqlStep)step;

            if (string.IsNullOrEmpty(sqlStep.ConnectionString))
            {
                errors.Add(new ValidationError(ValidationErrorType.SqlStepConnectionStringIsRequired));
            }

            var scripts = sqlStep.Databases.SelectMany(x => x.Scripts).ToList();

            if (scripts.Any(x => string.IsNullOrEmpty(x.Path)))
            {
                errors.Add(new ValidationError(ValidationErrorType.SqlStepPathValueIsRequired));
            }

            return errors.ToArray();
        }
    }
}