using System.Collections.Generic;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace RazorRockstars.WebHost
{
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
        public bool Alive { get; set; }
        public string Delete { get; set; }
    }

    [Csv(CsvBehavior.FirstEnumerable)]
    public class RockstarsResponse
    {
        public int Total { get; set; }
        public int? Aged { get; set; }
        public List<Rockstar> Results { get; set; }
    }

    [ClientCanSwapTemplates]
    public class RockstarsService : RestServiceBase<Rockstars>
    {
        public IDbConnectionFactory DbFactory { get; set; }

        public override object OnGet(Rockstars request)
        {
            using (var db = DbFactory.Open())
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
            using (var db = DbFactory.Open())
            {
                db.Insert(request.TranslateTo<Rockstar>());
                return OnGet(new Rockstars());
            }
        }
    }
}