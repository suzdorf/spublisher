using System;
using FluentAssertions;
using Moq;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class IisManagementStepValidatorTests
    {
        private const string Name = "Name";
        private const string AppPoolName = "AppPoolName";
        private const string Path = "Path";
        private readonly Mock<IBuildStep> _buildStepMock = new Mock<IBuildStep>();
        
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShouldThrowIfNotInAdministratorMode(bool isAdministratorMode, bool isValid)
        {
            var validator = new IisManagementStepValidator(isAdministratorMode);
            _buildStepMock.As<IIisManagementStep>();

            Action action = () => { validator.Validate(_buildStepMock.Object); };

            if (isValid)
            {
                action.Should().NotThrow<ShouldRunAsAdministratorException>();
            }
            else
            {
                action.Should().Throw<ShouldRunAsAdministratorException>();
            }
        }

        [Theory]
        [InlineData(AppPoolName, true)]
        [InlineData("", false)]
        public void ShouldValidateAppPoolValueForSite(string appPoolName, bool isValid)
        {
            var applicationMock = new Mock<IApplication>();
            applicationMock.As<IApplicationInfo>().Setup(x => x.AppPoolName).Returns(appPoolName);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(new []
            {
                applicationMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.ApplicationPoolForTheSiteIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationPoolForTheSiteIsRequired);
            }
        }

        [Theory]
        [InlineData(Name, true)]
        [InlineData("", false)]
        public void ShouldValidateName(string name, bool isValid)
        {
            var applicationMock = new Mock<IApplication>();
            applicationMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(name);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(new[]
            {
                applicationMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.NameValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.NameValueIsRequired);
            }
        }

        [Theory]
        [InlineData(Path, true)]
        [InlineData("", false)]
        public void ShouldValidatePath(string path, bool isValid)
        {
            var applicationMock = new Mock<IApplication>();
            applicationMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(path);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(new[]
            {
                applicationMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.PathValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.PathValueIsRequired);
            }
        }

        [Fact]
        public void ShouldValidateUniqueApplicationNames()
        {
            var applicationMock = new Mock<IApplication>();
            applicationMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(Name);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(new[]
            {
                applicationMock.Object,
                applicationMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationChildrenShouldHaveUniqueNames);
        }

        [Theory]
        [InlineData(Name, Path, true)]
        [InlineData("", "" , false)]
        public void ShouldValidateNestedApplications(string name, string path, bool isValid)
        {
            var applicationMock = new Mock<IApplication>();
            var nestedApplicationMock = new Mock<IApplication>();
            applicationMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(Path);
            applicationMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(Name);
            nestedApplicationMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(path);
            nestedApplicationMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(name);

            applicationMock.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock.Object
            });

            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Applications).Returns(new[]
            {
                applicationMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.PathValueIsRequired);
                result.Should().NotContain(x => x.Type == ValidationErrorType.NameValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.PathValueIsRequired);
                result.Should().Contain(x => x.Type == ValidationErrorType.NameValueIsRequired);
            }
        }
    }
}