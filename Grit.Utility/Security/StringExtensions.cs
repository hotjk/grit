using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Security
{
    public static class StringExtensions
    {
        public static byte[] GetByteArrayFromHexString(this string s)
        {
            return SoapHexBinary.Parse(s).Value;
        }

        public static string GetHexString(this byte[] b)
        {
            return new SoapHexBinary(b).ToString();
        }
    }
}
