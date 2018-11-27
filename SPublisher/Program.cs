using System.IO;
using SPublisher.Configuration;

namespace SPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("spublisher.json");
            var cf = new ConfigurationFactory();
            var model = cf.Get(json);
        }
    }
}
