﻿#nullable disable

using System;

namespace Amazon.Ec2
{
    public sealed class IamInstanceProfileSpecification
    {
        public IamInstanceProfileSpecification() { }

        public IamInstanceProfileSpecification(string nameOrArn)
        {
            if (nameOrArn is null) throw new ArgumentNullException(nameof(nameOrArn));

            if (nameOrArn.StartsWith("arn:", StringComparison.Ordinal))
            {
                Arn = nameOrArn;
            }
            else
            {
                Name = nameOrArn;
            }
        }

        public string Arn { get; init; }

        public string Name { get; init; }
    }
}