using System;
using FluentAssertions;
using Moq;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Configuration.Exceptions;
using SPublisher.Configuration.Models;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using Xunit;

namespace SPublisher.UnitTests.Configuration
{
    public class IisManagementStepValidatorTests
    {
        private const string Name = "Name";
        private const string Path = "Path";
        private readonly Mock<IBuildStep> _buildStepMock = new Mock<IBuildStep>();
        private readonly Mock<ISite> _siteMock = new Mock<ISite>();
        private readonly IBuildStepValidator _validator;

        public IisManagementStepValidatorTests()
        {
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(new[]
            {
                _siteMock.Object
            });
            _validator = new IisManagementStepValidator(true);
        }

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
            _siteMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(name);

            var result = _validator.Validate(_buildStepMock.Object);

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
            _siteMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(path);

            var result = _validator.Validate(_buildStepMock.Object);

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
            _siteMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(Name);
            _buildStepMock.As<IIisManagementStep>().SetupGet(x => x.Sites).Returns(new[]
            {
                _siteMock.Object,
                _siteMock.Object
            });

            var result = _validator.Validate(_buildStepMock.Object);

            result.Should().Contain(x => x.Type == ValidationErrorType.ApplicationChildrenShouldHaveUniqueNames);
        }

        [Theory]
        [InlineData(Name, Path, true)]
        [InlineData("", "" , false)]
        public void ShouldValidateNestedApplications(string name, string path, bool isValid)
        {
            var nestedApplicationMock = new Mock<IApplication>();
            _siteMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(Path);
            _siteMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(Name);
            nestedApplicationMock.As<IApplicationInfo>().Setup(x => x.Path).Returns(path);
            nestedApplicationMock.As<IApplicationInfo>().Setup(x => x.Name).Returns(name);

            _siteMock.SetupGet(x => x.Applications).Returns(new[]
            {
                nestedApplicationMock.Object
            });

            var result = _validator.Validate(_buildStepMock.Object);

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

        [Theory]
        [InlineData(0, false)]
        [InlineData(65536, false)]
        [InlineData(1, true)]
        [InlineData(65535, true)]
        public void ShoulValidateBindingPorts(int port, bool isValid)
        {
            _siteMock.SetupGet(x => x.Bindings).Returns(new IBinding[]
            {
                new BindingModel
                {
                    Port = port
                }, 
            });

            var result = _validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.SitePortInvalidValue);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.SitePortInvalidValue);
            }
        }

        [Theory]
        [InlineData("www.valid.com", true)]
        [InlineData("valid.com", true)]
        [InlineData("valid", true)]
        [InlineData("", true)]
        [InlineData("[invalid]", false)]
        [InlineData("invalid/dw", false)]
        public void ShoulValidateHostName(string hostName, bool isValid)
        {
            _siteMock.SetupGet(x => x.Bindings).Returns(new IBinding[]
            {
                new BindingModel
                {
                    HostName = hostName
                },
            });

            var result = _validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.SiteHostNameHasInvalidValue);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.SiteHostNameHasInvalidValue);
            }
        }

        [Theory]
        [InlineData("random string", false)]
        [InlineData("", false)]
        [InlineData("257.256.256.256", false)]
        [InlineData("-1.1.1.1", false)]
        [InlineData("*", true)]
        [InlineData("1.1.1.1", true)]
        public void ShoulValidateIpAddress(string ipdAddress, bool isValid)
        {
            _siteMock.SetupGet(x => x.Bindings).Returns(new IBinding[]
            {
                new BindingModel
                {
                    IpAddress = ipdAddress
                }
            });

            var result = _validator.Validate(_buildStepMock.Object);

            if (isValid)
            {
                result.Should().NotContain(x => x.Type == ValidationErrorType.SiteIpAddressInvalidValue);
            }
            else
            {
                result.Should().Contain(x => x.Type == ValidationErrorType.SiteIpAddressInvalidValue);
            }
        }
    }
}