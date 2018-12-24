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
                errors.Add(new ValidationError(ValidationErrorType.ConnectionStringIsRequired));
            }

            ValidateDatabaseCreate(sqlStep.Databases, errors);

            return errors.ToArray();
        }

        private static void ValidateDatabaseCreate(IDatabase[] databaseCreate, List<IValidationError> errors)
        {
            if (databaseCreate != null)
            {
                if (databaseCreate.DistinctBy(x=>x.DatabaseName).Count() != databaseCreate.Length)
                {
                    if (errors.All(x => x.Type != ValidationErrorType.DbNamesShouldBeUnique))
                        errors.Add(new ValidationError(ValidationErrorType.DbNamesShouldBeUnique));
                }

                if (databaseCreate.Any(x => string.IsNullOrEmpty(x.DatabaseName)))
                {
                    errors.Add(new ValidationError(ValidationErrorType.DbNameIsRequired));
                }
            }
        }
    }
}