using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPublisher.Configuration.BuildSteps;

namespace SPublisher.Configuration.JsonConversion
{
    public class BuildStepsConverter : CustomConverter<BuildStepModel>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override BuildStepModel Create(Type objectType, JObject jObject)
        {
            var type = jObject["Type"].Value<string>();

            switch (type)
            {
                case "cmd":
                    return new CommandLineStepModel();
                case "bat":
                    return new BatchFileStepModel();
                default:
                    return new BuildStepModel();
            }
        }
    }
}
