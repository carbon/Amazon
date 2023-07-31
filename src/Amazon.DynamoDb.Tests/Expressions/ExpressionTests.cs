using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Expressions.Tests;

using static Expression;

public class ExpressionTests
{
    [Fact]
    public void Expression1()
    {
        var expression = And(Eq("category", "shoes"), Gt("price", 5));

        var de = DynamoExpression.FromExpression(expression);

        Assert.Equal("category = :v0 and price > :v1", de.Text);

        Assert.Equal("shoes", de.AttributeValues[":v0"].Value);
        Assert.Equal(5, de.AttributeValues[":v1"].Value);
    }

    [Fact]
    public void Conjuction1()
    {
        var expression = Conjunction(Eq("category", "shoes"), Gt("price", 5), IsNull("deleted"));

        var de = DynamoExpression.FromExpression(expression);

        Assert.Equal("category = :v0 and price > :v1 and attribute_not_exists(deleted)", de.Text);

        Assert.Equal("shoes", de.AttributeValues[":v0"].Value);
        Assert.Equal(5, de.AttributeValues[":v1"].Value);
    }

    [Fact]
    public void Function1()
    {
        var de = DynamoExpression.Conjunction(
            Func("attribute_exists", new Symbol("name")),
            Gt("price", 5)
        );

        Assert.Equal("attribute_exists(#name) and price > :v0", de.Text);
        Assert.Equal("name", de.AttributeNames["#name"].ToString());
    }

    [Fact]
    public void Function2()
    {
        var de = DynamoExpression.Conjunction(
            Func("exists", new Symbol("category")),
            Lt("price", 25)
        );

        Assert.Equal("attribute_exists(category) and price < :v0", de.Text);

        Assert.Equal(25, de.AttributeValues[":v0"].Value);
    }

    [Fact]
    public void Function3()
    {
        var de = DynamoExpression.Conjunction(
            Exists("category"),
            NotExists("soldOut"),
            Lt("price", 25)
        );

        Assert.Equal("attribute_exists(category) and attribute_not_exists(soldOut) and price < :v0", de.Text);

        Assert.Equal(25, de.AttributeValues[":v0"].Value);
    }

    [Fact]
    public void Expression2()
    {
        var expression = And(Eq("category", "shoes"), NotEq("price", 5));

        var de = DynamoExpression.FromExpression(expression);

        Assert.Equal("category = :v0 and price <> :v1", de.Text);

        Assert.Equal("shoes", de.AttributeValues[":v0"].Value);
        Assert.Equal(5, de.AttributeValues[":v1"].Value);
    }

    [Fact]
    public void Expression3()
    {
        var expression = And(Eq("category", "shoes"), Between("price", Constant(5), Constant(10)));

        var de = DynamoExpression.FromExpression(expression);

        Assert.Equal("category = :v0 and price between :v1 and :v2", de.Text);

        Assert.Equal("shoes", de.AttributeValues[":v0"].Value);
        Assert.Equal(5, de.AttributeValues[":v1"].Value);
        Assert.Equal(10, de.AttributeValues[":v2"].Value);
    }

    [Fact]
    public void Expression5()
    {
        var expression = And(Eq("name", "banana"), Eq("active", 1));

        var de = DynamoExpression.FromExpression(expression);

        Assert.Equal("#name = :v0 and active = :v1", de.Text);

        Assert.Equal("banana", de.AttributeValues[":v0"].Value);
        Assert.Equal(1, de.AttributeValues[":v1"].Value);
        Assert.Equal("name", de.AttributeNames["#name"].ToString());
    }

    [Fact]
    public void Expression6()
    {
        var de = DynamoExpression.Conjunction(Eq("name", "banana"), Eq("active", 1));

        Assert.Equal("#name = :v0 and active = :v1", de.Text);

        Assert.Equal("banana", de.AttributeValues[":v0"].Value);
        Assert.Equal(1, de.AttributeValues[":v1"].Value);
        Assert.Equal("name", de.AttributeNames["#name"].ToString());
    }

    [Fact]
    public void Expression9()
    {
        var de = new DynamoQueryExpression(
            [ "name", "version" ],
            new Expression[] {
                Eq("name", "sortable"),
                Between("version", "1.0", "2.0")
            });

        Assert.Equal("#name = :v0 and version between :v1 and :v2", de.KeyExpression.Text);

        Assert.Equal("1.0", de.AttributeValues[":v1"].Value);
        Assert.Equal("2.0", de.AttributeValues[":v2"].Value);
    }
}