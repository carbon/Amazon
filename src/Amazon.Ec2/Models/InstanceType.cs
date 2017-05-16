using System.Collections.Generic;
using System.Linq;

namespace Amazon.Ec2.Models
{
    public class InstanceType
    {
        public InstanceType() { }

        public InstanceType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }


    public class InstanceTypeMap
    {
        public static InstanceType[] All => map.Select(m => m.Value).ToArray();

        private static readonly Dictionary<string, InstanceType> map = new Dictionary<string, InstanceType> {
            { "c1.medium"          , new InstanceType("c1.medium") },
            { "c1.xlarge"          , new InstanceType("c1.xlarge") },
            { "c3.2xlarge"         , new InstanceType("c3.2xlarge") },
            { "c3.4xlarge"         , new InstanceType("c3.4xlarge") },
            { "c3.8xlarge"         , new InstanceType("c3.8xlarge") },
            { "c3.large"           , new InstanceType("c3.large") },
            { "c3.xlarge"          , new InstanceType("c3.xlarge") },
            { "c4.large"           , new InstanceType("c4.large") },
            { "c4.xlarge"          , new InstanceType("c4.xlarge") },
            { "c4.2xlarge"         , new InstanceType("c4.2xlarge") },
            { "c4.4xlarge"         , new InstanceType("c4.4xlarge") },
            { "c4.8xlarge"         , new InstanceType("c4.8xlarge") },
       
            { "cc2.8xlarge"        , new InstanceType("cc2.8xlarge") },
            { "cg1.4xlarge"        , new InstanceType("cg1.4xlarge") },
            { "cr1.8xlarge"        , new InstanceType("cr1.8xlarge") },
        
            { "d2.2xlarge"         , new InstanceType("d2.2xlarge") },
            { "d2.4xlarge"         , new InstanceType("d2.4xlarge") },
            { "d2.8xlarge"         , new InstanceType("d2.8xlarge") },
            { "d2.xlarge"          , new InstanceType("d2.xlarge") },

            { "f1.16xlarge"        , new InstanceType("f1.16xlarge") },
            { "f1.2xlarge"         , new InstanceType("f1.2xlarge") },
            { "g2.2xlarge"         , new InstanceType("g2.2xlarge") },
            { "g2.8xlarge"         , new InstanceType("g2.8xlarge") },

            { "hi1.4xlarge"        , new InstanceType("hi1.4xlarge") },
            { "hs1.8xlarge"        , new InstanceType("hs1.8xlarge") },

            { "i2.2xlarge"         , new InstanceType("i2.2xlarge") },
            { "i2.4xlarge"         , new InstanceType("i2.4xlarge") },
            { "i2.8xlarge"         , new InstanceType("i2.8xlarge") },
            { "i2.xlarge"          , new InstanceType("i2.xlarge") },

            { "m1.large"           , new InstanceType("m1.large") },
            { "m1.medium"          , new InstanceType("m1.medium") },
            { "m1.small"           , new InstanceType("m1.small") },
            { "m1.xlarge"          , new InstanceType("m1.xlarge") },
            { "m2.2xlarge"         , new InstanceType("m2.2xlarge") },
            { "m2.4xlarge"         , new InstanceType("m2.4xlarge") },
            { "m2.xlarge"          , new InstanceType("m2.xlarge") },
            { "m3.2xlarge"         , new InstanceType("m3.2xlarge") },
            { "m3.large"           , new InstanceType("m3.large") },
            { "m3.medium"          , new InstanceType("m3.medium") },
            { "m3.xlarge"          , new InstanceType("m3.xlarge") },

            { "m4.10xlarge"        , new InstanceType("m4.10xlarge") },
            { "m4.16xlarge"        , new InstanceType("m4.16xlarge") },
            { "m4.2xlarge"         , new InstanceType("m4.2xlarge") },
            { "m4.4xlarge"         , new InstanceType("m4.4xlarge") },
            { "m4.large"           , new InstanceType("m4.large") },
            { "m4.xlarge"          , new InstanceType("m4.xlarge") },

            { "p2.16xlarge"        , new InstanceType("p2.16xlarge") },
            { "p2.8xlarge"         , new InstanceType("p2.8xlarge") },
            { "p2.xlarge"          , new InstanceType("p2.xlarge") },

            { "r3.2xlarge"         , new InstanceType("r3.2xlarge") },
            { "r3.4xlarge"         , new InstanceType("r3.4xlarge") },
            { "r3.8xlarge"         , new InstanceType("r3.8xlarge") },
            { "r3.large"           , new InstanceType("r3.large") },
            { "r3.xlarge"          , new InstanceType("r3.xlarge") },
            { "r4.16xlarge"        , new InstanceType("r4.16xlarge") },
            { "r4.2xlarge"         , new InstanceType("r4.2xlarge") },
            { "r4.4xlarge"         , new InstanceType("r4.4xlarge") },
            { "r4.8xlarge"         , new InstanceType("r4.8xlarge") },
            { "r4.large"           , new InstanceType("r4.large") },
            { "r4.xlarge"          , new InstanceType("r4.xlarge") },

            { "t1.micro"           , new InstanceType("t1.micro") },
            { "t2.2xlarge"         , new InstanceType("t2.2xlarge") },
            { "t2.large"           , new InstanceType("t2.large") },
            { "t2.medium"          , new InstanceType("t2.medium") },
            { "t2.micro"           , new InstanceType("t2.micro") },
            { "t2.nano"            , new InstanceType("t2.nano") },
            { "t2.small"           , new InstanceType("t2.small") },
            { "t2.xlarge"          , new InstanceType("t2.xlarge") },
            { "x1.16xlarge"        , new InstanceType("x1.16xlarge") },
            { "x1.32xlarge"        , new InstanceType("x1.32xlarge") },
        };
    }
}
