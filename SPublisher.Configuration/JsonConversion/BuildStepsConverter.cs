using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.Exceptions;

namespace SPublisher.Configuration.JsonConversion
{
    public class BuildStepsConverter : CustomConverter<BuildStepModel>
    {
        private readonly IDictionary<string, Func<BuildStepModel>> _buildStepModelCreators;

        public BuildStepsConverter(IDictionary<string, Func<BuildStepModel>> buildStepModelCreators)
        {
            _buildStepModelCreators = buildStepModelCreators;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override BuildStepModel Create(Type objectType, JObject jObject)
        {
            var typeField = jObject["Type"];

            if (typeField == null)
            {
                throw new BuildStepTypeIsMissingException();
            }

            var type = typeField.Value<string>();

            try
            {
                return _buildStepModelCreators[type]();
            }
            catch (KeyNotFoundException)
            {
                throw new BuildStepTypeNotFoundException(type);
            }
        }
    }
}
