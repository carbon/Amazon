using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace DeleteBucket
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
                case "paris":
                    return "eu-west-3";
                case "tokyo":
                    return "ap-northeast-1";
                case "canada":
                    return "ca-central-1";
                case "seoul":
                    return "ap-northeast-2";
                case "sao paulo":
                    return "sa-east-1";
            }
            return "ap-south-1";
        }
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key Here *** ";
            string SecretKey = " *** Enter Secret Key Here *** ";
            string NameOfTheBucket = " *** Name Of The Bucket *** ";
            string RegionOfTheBucket = " *** Enter The Region Of The Bucket (Eg: mumbai) ***";
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                ListObjectsRequest ObjectRequest = new ListObjectsRequest
                {
                    BucketName = NameOfTheBucket
                };
                ListObjectsResponse ListResponse;
                do
                {
                    ListResponse = client.ListObjects(ObjectRequest);
                    foreach (S3Object obj in ListResponse.S3Objects)
                    {
                        DeleteObjectRequest DeleteObject = new DeleteObjectRequest
                        {
                            BucketName = NameOfTheBucket,
                            Key = obj.Key
                        };
                        client.DeleteObject(DeleteObject);
                    }
                    ObjectRequest.Marker = ListResponse.NextMarker;
                } while (ListResponse.IsTruncated);

                DeleteBucketRequest DeleteRequest = new DeleteBucketRequest
                {
                    BucketName = NameOfTheBucket,
                    UseClientRegion = true
                };
                client.DeleteBucket(DeleteRequest);
                Console.WriteLine("Bucket Deleted");
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
            Console.ReadLine();
        }
    }
}