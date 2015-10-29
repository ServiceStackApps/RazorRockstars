using System;
using Amazon.DynamoDBv2;
using ServiceStack.Aws.DynamoDb;
using ServiceStack.Configuration;

namespace RazorRockstars.S3
{
    public static class AwsConfig
    {
        public static string S3BucketName
        {
            get { return ConfigUtils.GetAppSetting("S3BucketName", "aws-razor-rockstars"); }
        }

        public static IAmazonDynamoDB CreateAmazonDynamoDb()
        {
            var dynamoClient = new AmazonDynamoDBClient(
                ConfigUtils.GetNullableAppSetting("DynamoDbAccessKey") ?? AwsAccessKey, 
                ConfigUtils.GetNullableAppSetting("DynamoDbSecretKey") ?? AwsSecretKey, 
                new AmazonDynamoDBConfig {
                    ServiceURL = ConfigUtils.GetAppSetting("DynamoDbUrl","http://dynamodb.us-east-1.amazonaws.com"),
                });
            return dynamoClient;
        }

        public static IPocoDynamo CreatePocoDynamo()
        {
            return new PocoDynamo(CreateAmazonDynamoDb());
        }

        public static string AwsAccessKey
        {
            get
            {
                var accessKey = ConfigUtils.GetNullableAppSetting("AWS_ACCESS_KEY")
                    ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");

                if (string.IsNullOrEmpty(accessKey))
                    throw new ArgumentException("AWS_ACCESS_KEY must be defined in App.config or Environment Variable");

                return accessKey;
            }
        }

        public static string AwsSecretKey
        {
            get
            {
                var secretKey = ConfigUtils.GetNullableAppSetting("AWS_SECRET_KEY")
                    ?? Environment.GetEnvironmentVariable("AWS_SECRET_KEY");

                if (string.IsNullOrEmpty(secretKey))
                    throw new ArgumentException("AWS_SECRET_KEY must be defined in App.config or Environment Variable");

                return secretKey;
            }
        }
    }
}