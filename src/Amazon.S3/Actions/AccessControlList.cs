using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace BucketMake~NotMakePublic
{
    class Program
    {
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
                case "paris":
                    return "eu-west-3";
                case "seoul":
                    return "ap-northeast-2";
                case "sao paulo":
                    return "sa-east-1";
            }
            return "";
        }
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key Here *** ";
            string SecretKey = " *** Enter Secret Key Here *** ";
            string NameOfTheBucket = " *** Name Of The Bucket *** ";
            string MakeStatus = " *** private/publicread/publicreadwrite *** ";
            string RegionOfTheBucket = " *** Enter The Region Of The Bucket (Eg: mumbai) ***";
            MakeStatus = MakeStatus.ToLower();
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                if(MakeStatus == "publicread")
                {
                    PutACLRequest Request = new PutACLRequest
                    {
                        BucketName = NameOfTheBucket,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    client.PutACL(Request);
                }
                else if(MakeStatus == "publicreadwrite")
                {
                    PutACLRequest Request = new PutACLRequest
                    {
                        BucketName = NameOfTheBucket,
                        CannedACL = S3CannedACL.PublicReadWrite
                    };
                    client.PutACL(Request);
                }
                else if(MakeStatus == "private")
                {
                    PutACLRequest Request = new PutACLRequest
                    {
                        BucketName = NameOfTheBucket,
                        CannedACL = S3CannedACL.Private
                    };
                    client.PutACL(Request);
                }
                Console.WriteLine("Access type changed");
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
            Console.ReadLine();
        }
    }
}