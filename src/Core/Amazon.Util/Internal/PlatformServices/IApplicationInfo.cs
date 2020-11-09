
namespace Amazon.Util.Internal.PlatformServices
{
    public interface IApplicationInfo
    {
        string AppTitle { get; }
        string AppVersionName { get; }
        string AppVersionCode { get; }
        string PackageName { get; }
    }
}
