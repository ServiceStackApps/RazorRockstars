using System.Net;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Formats;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Razor;

//The entire C# code for the stand-alone RazorRockstars demo.
namespace RazorRockstars.SelfHost
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("Test Razor", typeof(AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            LogManager.LogFactory = new ConsoleLogFactory();

            Plugins.Add(new RazorFormat());
            Plugins.Add(new MarkdownFormat());

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.CreateTableIfNotExists<Rockstar>();
                db.InsertAll(RockstarsService.SeedData);
            }

            this.CustomErrorHttpHandlers[HttpStatusCode.NotFound] = new RazorHandler("/notfound");
            this.CustomErrorHttpHandlers[HttpStatusCode.Unauthorized] = new RazorHandler("/login");
        }
    }
}