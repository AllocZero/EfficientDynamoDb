using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Util.Internal.PlatformServices
{
    public class ApplicationSettings : IApplicationSettings
    {
        public void SetValue(string key, string value, ApplicationSettingsMode mode)
        {
            throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
        }

        public string GetValue(string key, ApplicationSettingsMode mode)
        {
            throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
        }

        public void RemoveValue(string key, ApplicationSettingsMode mode)
        {
            throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
        }

    }
}
