using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace CretBucket
{
    class Program
    {
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key *** ";
            string SecretKey = " *** Enter Secret Key *** ";

            try
            {
                // Region of the Amazon S3 Client is Mumbai i.e APSouth1
                using (AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1))
                {
                    ListBucketsResponse response = client.ListBuckets();
                    Console.WriteLine("\tCreation Date \t\t" + "Bucket Name");
                    foreach (S3Bucket b in response.Buckets)
                    {
                        Console.WriteLine(b.CreationDate + " \t" + b.BucketName);
                    }
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