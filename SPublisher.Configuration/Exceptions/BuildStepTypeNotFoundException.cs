using SPublisher.Core.Exceptions;

namespace SPublisher.Configuration.Exceptions
{
    public class BuildStepTypeNotFoundException : SPublisherException
    {
        public BuildStepTypeNotFoundException(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}