namespace Amazon.S3.Models.Tests;

public class DeleteResultTests
{
    [Fact]
    public void CanDeserializeError()
    {
        var result = S3Serializer<DeleteResult>.Deserialize(
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
            """u8.ToArray());
        
        Assert.NotNull(result.Deleted);
        Assert.NotNull(result.Errors);
        
        Assert.Single(result.Deleted);
        Assert.Single(result.Errors);
        
        Assert.Equal("sample1.txt", result.Deleted[0].Key);
        Assert.Equal("sample2.txt", result.Errors[0].Key);
        
        Assert.True(result.HasErrors);
    }

    [Fact]
    public void CanDeserialize()
    {
        var result = S3Serializer<DeleteResult>.Deserialize(
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
            """u8.ToArray());

        Assert.Equal(2, result.Deleted.Length);

        Assert.Equal("1.txt", result.Deleted[0].Key);
        Assert.Equal("2.txt", result.Deleted[1].Key);

        Assert.Null(result.Errors);
    }
}