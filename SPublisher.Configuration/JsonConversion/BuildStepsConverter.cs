using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPublisher.Configuration.BuildSteps;

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
            var type = jObject["Type"].Value<string>();

            return _buildStepModelCreators[type]();
        }
    }
}
