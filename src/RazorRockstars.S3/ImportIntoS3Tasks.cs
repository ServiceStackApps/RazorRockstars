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
        public void Copy_Razor_files_to_awsdemo_Bucket()
        {
            var fs = new FileSystemVirtualPathProvider(appHost, "~/../RazorRockstars.WebHost".MapHostAbsolutePath());

            var skipDirs = new[] { "bin", "obj" };
            var matchingFileTypes = new[] { "cshtml", "md", "css", "js", "png", "jpg" };
            var replaceHtml = new Dictionary<string, string> {
                { "", "" },
            };

            foreach (var file in fs.GetAllFiles())
            {
                if (skipDirs.Any(x => file.VirtualPath.StartsWith(x))) continue;
                if (!matchingFileTypes.Contains(file.Extension)) continue;

                using (var stream = file.OpenRead())
                {
                    s3.WriteFile(file.VirtualPath, stream);
                }
            }
        }
    }
}