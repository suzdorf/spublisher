namespace SPublisher.Core.IisManagement
{
    public interface ISite : IApplication
    {
        IBinding[] Bindings { get; }
    }
}