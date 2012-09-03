using System.Collections.Generic;
using System.Runtime.Serialization;
using Funq;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
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

    [Route("/rockstars")]
    [Route("/rockstars/aged/{Age}")]
    [Route("/rockstars/delete/{Delete}")]
    [Route("/rockstars/{Id}")]
    public class Rockstars
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Delete { get; set; }
    }

    //DataContract attributes for CSV Format to detect a DTO so only serializes first Enumerable field
    [DataContract] 
    public class RockstarsResponse
    {
        [DataMember] public int Total { get; set; }
        [DataMember] public int? Aged { get; set; }
        [DataMember] public List<Rockstar> Results { get; set; }
    }

    public class RockstarsService : RestServiceBase<Rockstars>
    {
        public IDbConnectionFactory DbFactory { get; set; }

        public override object OnGet(Rockstars request)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                if (request.Delete == "reset")
                {
                    db.DeleteAll<Rockstar>();
                    db.InsertAll(Rockstar.SeedData);
                }
                else if (request.Delete.IsInt())
                {
                    db.DeleteById<Rockstar>(request.Delete.ToInt());
                }

                return new RockstarsResponse {
                    Aged = request.Age,
                    Total = db.Scalar<int>("select count(*) from Rockstar"),
                    Results = request.Id != default(int) ?
                        db.Select<Rockstar>(q => q.Id == request.Id)
                          : request.Age.HasValue ?
                        db.Select<Rockstar>(q => q.Age == request.Age.Value)
                          : db.Select<Rockstar>()
                };
            }
        }

        public override object OnPost(Rockstars request)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Insert(request.TranslateTo<Rockstar>());
                return OnGet(new Rockstars());
            }
        }
    }
}