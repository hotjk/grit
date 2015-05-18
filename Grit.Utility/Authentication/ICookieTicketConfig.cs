using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Authentication
{
    public interface ICookieTicketConfig
    {
        string CookieName { get; }
        bool SlidingExpiration { get; }
        string LoginUrl { get; }
        byte[] EncryptionKey { get; }
        byte[] EncryptionIV { get; }
        byte[] ValidationKey { get; }
        TimeSpan Timeout { get; }
        bool RequireSSL { get; }
        string CookiePath { get; }
    }
}
