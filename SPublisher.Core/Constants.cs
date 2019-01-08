using System.Collections.Generic;
using SPublisher.Core.Enums;

namespace SPublisher.Core
{
    public static class Constants
    {
        public static class Steps
        {
            public const string CommandLineBuildStep = "cmd";
            public const string IisManagementBuildStep = "iis";
            public const string SqlBuildStep = "sql";
        }

        public static class SqlServerType
        {
            public const string MsSql = "mssql";
            public const string MySql = "mysql";
            public const string PostgreSql = "postgre";

            public static readonly Dictionary<Enums.SqlServerType, string> BuildDictionary =
                new Dictionary<Enums.SqlServerType, string>
                {
                    {Enums.SqlServerType.PostgreSql, PostgreSql},
                    {Enums.SqlServerType.MySql, MySql},
                    {Enums.SqlServerType.MsSql, MsSql}
                };
        }

        public static class SiteBinding
        {
            public static class Types
            {
                public const string Http = "http";
                public const string Https = "https";

                public static readonly Dictionary<BindingType, string> BuildDictionary = new Dictionary<BindingType, string>
                {
                    {BindingType.Http, Http },
                    {BindingType.Https, Https }
                };
            }

            public const int DefaultHttpPort = 80;
            public const int DefaultHttpsPort = 443;
            public const string DefaultIpAddress = "*";
            public const string CertificateStoreName = "My";
        }
    }
}