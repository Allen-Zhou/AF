using System;
using System.IO;
using System.Reflection;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant.NHImpl
{
    public class DynamicSqlBuilder
    {
        private const string CONFIGPATH = "Config";

        protected static ISqlMapper _sqlMapper;

        private const string FILENAME = "SqlMap.config";

        /// <summary>
        /// 只在系统启动时调用一次
        /// </summary>
        public static void InitSqlMapper()
        {
            var builder = new DomSqlMapBuilder();
            var configfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIGPATH, FILENAME);
            _sqlMapper = builder.Configure(configfileName);
            Console.WriteLine("动态查询模板配置完毕");
        }
 
        /// <summary>
        /// 根据传入的模板名称和参数列表，返回最终的sql
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        public static string GetSql( string statementName, object paramObject)
        {
            string result = string.Empty;

            try
            {
                IMappedStatement statement = _sqlMapper.GetMappedStatement(statementName);
 
                RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject,
                                                                             _sqlMapper.CreateSqlMapSession());
                result = scope.PreparedStatement.PreparedSql;                
                 
            }
            catch(Exception e)
            {
                throw new UnhandledException(string.Format("根据传入的statementName:{0}和参数列表 生成最终sql 时发生异常",statementName),e);
            }
            return result;
        }
    }
}
