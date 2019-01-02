﻿using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class DbConnection : IConnectionAccessor, IConnectionSetter
    {
        public string ConnectionString { get; private set; }
        public void Set(ISqlConnectionSettings connectionSettings)
        {
            ConnectionString = connectionSettings.ConnectionString;
            ServerType = connectionSettings.ServerType;
        }

        public SqlServerType ServerType { get; private set; }
    }
}