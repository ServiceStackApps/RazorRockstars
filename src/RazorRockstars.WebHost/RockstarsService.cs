using System.Collections.Generic;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace RazorRockstars.WebHost
{
    [Route("/rockstars")]
    [Route("/rockstars/aged/{Age}")]
    public class Rockstars
    {
        public int? Age { get; set; }
        public int Id { get; set; }
    }

    [Route("/rockstars/delete/{Id}")]
    public class DeleteRockstar
    {
        public int Id { get; set; }
    }

    [Route("/rockstars/delete/reset")]
    public class ResetRockstars { }

    [Csv(CsvBehavior.FirstEnumerable)]
    public class RockstarsResponse
    {
        public int Total { get; set; }
        public int? Aged { get; set; }
        public List<Rockstar> Results { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Rockstars")]
    public class RockstarsService : Service
    {
        public object Get(Rockstars request)
        {
            return new RockstarsResponse {
                Aged = request.Age,
                Total = Db.Scalar<int>("select count(*) from Rockstar"),
                Results = request.Id != default(int) 
                    ? Db.Select<Rockstar>(q => q.Id == request.Id)
                    : request.Age.HasValue 
                        ? Db.Select<Rockstar>(q => q.Age == request.Age.Value)
                        : Db.Select<Rockstar>()
            };
        }

        public object Any(DeleteRockstar request)
        {
            Db.DeleteById<Rockstar>(request.Id);
            return Get(new Rockstars());
        }

        public object Post(Rockstar request)
        {
            Db.Insert(request);
            return Get(new Rockstars());
        }

        public object Any(ResetRockstars request)
        {
            Db.DeleteAll<Rockstar>();
            Db.InsertAll(AppHost.SeedData);
            return Get(new Rockstars());
        }
    }

}
