using System.Xml.Linq;

namespace Amazon.S3.Models.Tests;

public class BatchDeleteTests
{
    [Fact]
    public void CanSerialize_Silent()
    {
        var batch = new DeleteBatch(new[] { "1", "2" }, quite: true);

        Assert.Equal(
            """
            <Delete>
              <Quiet>true</Quiet>
              <Object>
                <Key>1</Key>
              </Object>
              <Object>
                <Key>2</Key>
              </Object>
            </Delete>
            """,
      actual: batch.ToXmlString(SaveOptions.None));
    }

    [Fact]
    public void CanSerialize()
    {
        var batch = new DeleteBatch(new[] { "1", "2" });

        Assert.Equal(
            """
            <Delete>
              <Object>
                <Key>1</Key>
              </Object>
              <Object>
                <Key>2</Key>
              </Object>
            </Delete>
            """,
      actual: batch.ToXmlString(SaveOptions.None));
    }


    [Fact]
    public void TestResponse()
    {
        var result = DeleteResult.ParseXml(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <DeleteResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
            	<Deleted>
            		<Key>sample1.txt</Key>
            	</Deleted>
            	<Error>
            		<Key>sample2.txt</Key>
            		<Code>AccessDenied</Code>
            		<Message>Access Denied</Message>
            	</Error>
            </DeleteResult>
            """);

        Assert.Single(result.Deleted);
        Assert.Single(result.Errors);

        Assert.Equal("sample1.txt", result.Deleted[0].Key);
        Assert.Equal("sample2.txt", result.Errors[0].Key);
    }

    [Fact]
    public void TestResponse2()
    {
        var result = DeleteResult.ParseXml(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <DeleteResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
            	<Deleted>
            		<Key>1.txt</Key>
            	</Deleted>
                <Deleted>
            		<Key>2.txt</Key>
            	</Deleted>
            </DeleteResult>
            """);

        Assert.Equal(2, result.Deleted.Length);

        Assert.Equal("1.txt", result.Deleted[0].Key);
        Assert.Equal("2.txt", result.Deleted[1].Key);

        Assert.Null(result.Errors);
    }
}