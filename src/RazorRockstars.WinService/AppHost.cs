using System.Net;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.Formats;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Razor;

//The entire C# code for the stand-alone RazorRockstars demo.
namespace RazorRockstars.WinService
{
    public class AppHost : AppHostHttpListenerBase
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

            //AddAuthentication(container); //Uncomment to enable User Authentication
        }
    }

    //Poco Data Model for OrmLite + SeedData 
    [Route("/rockstars", "POST")]
    public class Rockstar
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public bool Alive { get; set; }

        public string Url
        {
            get { return "/stars/{0}/{1}".Fmt(Alive ? "alive" : "dead", LastName.ToLower()); }
        }

        public Rockstar() { }
        public Rockstar(int id, string firstName, string lastName, int age, bool alive)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Alive = alive;
        }
    }
}