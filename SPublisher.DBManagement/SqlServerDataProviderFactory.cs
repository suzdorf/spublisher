using System;
using System.Collections.Generic;
using SPublisher.Core;
using SPublisher.DBManagement.DataProviders;

namespace SPublisher.DBManagement
{
    public class SqlServerDataProviderFactory : ISqlServerDataProviderFactory
    {
        private readonly IConnectionAccessor _connectionAccessor;

        public SqlServerDataProviderFactory(IConnectionAccessor connectionAccessor)
        {
            _connectionAccessor = connectionAccessor;
        }

        public ISqlServerDataProvider Get()
        {
            return _buildStepModelCreators[_connectionAccessor.ServerType](_connectionAccessor);
        }

        private static readonly IDictionary<SqlServerType, Func<IConnectionAccessor, ISqlServerDataProvider>> _buildStepModelCreators =
            new Dictionary<SqlServerType, Func<IConnectionAccessor, ISqlServerDataProvider>>
            {
                {SqlServerType.MsSql, accessor => new SqlServerDataProvider(accessor) },
                {SqlServerType.MySql, accessor => new MySqlDataProvider(accessor) }
            };

    }
}