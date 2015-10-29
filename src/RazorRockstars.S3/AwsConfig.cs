using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
using ServiceStack;
using ServiceStack.Aws.DynamoDb;
using ServiceStack.Aws.S3;
using ServiceStack.Configuration;

namespace RazorRockstars.S3
{
    public static class AwsConfig
    {
        public const string DynamoDbUrl = "http://localhost:8000"; //Local DynamoDB
        public const string S3BucketName = "aws-razor-rockstars";

        public static IAmazonDynamoDB CreateAmazonDynamoDb()
        {
            var dynamoClient = new AmazonDynamoDBClient("keyId", "key", new AmazonDynamoDBConfig {
                ServiceURL = DynamoDbUrl,
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