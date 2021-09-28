using System;
using System.Globalization;

namespace Amazon.S3.Models.Tests;

public class ListBucketResultTests
{
    [Fact]
    public void Test2()
    {
        var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<ListBucketResult xmlns=""http://s3.amazonaws.com/doc/2006-03-01/"">
	<Name>cmcdn</Name>
	<Prefix>1</Prefix>
	<Marker></Marker>
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
</ListBucketResult>";

        var result = ListBucketResult.ParseXml(xml);

        Assert.Equal("cmcdn", result.Name);
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
        string xml = @"<ListBucketResult xmlns=""http://s3.amazonaws.com/doc/2006-03-01/"">
            <Name>cmcdn</Name>
            <Prefix>1</Prefix>
            <Marker></Marker>
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
        </ListBucketResult>";

        var result = ListBucketResult.ParseXml(xml);

        Assert.Equal("cmcdn", result.Name);
        Assert.Equal("1", result.Prefix);
        Assert.Equal("", result.Marker);
        Assert.Equal(100, result.MaxKeys);

        Assert.Equal(2, result.Items.Length);
        Assert.Equal("100000/800x600.jpeg", result.Items[0].Key);

        // Assert.Equal("STANDARD", result.Items[0].StorageClass);
        Assert.Equal(DateTime.Parse("2009-06-20T09:54:05.000Z", null, DateTimeStyles.AdjustToUniversal), result.Items[0].LastModified);
    }
}
