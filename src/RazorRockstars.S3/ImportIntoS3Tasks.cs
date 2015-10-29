using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.S3;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Aws.S3;
using ServiceStack.Testing;
using ServiceStack.VirtualPath;

namespace RazorRockstars.S3
{
    [TestFixture]
    public class ImportIntoS3Tasks
    {
        readonly ServiceStackHost appHost;
        readonly S3VirtualPathProvider s3;

        public ImportIntoS3Tasks()
        {
            appHost = new BasicAppHost().Init();

            var s3Client = new AmazonS3Client(AwsConfig.AwsAccessKey, AwsConfig.AwsSecretKey, RegionEndpoint.USEast1);
            s3 = new S3VirtualPathProvider(s3Client, AwsConfig.S3BucketName, appHost);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void Drop_and_Create_Bucket()
        {
            try
            {
                s3.ClearBucket();
                s3.AmazonS3.DeleteBucket(AwsConfig.S3BucketName);
            }
            catch { }

            s3.AmazonS3.PutBucket(AwsConfig.S3BucketName);
        }

        [Test]
        public void Copy_Razor_files_to_AWS_Bucket()
        {
            var fs = new FileSystemVirtualPathProvider(appHost, "~/../RazorRockstars.WebHost".MapHostAbsolutePath());

            var skipDirs = new[] { "bin", "obj" };
            var matchingFileTypes = new[] { "cshtml", "md", "css", "js", "png", "jpg" };
            var replaceHtml = new Dictionary<string, string> {
                { "title-bg.png", "title-bg-aws.png" }, //Title Background
                { "https://gist.github.com/3617557.js", "https://gist.github.com/mythz/396dbf54ce6079cc8b2d.js" }, //AppHost.cs
                { "https://gist.github.com/3616766.js", "https://gist.github.com/mythz/ca524426715191b8059d.js" }, //RockstarsService.cs
            };

            foreach (var file in fs.GetAllFiles())
            {
                if (skipDirs.Any(x => file.VirtualPath.StartsWith(x))) continue;
                if (!matchingFileTypes.Contains(file.Extension)) continue;

                if (file.Extension == "cshtml")
                {
                    var contents = file.ReadAllText();
                    foreach (var entry in replaceHtml)
                    {
                        contents = contents.Replace(entry.Key, entry.Value);
                    }
                    s3.WriteFile(file.VirtualPath, contents);
                }
                else
                {
                    using (var stream = file.OpenRead())
                    {
                        s3.WriteFile(file.VirtualPath, stream);
                    }
                }
            }
        }
    }
}