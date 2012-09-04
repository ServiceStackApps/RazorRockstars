using System.Net;
using Funq;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

//The entire C# code for the stand-alone RazorRockstars demo.
namespace RazorRockstars.WebHost
{
    public class AppHost : AppHostBase 
    {
        public AppHost() : base("Test Razor", typeof(AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            Plugins.Add(new RazorFormat());

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(":memory:", false, SqliteDialect.Provider));

            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.CreateTableIfNotExists<Rockstar>();
                db.InsertAll(Rockstar.SeedData);
            }

            SetConfig(new EndpointHostConfig {
                CustomHttpHandlers = {
                    { HttpStatusCode.NotFound, new RazorHandler("/notfound") }
                }
            });
		}
    }

    public class Rockstar
    {
        public static Rockstar[] SeedData = new[] {
            new Rockstar(1, "Jimi", "Hendrix", 27, false), 
            new Rockstar(2, "Janis", "Joplin", 27, false), 
            new Rockstar(4, "Kurt", "Cobain", 27, false),              
            new Rockstar(5, "Elvis", "Presley", 42, false), 
            new Rockstar(6, "Michael", "Jackson", 50, false), 
            new Rockstar(7, "Eddie", "Vedder", 47, true), 
            new Rockstar(8, "Dave", "Grohl", 43, true), 
            new Rockstar(9, "Courtney", "Love", 48, true), 
            new Rockstar(10, "Bruce", "Springsteen", 62, true), 
        };

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