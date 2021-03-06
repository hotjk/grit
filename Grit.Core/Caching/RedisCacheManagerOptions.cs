﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Core.Caching
{
    public class RedisCacheManagerOptions
    {
        public RedisCacheManagerOptions(string configuration, int db)
        {
            Configuration = configuration;
            DBIndex = db;
        }
        public string Configuration { get; private set; }
        public int DBIndex { get; private set; }
    }
}
