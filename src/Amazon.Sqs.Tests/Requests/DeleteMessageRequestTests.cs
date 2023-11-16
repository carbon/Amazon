using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class DeleteMessageRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new DeleteMessageRequest(
            "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
            "AQEB3LQoW7GQWgodQCEJXHjMvO/QkeCHiRldRfLC/E6RUggm+BjpthqxfoUOUn6Vs271qmrBaufFqEmnMKgk2n1EuUBne1pe+hZcrDE8IveUUPmqkUT54FGhAAjPX3oEIryz/XeQ/muKAuLclcZvt2Q+ZDPW8DvZqMa1RoHxOqSq+6kQ4PwgQxB+VqDYvIc/LpHOoL4PTROBXgLPjWrzz/knK6HTzKpqC4ESvFdJ/dkk2nvS0iqYOly5VQknK/lv/rTUOgEYevjJSrNLIPDgZGyvgcLwbm6+yo1cW/c9cPDiVm96gIhVkuiCZ1gtskoOtyroZVPcY71clDG2EPZJeY8akMd3u+sXEMWhiOPFs1cgWQs2ugsL+vdwMCbsZRkXbJv7"
        );

        Assert.Equal(
            """
            {
              "QueueUrl": "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/",
              "ReceiptHandle": "AQEB3LQoW7GQWgodQCEJXHjMvO/QkeCHiRldRfLC/E6RUggm+BjpthqxfoUOUn6Vs271qmrBaufFqEmnMKgk2n1EuUBne1pe+hZcrDE8IveUUPmqkUT54FGhAAjPX3oEIryz/XeQ/muKAuLclcZvt2Q+ZDPW8DvZqMa1RoHxOqSq+6kQ4PwgQxB+VqDYvIc/LpHOoL4PTROBXgLPjWrzz/knK6HTzKpqC4ESvFdJ/dkk2nvS0iqYOly5VQknK/lv/rTUOgEYevjJSrNLIPDgZGyvgcLwbm6+yo1cW/c9cPDiVm96gIhVkuiCZ1gtskoOtyroZVPcY71clDG2EPZJeY8akMd3u+sXEMWhiOPFs1cgWQs2ugsL+vdwMCbsZRkXbJv7"
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.DeleteMessageRequest));
    }
}
