﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Basic
{
    public static class AssemblyManager
    {
        private static DateTime _Y2K = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime _Y1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static System.Version GetComplieVersion(Assembly assembly)
        {
            string version = assembly.GetName().Version.ToString();
            return System.Version.Parse(version);
        }

        public static DateTime GetVersionTimestamp(System.Version version)
        {
            return _Y2K.AddDays(version.Build).AddSeconds(version.Revision * 2);
        }

        public static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return _Y1970.AddSeconds(secondsSince1970).ToLocalTime();
        }
    }
}
