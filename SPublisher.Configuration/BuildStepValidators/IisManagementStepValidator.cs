using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.BuildStepValidators
{
    public class IisManagementStepValidator : IBuildStepValidator
    {
        private readonly bool _isAdministratorMode;

        public IisManagementStepValidator(bool isAdministratorMode)
        {
            _isAdministratorMode = isAdministratorMode;
        }
        public IValidationError[] Validate(IBuildStep step)
        {
            var errors = new List<IValidationError>();
            var iisManagementStep = (IIisManagementStep)step;

            if (!_isAdministratorMode)
            {
                throw new ShouldRunAsAdministratorException();
            }

            ValidateApplications(iisManagementStep.Sites, errors);

            ValidateBindings(iisManagementStep.Sites.SelectMany(x => x.Bindings).ToArray(), errors);

            return errors.ToArray();
        }

        private void ValidateApplications(IApplication[] applications, List<IValidationError> errors)
        {
            ValidateName(applications, errors);

            foreach (var application in applications)
            {
                ValidatePath(application.Path, errors);

                if (application.Applications != null && application.Applications.Any())
                {
                    ValidateApplications(application.Applications, errors);
                }
            }
        }

        private void ValidatePath(string path, List<IValidationError> errors)
        {
            if (string.IsNullOrEmpty(path))
            {
                if (errors.All(x => x.Type != ValidationErrorType.ApplicationPathValueIsRequired))
                    errors.Add(new ValidationError(ValidationErrorType.ApplicationPathValueIsRequired));
            }
        }

        private static void ValidateName(IApplication[] applications, List<IValidationError> errors)
        {
            if (applications.Any(x=>string.IsNullOrEmpty(x.Name)))
            {
                if (errors.All(x => x.Type != ValidationErrorType.ApplicationNameValueIsRequired))
                    errors.Add(new ValidationError(ValidationErrorType.ApplicationNameValueIsRequired));
            }

            var applicationNames = applications.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToArray();

            if (applicationNames.Distinct().Count() != applicationNames.Length)
            {
                if (errors.All(x => x.Type != ValidationErrorType.ApplicationChildrenShouldHaveUniqueNames))
                    errors.Add(new ValidationError(ValidationErrorType.ApplicationChildrenShouldHaveUniqueNames));
            }
        }

        private static void ValidateBindings(IBinding[] bindings, List<IValidationError> errors)
        {
            if (bindings.Any(x => !ValidatePort(x.Port)))
            {
                errors.Add(new ValidationError(ValidationErrorType.SitePortInvalidValue));
            }

            if (bindings.Any(x => !ValidateIPv4(x.IpAddress)))
            {
                errors.Add(new ValidationError(ValidationErrorType.SiteIpAddressInvalidValue));
            }

            if (bindings.Any(x => !ValidateHostName(x.HostName)))
            {
                errors.Add(new ValidationError(ValidationErrorType.SiteHostNameHasInvalidValue));
            }
        }

        private static bool ValidatePort(int value)
        {
            return value > 0 && value < 65536;
        }

        private static bool ValidateIPv4(string value)
        {
            if (value == Constants.SiteBinding.DefaultIpAddress) return true;
            return value.Count(c => c == '.') == 3 && IPAddress.TryParse(value, out var address);
        }

        private static bool ValidateHostName(string value)
        {
            if (value == string.Empty) return true;
            return Uri.CheckHostName(value) != UriHostNameType.Unknown;
        }
    }
}