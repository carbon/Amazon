//  This C# program is to Enable or Disable Versioning in an Amazon S3 bucket

//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3

//  Enter Access Key & Secret Key provided in AWS console
//  You can use AWS Console Access Key & Secret Key
//  For Security purpose create a user in IAM and provide full access to S3 in policies section

//  The staus can either be disable or enable
//  RegionOfTheBucket -> do not enter region code, just enter the region name -> mumbai

//          status = disable;
//          RegionOfTheBucket = mumbai;


using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace EnableDisableVersioning
{
    class Program
    {
        //  This funtion is to get the bucket region code
        public static string ClientRegion(string RegionOfClient)
        {
            switch (RegionOfClient)
            {
                case "ireland":
                    return "eu-west-1";
                case "mumbai":
                    return "ap-south-1";
                case "frankfurt":
                    return "eu-central-1";
                case "london":
                    return "eu-west-2";
                case "sydney":
                    return "ap-southeast-2";
                case "ohio":
                    return "us-east-2";
                case "virginia":
                    return "us-east-1";
                case "california":
                    return "us-west-1";
                case "oregon":
                    return "us-west-2";
                case "singapore":
                    return "ap-southeast-1";
                case "tokyo":
                    return "ap-northeast-1";
                case "canada":
                    return "ca-central-1";
                case "seoul":
                    return "ap-northeast-2";
                case "sao paulo":
                    return "sa-east-1";
            }
            return "";
        }
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key *** ";
            string SecretKey = " *** Enter Secret Key *** ";
            string NameOfTheBucket = " *** Enter Name Of The Bucket Here *** ";
            string status = "*** Enter Status ***  Enter -> (enable/disable)";
            string RegionOfTheBucket = " *** Enter Region Of The bucket *** Eg: mumbai/ireland";
            status = status.ToLower();
            AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
            PutBucketVersioningRequest BucketVersioning = new PutBucketVersioningRequest();
            try
            {
                BucketVersioning.BucketName = NameOfTheBucket;
                if (status == "enable")
                {
                    BucketVersioning.VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Enabled };
                    client.PutBucketVersioning(BucketVersioning);
                    Console.WriteLine("Versioning Enabled");
                }
                else if (status == "disable")
                {
                    BucketVersioning.VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Suspended };
                    client.PutBucketVersioning(BucketVersioning);
                    Console.WriteLine("Versioning Disabled");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
            Console.ReadLine();
        }
    }
}