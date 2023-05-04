using SqlSugar;
namespace RoslynCat.SQL
{
    public static class SqlSugarConfiguration
    {
        public static ISqlSugarClient Configure() {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = new GetConfig().ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });

            // 配置SqlSugar的日志记录器
            db.Aop.OnLogExecuting = (sql,pars) =>
            {
                // 获取当前日期
                string currentDate = DateTime.Now.ToString("yyyyMMdd");

                // 构建日志文件名
                string logFileName = $"{currentDate}-sqlLog.txt";

                // 获取日志文件路径
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);

                // 如果日志文件不存在，则创建日志文件
                if (!File.Exists(logFilePath)) {
                    File.Create(logFilePath).Close();
                }

                // 记录日志到文件中
                string logContent = $"执行SQL：{sql}，参数：{pars.ToString()}";
                File.AppendAllText(logFilePath,logContent);
            };
            return db;
        }
    }
}
