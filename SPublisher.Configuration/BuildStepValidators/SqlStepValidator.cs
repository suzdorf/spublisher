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

            ValidateDatabaseCreate(sqlStep.DatabaseCreate, errors);

            return errors.ToArray();
        }

        private static void ValidateDatabaseCreate(IDatabaseCreate[] databaseCreate, List<IValidationError> errors)
        {
            if (databaseCreate != null)
            {
                if (databaseCreate.DistinctBy(x=>x.DbName).Count() != databaseCreate.Length)
                {
                    if (errors.All(x => x.Type != ValidationErrorType.DbNamesShouldBeUnique))
                        errors.Add(new ValidationError(ValidationErrorType.DbNamesShouldBeUnique));
                }

                if (databaseCreate.Any(x => string.IsNullOrEmpty(x.DbName)))
                {
                    errors.Add(new ValidationError(ValidationErrorType.DbNameIsRequired));
                }
            }
        }
    }
}