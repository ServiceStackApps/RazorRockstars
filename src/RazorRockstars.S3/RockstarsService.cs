using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.Aws.DynamoDb;
using ServiceStack.DataAnnotations;
using ServiceStack.Text;

namespace RazorRockstars.S3
{
    [Route("/rockstars")]
    [Route("/rockstars/{Id}")]
    [Route("/rockstars/aged/{Age}")]
    public class SearchRockstars
    {
        public int? Age { get; set; }
        public int Id { get; set; }
    }

    [Route("/rockstars/delete/{Id}")]
    public class DeleteRockstar
    {
        public int Id { get; set; }
    }

    [Route("/reset")]
    public class ResetRockstars { }

    [Csv(CsvBehavior.FirstEnumerable)]
    public class RockstarsResponse
    {
        public int Total { get; set; }
        public int? Aged { get; set; }
        public List<Rockstar> Results { get; set; }
    }

    //Poco Data Model for DynamoDB + SeedData 
    [Route("/rockstars", "POST")]
    [References(typeof(RockstarAgeIndex))]
    public class Rockstar
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool Alive { get; set; }

        public string Url
        {
            get { return "/stars/{0}/{1}/".Fmt(Alive ? "alive" : "dead", LastName.ToLower()); }
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

    public class RockstarAgeIndex : IGlobalIndex<Rockstar>
    {
        [HashKey]
        public int Age { get; set; }

        [RangeKey]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Alive { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Rockstars")]
    public class RockstarsService : Service
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

        public IPocoDynamo PocoDynamo { get; set; }

        public object Get(SearchRockstars request)
        {
            return new RockstarsResponse
            {
                Aged = request.Age,
                Total = (int)PocoDynamo.ScanItemCount<Rockstar>(),
                Results = request.Id != default(int)
                    ? new[] { PocoDynamo.GetItem<Rockstar>(request.Id) }.ToList()
                    : request.Age.HasValue
                        ? PocoDynamo.FromQueryIndex<RockstarAgeIndex>(q => q.Age == request.Age.Value).QueryInto<Rockstar>().ToList()
                        : PocoDynamo.ScanAll<Rockstar>().ToList()
            };
        }

        public object Any(DeleteRockstar request)
        {
            PocoDynamo.DeleteItem<Rockstar>(request.Id);
            return Get(new SearchRockstars());
        }

        public object Post(Rockstar request)
        {
            PocoDynamo.PutItem(request);
            return Get(new SearchRockstars());
        }

        public object Any(ResetRockstars request)
        {
            var rockstarIds = PocoDynamo.FromScan<Rockstar>().Select(x => x.Id).Scan().Map(x => x.Id);
            PocoDynamo.DeleteItems<Rockstar>(rockstarIds);
            PocoDynamo.PutItems(SeedData);
            return Get(new SearchRockstars());
        }
    }
}
