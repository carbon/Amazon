using System.Globalization;

namespace Amazon.S3.Models.Tests;

public class ListBucketResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = S3Serializer<ListBucketResult>.Deserialize(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <ListBucketResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
            	<Name>carbon</Name>
            	<Prefix>1</Prefix>
            	<MaxKeys>100</MaxKeys>
            	<IsTruncated>true</IsTruncated>
                <NextContinuationToken>1ueGcxLPRx1Tr/XYExHnhbYLgveDs2J/wm36Hy4vbOwM=</NextContinuationToken>
            	<Contents>
            		<Key>100000/800x600.jpeg</Key>
            		<LastModified>2009-06-20T09:54:05.000Z</LastModified>
            		<ETag>&quot;c55fad5b272947050bed993ec22c6d0d&quot;</ETag>
            		<Size>116879</Size>
            		<Owner>
            			<ID>9c18bda0312b59b259789b4acf29a06efdb6193a4ef51fcafa739f5cda4f3bf0</ID>
            			<DisplayName>jason17095</DisplayName>
            		</Owner>
            		<StorageClass>STANDARD</StorageClass>
            	</Contents>
            	<Contents>
            		<Key>100001/800x600.jpeg</Key>
            		<LastModified>2009-06-20T09:54:01.000Z</LastModified>
            		<ETag>&quot;4ef58e19a01ea04d4f9da27c6f6638d7&quot;</ETag>
            		<Size>116882</Size>
            		<Owner>
            			<ID>9c18bda0312b59b259789b4acf29a06efdb6193a4ef51fcafa739f5cda4f3bf0</ID>
            			<DisplayName>jason17095</DisplayName>
            		</Owner>
            		<StorageClass>STANDARD</StorageClass>
            	</Contents>
            </ListBucketResult>
            """u8.ToArray());

        Assert.Equal("carbon", result.Name);
        Assert.Equal("1", result.Prefix);

        Assert.Equal(116879, result.Items[0].Size);
        Assert.Equal(116882, result.Items[1].Size);
        Assert.True(result.IsTruncated);

        Assert.Equal("STANDARD", result.Items[1].StorageClass);

        Assert.Equal("100000/800x600.jpeg", result.Items[0].Key);
        Assert.Equal("100001/800x600.jpeg", result.Items[1].Key);
    }

    [Fact]
    public void Test()
    {
        var result = S3Serializer<ListBucketResult>.Deserialize(
            """
            <ListBucketResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
                <Name>carbon</Name>
                <Prefix>1</Prefix>
                <MaxKeys>100</MaxKeys>
                <IsTruncated>true</IsTruncated>
                <Contents>
                    <Key>100000/800x600.jpeg</Key>
                    <LastModified>2009-06-20T09:54:05.000Z</LastModified>
                    <ETag>&quot;c55fad5b272947050bed993ec22c6d0d&quot;</ETag>
                    <Size>116879</Size>
                    <Owner>
                        <ID>a</ID>
                        <DisplayName>1</DisplayName>
                    </Owner>
                    <StorageClass>STANDARD</StorageClass>
                </Contents>
                <Contents>
                    <Key>100001/800x600.jpeg</Key>
                    <LastModified>2009-06-20T09:54:01.000Z</LastModified>
                    <ETag>&quot;4ef58e19a01ea04d4f9da27c6f6638d7&quot;</ETag>
                    <Size>116882</Size>
                    <Owner>
                        <ID>a</ID>
                        <DisplayName>1</DisplayName>
                    </Owner>
                    <StorageClass>STANDARD</StorageClass>
                </Contents>
            </ListBucketResult>
            """u8.ToArray());

        Assert.Equal("carbon", result.Name);
        Assert.Equal("1", result.Prefix);
        Assert.Equal(100, result.MaxKeys);

        Assert.Equal(2, result.Items.Length);
        Assert.Equal("100000/800x600.jpeg", result.Items[0].Key);

        // Assert.Equal("STANDARD", result.Items[0].StorageClass);
        Assert.Equal(DateTime.Parse("2009-06-20T09:54:05.000Z", null, DateTimeStyles.AdjustToUniversal), result.Items[0].LastModified);
    }

    [Fact]
    public void CanDeserialize3()
    {
        // example via https://docs.aws.amazon.com/AmazonS3/latest/API/API_ListObjectsV2.html

        var result = S3Serializer<ListBucketResult>.Deserialize(
            """
            <ListBucketResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
              <Name>quotes</Name>
              <Prefix>E</Prefix>
              <StartAfter>ExampleGuide.pdf</StartAfter>
              <KeyCount>1</KeyCount>
              <MaxKeys>3</MaxKeys>
              <IsTruncated>false</IsTruncated>
              <Contents>
                <Key>ExampleObject.txt</Key>
                <LastModified>2013-09-17T18:07:53.000Z</LastModified>
                <ETag>"599bab3ed2c697f1d26842727561fd94"</ETag>
                <Size>857</Size>
                <StorageClass>REDUCED_REDUNDANCY</StorageClass>
              </Contents>
            </ListBucketResult>
            """u8.ToArray());

        Assert.Equal("quotes",           result.Name);
        Assert.Equal("E",                result.Prefix);
        Assert.Equal("ExampleGuide.pdf", result.StartAfter);
        Assert.Equal(1,                  result.KeyCount);
        Assert.Equal(3,                  result.MaxKeys);

        Assert.Single(result.Items);

        Assert.Equal("ExampleObject.txt",                                                                result.Items[0].Key);
        Assert.Equal(857,                                                                                result.Items[0].Size);
        Assert.Equal(DateTime.Parse("2013-09-17T18:07:53.000Z", null, DateTimeStyles.AdjustToUniversal), result.Items[0].LastModified);
    }
}
