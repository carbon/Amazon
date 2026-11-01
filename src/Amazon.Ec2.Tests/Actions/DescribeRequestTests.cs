namespace Amazon.Ec2.Tests;

public class DescribeRequestTests
{
    [Fact]
    public void SerializesFiltersThenPagingThenIds()
    {
        var request = new DescribeInstancesRequest("i-1", "i-2") {
            MaxResults = 5,
            NextToken = "token"
        };

        request.Filters.Add(new Filter("instance-type", "t2.micro"));
        request.Filters.Add(new Filter("is-public", true));

        Assert.Equal(
            "Action=DescribeInstances&Filter.1.Name=instance-type&Filter.1.Value=t2.micro&Filter.2.Name=is-public&Filter.2.Value=true&MaxResults=5&NextToken=token&InstanceId.1=i-1&InstanceId.2=i-2",
            request.Serialize());
    }

    [Fact]
    public void OmitsUnsetPagingParameters()
    {
        Assert.Equal("Action=DescribeInstances", new DescribeInstancesRequest().Serialize());
    }

    [Fact]
    public void SerializesEachRequestWithItsIdPrefix()
    {
        Assert.Equal("Action=DescribeImages&ImageId.1=ami-1&ImageId.2=ami-2", new DescribeImagesRequest(["ami-1", "ami-2"]).Serialize());
        Assert.Equal("Action=DescribeInstanceTypes&InstanceType.1=t2.micro", new DescribeInstanceTypesRequest("t2.micro").Serialize());
        Assert.Equal("Action=DescribeNetworkInterfaces&NetworkInterfaceId.1=eni-1", new DescribeNetworkInterfacesRequest("eni-1").Serialize());
        Assert.Equal("Action=DescribeSubnets&SubnetId.1=subnet-1", new DescribeSubnetsRequest("subnet-1").Serialize());
        Assert.Equal("Action=DescribeVpcs&VpcId.1=vpc-1", new DescribeVpcsRequest("vpc-1").Serialize());
    }

    [Fact]
    public void RequestsWithoutIdsDescribeEverything()
    {
        Assert.Equal("Action=DescribeImages", new DescribeImagesRequest().Serialize());
        Assert.Equal("Action=DescribeInstanceTypes", new DescribeInstanceTypesRequest().Serialize());
        Assert.Equal("Action=DescribeNetworkInterfaces", new DescribeNetworkInterfacesRequest().Serialize());
        Assert.Equal("Action=DescribeSubnets", new DescribeSubnetsRequest().Serialize());
        Assert.Equal("Action=DescribeVpcs", new DescribeVpcsRequest().Serialize());
    }

    [Fact]
    public void DescribeImagesSendsOwnerParameter()
    {
        var request = new DescribeImagesRequest { OwnerIds = ["self", "amazon"] };

        Assert.Equal("Action=DescribeImages&Owner.1=self&Owner.2=amazon", request.Serialize());
    }

    [Fact]
    public void ConstructorsRejectNullIds()
    {
        Assert.Throws<ArgumentNullException>(() => new DescribeImagesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new DescribeInstancesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new DescribeInstanceTypesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new DescribeNetworkInterfacesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new DescribeVolumesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new DescribeVpcsRequest(null!));
    }

    [Fact]
    public void DescribeVolumesRejectsEmptyIds()
    {
        Assert.Throws<ArgumentException>(() => new DescribeVolumesRequest([]));
    }
}

public class FilterTests
{
    [Fact]
    public void BooleanValuesAreLowercase()
    {
        Assert.Equal("true", new Filter("a", true).Value);
        Assert.Equal("false", new Filter("a", false).Value);
    }

    [Fact]
    public void RejectsMissingNameOrValue()
    {
        Assert.Throws<ArgumentException>(() => new Filter("", "value"));
        Assert.Throws<ArgumentException>(() => new Filter("name", ""));
        Assert.Throws<ArgumentNullException>(() => new Filter(null!, "value"));
        Assert.Throws<ArgumentNullException>(() => new Filter("name", null!));
        Assert.Throws<ArgumentException>(() => new Filter("", true));
    }
}
