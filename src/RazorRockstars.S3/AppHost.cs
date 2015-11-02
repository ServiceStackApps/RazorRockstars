using System.Collections.Generic;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Funq;
using ServiceStack;
using ServiceStack.Aws.DynamoDb;
using ServiceStack.Aws.S3;
using ServiceStack.Formats;
using ServiceStack.IO;
using ServiceStack.Razor;

namespace RazorRockstars.S3
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("S3 RazorRockstars", typeof(RockstarsService).Assembly) { }

        public override void Configure(Container container)
        {
            //RockstarsService is powered by PocoDynamo and DynamoDB
            container.Register<IPocoDynamo>(c => new PocoDynamo(AwsConfig.CreateAmazonDynamoDb()));

            //Add S3 Bucket as lowest priority Virtual Path Provider 
            //All Razor Views, Markdown Content, imgs, js, css, etc are being served from an S3 Bucket
            var s3Client = new AmazonS3Client(AwsConfig.AwsAccessKey, AwsConfig.AwsSecretKey, RegionEndpoint.USEast1);
            VirtualFiles = new S3VirtualPathProvider(s3Client, AwsConfig.S3BucketName, this);

            //Register and Create Missing DynamoDB Tables
            var db = container.Resolve<IPocoDynamo>();
            db.RegisterTable<Rockstar>();
            db.InitSchema();

            //Insert Rockstar POCOs in DynamoDB
            db.PutItems(RockstarsService.SeedData);

            //Check LastModified in S3 for changes and recompile Razor + Markdown pages if needed
            GetPlugin<MarkdownFormat>().CheckLastModifiedForChanges = true;
            Plugins.Add(new RazorFormat { CheckLastModifiedForChanges = true });
        }

        public override List<IVirtualPathProvider> GetVirtualFileSources()
        {
            var pathProviders = base.GetVirtualFileSources();
            pathProviders.Add(VirtualFiles);
            return pathProviders;
        }
    }
}