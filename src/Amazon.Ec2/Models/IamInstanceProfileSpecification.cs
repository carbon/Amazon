using System;

namespace Amazon.Ec2
{
    public class IamInstanceProfileSpecification
    {
        public IamInstanceProfileSpecification() { }

        public IamInstanceProfileSpecification(string nameOrArn)
        {
            if (nameOrArn is null) throw new ArgumentNullException(nameof(nameOrArn));

            if (nameOrArn.StartsWith("arn:"))
            {
                Arn = nameOrArn;
            }
            else
            {
                Name = nameOrArn;
            }
        }

        public string Arn { get; set; }

        public string Name { get; set; }
    }
}