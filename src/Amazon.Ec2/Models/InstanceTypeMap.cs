using System.Linq;

namespace Amazon.Ec2.Models;

public sealed class InstanceTypeMap
{
    public static InstanceType[] All => map.Select(m => m.Value).ToArray();

    private static readonly Dictionary<string, InstanceType> map = new () {
        { "c1.medium"   , new ("c1.medium") },
        { "c1.xlarge"   , new ("c1.xlarge") },
        { "c3.2xlarge"  , new ("c3.2xlarge") },
        { "c3.4xlarge"  , new ("c3.4xlarge") },
        { "c3.8xlarge"  , new ("c3.8xlarge") },
        { "c3.large"    , new ("c3.large") },
        { "c3.xlarge"   , new ("c3.xlarge") },
        { "c4.large"    , new ("c4.large") },
        { "c4.xlarge"   , new ("c4.xlarge") },
        { "c4.2xlarge"  , new ("c4.2xlarge") },
        { "c4.4xlarge"  , new ("c4.4xlarge") },
        { "c4.8xlarge"  , new ("c4.8xlarge") },
       
        { "cc2.8xlarge" , new ("cc2.8xlarge") },
        { "cg1.4xlarge" , new ("cg1.4xlarge") },
        { "cr1.8xlarge" , new ("cr1.8xlarge") },
        
        { "d2.2xlarge"  , new ("d2.2xlarge") },
        { "d2.4xlarge"  , new ("d2.4xlarge") },
        { "d2.8xlarge"  , new ("d2.8xlarge") },
        { "d2.xlarge"   , new ("d2.xlarge") },

        { "f1.16xlarge" , new ("f1.16xlarge") },
        { "f1.2xlarge"  , new ("f1.2xlarge") },
        { "g2.2xlarge"  , new ("g2.2xlarge") },
        { "g2.8xlarge"  , new ("g2.8xlarge") },

        { "hi1.4xlarge" , new ("hi1.4xlarge") },
        { "hs1.8xlarge" , new ("hs1.8xlarge") },

        { "i2.2xlarge"  , new ("i2.2xlarge") },
        { "i2.4xlarge"  , new ("i2.4xlarge") },
        { "i2.8xlarge"  , new ("i2.8xlarge") },
        { "i2.xlarge"   , new ("i2.xlarge") },

        { "m1.large"    , new ("m1.large") },
        { "m1.medium"   , new ("m1.medium") },
        { "m1.small"    , new ("m1.small") },
        { "m1.xlarge"   , new ("m1.xlarge") },
        { "m2.2xlarge"  , new ("m2.2xlarge") },
        { "m2.4xlarge"  , new ("m2.4xlarge") },
        { "m2.xlarge"   , new ("m2.xlarge") },
        { "m3.2xlarge"  , new ("m3.2xlarge") },
        { "m3.large"    , new ("m3.large") },
        { "m3.medium"   , new ("m3.medium") },
        { "m3.xlarge"   , new ("m3.xlarge") },

        { "m4.10xlarge" , new ("m4.10xlarge") },
        { "m4.16xlarge" , new ("m4.16xlarge") },
        { "m4.2xlarge"  , new ("m4.2xlarge") },
        { "m4.4xlarge"  , new ("m4.4xlarge") },
        { "m4.large"    , new ("m4.large") },
        { "m4.xlarge"   , new ("m4.xlarge") },

        { "p2.16xlarge" , new ("p2.16xlarge") },
        { "p2.8xlarge"  , new ("p2.8xlarge") },
        { "p2.xlarge"   , new ("p2.xlarge") },

        { "r3.2xlarge"  , new ("r3.2xlarge") },
        { "r3.4xlarge"  , new ("r3.4xlarge") },
        { "r3.8xlarge"  , new ("r3.8xlarge") },
        { "r3.large"    , new ("r3.large") },
        { "r3.xlarge"   , new ("r3.xlarge") },
        { "r4.16xlarge" , new ("r4.16xlarge") },
        { "r4.2xlarge"  , new ("r4.2xlarge") },
        { "r4.4xlarge"  , new ("r4.4xlarge") },
        { "r4.8xlarge"  , new ("r4.8xlarge") },
        { "r4.large"    , new ("r4.large") },
        { "r4.xlarge"   , new ("r4.xlarge") },

        { "t1.micro"    , new ("t1.micro") },
        { "t2.2xlarge"  , new ("t2.2xlarge") },
        { "t2.large"    , new ("t2.large") },
        { "t2.medium"   , new ("t2.medium") },
        { "t2.micro"    , new ("t2.micro") },
        { "t2.nano"     , new ("t2.nano") },
        { "t2.small"    , new ("t2.small") },
        { "t2.xlarge"   , new ("t2.xlarge") },
        { "x1.16xlarge" , new ("x1.16xlarge") },
        { "x1.32xlarge" , new ("x1.32xlarge") },
    };
}