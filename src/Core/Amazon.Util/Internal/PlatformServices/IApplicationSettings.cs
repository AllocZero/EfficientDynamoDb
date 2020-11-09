using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Util.Internal.PlatformServices
{
    public enum ApplicationSettingsMode
    {
        None,
        Local,
        Roaming
    }

    public interface IApplicationSettings
    {
        void SetValue(string key, string value, ApplicationSettingsMode mode);

        string GetValue(string key, ApplicationSettingsMode mode);

        void RemoveValue(string key, ApplicationSettingsMode mode);
    }
}
