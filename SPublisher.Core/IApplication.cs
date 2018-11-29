namespace SPublisher.Core
{
    public interface IApplication : IAppPoolInfo, IApplicationInfo
    {
        IApplication[] Applications { get; }
    }
}
