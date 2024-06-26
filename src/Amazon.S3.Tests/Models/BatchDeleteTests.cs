﻿using System.Xml.Linq;

namespace Amazon.S3.Models.Tests;

public class BatchDeleteTests
{
    [Fact]
    public void CanSerialize_Silent()
    {
        var batch = new DeleteBatch([ "1", "2" ], quite: true);

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
      actual: batch.ToXmlString(true), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void CanSerialize()
    {
        var batch = new DeleteBatch([ "1", "2" ]);

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
            """, batch.ToXmlString(true), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void CanSerializeWithoutFormatting()
    {
        var batch = new DeleteBatch(["1"]);

        Assert.Equal("<Delete><Object><Key>1</Key></Object></Delete>", batch.ToXmlString());
    }
}
