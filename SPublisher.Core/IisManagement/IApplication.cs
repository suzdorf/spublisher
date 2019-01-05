namespace SPublisher.Core.IisManagement
{
    public interface IApplication : IAppPoolInfo, IApplicationInfo
    {
        IApplication[] Applications { get; }
        bool IsVirtualDirectory { get; }
    }
}
