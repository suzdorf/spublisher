namespace SPublisher.DBManagement
{
    public class SqlScriptInfo : ISqlScriptInfo
    {
        public SqlScriptInfo(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}