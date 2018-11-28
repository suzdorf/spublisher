using Microsoft.Web.Administration;

namespace SPublisher.IisManagement
{
    public interface IServerManagerStorage
    {
        ServerManager Get();
    }
}