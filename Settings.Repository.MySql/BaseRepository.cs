﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Repository.MySql
{
    public class BaseRepository
    {
        protected static IDbConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(Grit.Configuration.MySql.Settings);
            connection.Open();
            return connection;
        }
    }
}