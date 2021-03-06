﻿using System.Collections.Generic;
using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;

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

            if (sqlStep.ServerType == SqlServerType.Invalid)
            {
                errors.Add(new ValidationError(ValidationErrorType.SqlServerTypeInvalidValue));
            }

            var scripts = sqlStep.Databases.SelectMany(x => x.Scripts).ToList();

            if (scripts.Any(x => string.IsNullOrEmpty(x.Path)))
            {
                errors.Add(new ValidationError(ValidationErrorType.SqlStepPathValueIsRequired));
            }

            if (sqlStep.Databases.Any(x =>
                x.RestoreAvailable &&
                !string.IsNullOrEmpty(x.BackupPath) &&
                string.IsNullOrEmpty(x.DatabaseName)))
            {
                errors.Add(new ValidationError(ValidationErrorType.DatabaseNameMustBeSpecifiedForRestoreOperation));
            }

            return errors.ToArray();
        }
    }
}