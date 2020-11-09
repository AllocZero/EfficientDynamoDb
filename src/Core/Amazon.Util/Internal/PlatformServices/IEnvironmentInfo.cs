
namespace Amazon.Util.Internal.PlatformServices
{
    public interface IEnvironmentInfo
    {
        string Platform { get; }
        string PlatformUserAgent { get; }
        string FrameworkUserAgent { get; }
    }
}
