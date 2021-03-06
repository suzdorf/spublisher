﻿using SPublisher.Core;
using SPublisher.Core.DbManagement;

namespace SPublisher.DBManagement
{
    public interface ISqlServerDataProvider
    {
        bool DataBaseExists(string databaseName);

        void CreateDataBase(IDatabase database);

        void RestoreDatabase(IDatabase database);

        void ExecuteScript(string script, string databaseName);

        void CreateHashInfoTableIfNotExists(string databaseName);

        IScriptHashInfo[] GetHashInfoList(string databaseName);

        void SaveHashInfo(string databaseName, IFile hashInfo);
    }
}