namespace SPublisher.Configuration
{
    public class BuildStepTypeNotFoundMessage : IBuildStepTypeNotFoundMessage
    {
        public BuildStepTypeNotFoundMessage(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}