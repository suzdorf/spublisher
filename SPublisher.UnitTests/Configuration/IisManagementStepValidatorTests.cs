using System;
using FluentAssertions;
using Moq;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.IisManagement;
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
        [InlineData(Name, true)]
        [InlineData("", false)]
        public void ShouldValidateName(string name, bool isValid)
        {
            var siteMock = new Mock<ISite>();
            siteMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(name);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(new[]
            {
                siteMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.ApplicationNameValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationNameValueIsRequired);
            }
        }

        [Theory]
        [InlineData(Path, true)]
        [InlineData("", false)]
        public void ShouldValidatePath(string path, bool isValid)
        {
            var siteMock = new Mock<ISite>();
            siteMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(path);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(new[]
            {
                siteMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.ApplicationPathValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationPathValueIsRequired);
            }
        }

        [Fact]
        public void ShouldValidateUniqueApplicationNames()
        {
            var siteMock = new Mock<ISite>();
            siteMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(Name);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(new[]
            {
                siteMock.Object,
                siteMock.Object
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
            var siteMock = new Mock<ISite>();
            var nestedApplicationMock = new Mock<IApplication>();
            siteMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(Path);
            siteMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(Name);
            nestedApplicationMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(path);
            nestedApplicationMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(name);

            siteMock.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock.Object
            });

            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(new[]
            {
                siteMock.Object
            });

            var validator = new IisManagementStepValidator(true);

            var result = validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.ApplicationPathValueIsRequired);
                result.Should().NotContain(x => x.Type == ValidationErrorType.ApplicationNameValueIsRequired);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationPathValueIsRequired);
                result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationNameValueIsRequired);
            }
        }
    }
}