using System;
using System.Net;
using System.Reflection;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using RazorRockstars.CompiledViews;

namespace RazorRockstars.MacHost
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("Test Razor", typeof(RockstarsService).Assembly) { }

        public override void Configure(Container container)
        {
            SetConfig(new HostConfig {
                DebugMode = true,
                EmbeddedResourceBaseTypes = { GetType(), typeof(BaseTypeMarker) },
            });

            Plugins.Add(new RazorFormat {
                LoadFromAssemblies = { typeof(RockstarsService).Assembly }
            });

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

