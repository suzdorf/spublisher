using System.IO;
using SPublisher.Configuration;
using SPublisher.IisManagement;

namespace SPublisher
{
    class Program
    {
        private static readonly ServerManagerAccessor ServerManagerAccessor = new ServerManagerAccessor();
        private static readonly IServerManagerDataProvider ServerManagerDataProvider =  new ServerManagerDataProvider(ServerManagerAccessor);
        private static readonly IApplicationCreator ApplicationCreator = new ApplicationCreator(ServerManagerDataProvider);
        private static readonly ISiteCreator SiteCreator = new DefaultSiteCreator(ServerManagerAccessor, ApplicationCreator);
        static void Main(string[] args)
        {
            var json = File.ReadAllText("spublisher.json");
            var cf = new ConfigurationFactory();
            var model = cf.Get(json);

            SiteCreator.Create(model.Applications);
        }
    }
}
