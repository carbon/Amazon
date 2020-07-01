using Xunit;

namespace Amazon.S3.Models.Tests
{
    public class ListObjectVersionsTests
    {
        [Fact]
        public void Parse()
        {
            string text = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<ListVersionsResult xmlns=""http://s3.amazonaws.com/doc/2006-03-01/"">
    <Name>bucket</Name>
    <Prefix>my</Prefix>
    <KeyMarker/>
    <VersionIdMarker/>
    <MaxKeys>5</MaxKeys>
    <IsTruncated>false</IsTruncated>
    <Version>
        <Key>my-image.jpg</Key>
        <VersionId>3/L4kqtJl40Nr8X8gdRQBpUMLUo</VersionId>
        <IsLatest>true</IsLatest>
        <LastModified>2009-10-12T17:50:30.000Z</LastModified>
        <ETag>""fba9dede5f27731c9771645a39863328""</ETag>
        <Size>434234</Size>
        <StorageClass>STANDARD</StorageClass>
        <Owner>
            <ID>75aa57f09aa0c8caeab4f8c24e99d10f8e7faeebf76c078efc7c6caea54ba06a</ID>
            <DisplayName>mtd@amazon.com</DisplayName>
        </Owner>
    </Version>
    <DeleteMarker>
        <Key>my-second-image.jpg</Key>
        <VersionId>03jpff543dhffds434rfdsFDN943fdsFkdmqnh892</VersionId>
        <IsLatest>true</IsLatest>
        <LastModified>2009-11-12T17:50:30.000Z</LastModified>
        <Owner>
            <ID>75aa57f09aa0c8caeab4f8c24e99d10f8e7faeebf76c078efc7c6caea54ba06a</ID>
            <DisplayName>mtd@amazon.com</DisplayName>
        </Owner>    
    </DeleteMarker>
    <Version>
        <Key>my-second-image.jpg</Key>
        <VersionId>QUpfdndhfd8438MNFDN93jdnJFkdmqnh893</VersionId>
        <IsLatest>false</IsLatest>
        <LastModified>2009-10-10T17:50:30.000Z</LastModified>
        <ETag>""9b2cf535f27731c974343645a3985328""</ETag>
        <Size>166434</Size>
        <StorageClass>STANDARD</StorageClass>
        <Owner>
            <ID>75aa57f09aa0c8caeab4f8c24e99d10f8e7faeebf76c078efc7c6caea54ba06a</ID>
            <DisplayName>mtd@amazon.com</DisplayName>
        </Owner>
    </Version>
    <DeleteMarker>
        <Key>my-third-image.jpg</Key>
        <VersionId>03jpff543dhffds434rfdsFDN943fdsFkdmqnh892</VersionId>
        <IsLatest>true</IsLatest>
        <LastModified>2009-10-15T17:50:30.000Z</LastModified>
        <Owner>
            <ID>75aa57f09aa0c8caeab4f8c24e99d10f8e7faeebf76c078efc7c6caea54ba06a</ID>
            <DisplayName>mtd@amazon.com</DisplayName>
        </Owner>    
    </DeleteMarker>   
    <Version>
        <Key>my-third-image.jpg</Key>
        <VersionId>UIORUnfndfhnw89493jJFJ</VersionId>
        <IsLatest>false</IsLatest>
        <LastModified>2009-10-11T12:50:30.000Z</LastModified>
        <ETag>""772cf535f27731c974343645a3985328""</ETag>
        <Size>64</Size>
        <StorageClass>STANDARD</StorageClass>
        <Owner>
            <ID>75aa57f09aa0c8caeab4f8c24e99d10f8e7faeebf76c078efc7c6caea54ba06a</ID>
            <DisplayName>mtd@amazon.com</DisplayName>
        </Owner>
     </Version>
</ListVersionsResult>";

            var result = ListVersionsResult.ParseXml(text);

            Assert.Equal("bucket", result.Name);
            Assert.Equal("my", result.Prefix);
            Assert.Equal(5, result.MaxKeys);
            Assert.False(result.IsTruncated);

            Assert.Equal(3, result.Versions.Length);

            Assert.Equal(2, result.DeleteMarkers.Length);

            var v0 = result.Versions[0];

            Assert.Equal("my-image.jpg", v0.Key);
            Assert.Equal("3/L4kqtJl40Nr8X8gdRQBpUMLUo", v0.VersionId);
            Assert.True(v0.IsLatest);
            Assert.Equal("\"fba9dede5f27731c9771645a39863328\"", v0.ETag);
            Assert.Equal(434234, v0.Size);
            Assert.Equal("STANDARD", v0.StorageClass);
            Assert.Equal("75aa57f09aa0c8caeab4f8c24e99d10f8e7faeebf76c078efc7c6caea54ba06a", v0.Owner.ID);
            Assert.Equal("mtd@amazon.com", v0.Owner.DisplayName);

            var d0 = result.DeleteMarkers[0];

            Assert.Equal("my-second-image.jpg", d0.Key);
            Assert.Equal("03jpff543dhffds434rfdsFDN943fdsFkdmqnh892", d0.VersionId);
            Assert.True(d0.IsLatest);
            Assert.Equal(2009, d0.LastModified.Year);
        }
    }
}