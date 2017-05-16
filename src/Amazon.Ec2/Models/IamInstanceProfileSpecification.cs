using System;

namespace Amazon.Ec2
{
    public class IamInstanceProfileSpecification
    {
        public IamInstanceProfileSpecification() { }

        public IamInstanceProfileSpecification(string nameOrArn)
        {
            #region Preconditions

            if (nameOrArn == null)
                throw new ArgumentNullException(nameof(nameOrArn));

            #endregion

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