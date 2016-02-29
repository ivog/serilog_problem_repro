using System.IO;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Serilog;
using Serilog.Context;
using Xunit;

namespace ClassLibrary1
{
    public class Class1
    {
        [Fact]
        public void test()
        {
            LogContext.PushProperty("test", "bla");
            Log.Debug("test");

            ISessionFactory factory = FluentNHibernate.Cfg.Fluently.Configure()
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Class1>())
                .Database(SQLiteConfiguration.Standard.UsingFile("firstProject.db"))
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();

            factory.OpenSession();
        }

        private static void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists("firstProject.db"))
                File.Delete("firstProject.db");

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }
    }
}
