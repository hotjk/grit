using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.Utility.Security;

namespace Grit.Utility.Authentication
{
    public class CookieTicketConfig : ICookieTicketConfig
    {
        public string CookieName { get; private set; }
        public bool SlidingExpiration { get; private set; }
        public string LoginUrl { get; private set; }
        public byte[] EncryptionKey { get; private set; }
        public byte[] EncryptionIV { get; private set; }
        public byte[] ValidationKey { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public bool RequireSSL { get; private set; }
        public string CookiePath { get; private set; }

        public static CookieTicketConfig Default()
        {
            CookieTicketConfig config = new CookieTicketConfig();
            config.CookieName = GetSetting("Auth.CookieName", ".auth");
            config.SlidingExpiration = GetBoolSetting("Auth.SlidingExpiration", true);
            config.LoginUrl = GetSetting("Auth.LoginUrl", "/");
            config.EncryptionKey = GetRequiredSetting("Auth.EncryptionKey").GetByteArrayFromHexString();
            config.EncryptionIV = GetRequiredSetting("Auth.EncryptionIV").GetByteArrayFromHexString();
            config.ValidationKey = GetRequiredSetting("Auth.ValidationKey").GetByteArrayFromHexString();
            config.Timeout = new TimeSpan(0, GetIntSetting("Auth.Timeout", 120), 0);
            config.RequireSSL = GetBoolSetting("Auth.RequireSSL", false);
            config.CookiePath = GetSetting("Auth.CookiePath", "/");
            return config;
        }

        private static string GetRequiredSetting(string name)
        {
            var setting = ConfigurationManager.AppSettings[name];
            if (setting != null)
            {
                return setting;
            }

            throw new Exception(string.Format("Required setting '{0}' not found.", name));
        }

        private static string GetSetting(string name, string defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[name];
            if (setting == null)
            {
                setting = defaultValue;
            }
            return setting;
        }

        private static bool GetBoolSetting(string name, bool defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[name];
            bool result;
            if(!bool.TryParse(setting, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        private static int GetIntSetting(string name, int defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[name];
            int result;
            if (!int.TryParse(setting, out result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}
