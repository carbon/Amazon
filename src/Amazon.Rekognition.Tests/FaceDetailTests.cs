using System.Text.Json;

namespace Amazon.Rekognition.Tests;

public class FaceDetailTests
{
    [Fact]
    public void CanDeserialize()
    {
        var model = JsonSerializer.Deserialize<FaceDetail>(
            """
            {
              "BoundingBox": {
                "Height": 0.18000000715255737,
                "Left": 0.5555555820465088,
                "Top": 0.33666667342185974,
                "Width": 0.23999999463558197
              },
              "Confidence": 100,
              "Landmarks": [
                {
                  "Type": "eyeLeft",
                  "X": 0.6394737362861633,
                  "Y": 0.40819624066352844
                },
                {
                  "Type": "eyeRight",
                  "X": 0.7266660928726196,
                  "Y": 0.41039225459098816
                },
                {
                  "Type": "eyeRight",
                  "X": 0.6912462115287781,
                  "Y": 0.44240960478782654
                },
                {
                  "Type": "mouthDown",
                  "X": 0.6306198239326477,
                  "Y": 0.46700039505958557
                },
                {
                  "Type": "mouthUp",
                  "X": 0.7215608954429626,
                  "Y": 0.47114261984825134
                }
              ],
              "Pose": {
                "Pitch": 4.050806522369385,
                "Roll": 0.9950747489929199,
                "Yaw": 13.693790435791016
              },
              "Quality": {
                "Brightness": 37.60169982910156,
                "Sharpness": 80
              }
            }
            """)!;


        Assert.Equal(5, model.Landmarks.Count);
        Assert.Equal("eyeLeft", model.Landmarks[0].Type);

        Assert.Equal(0.18000000715255737, model.BoundingBox.Height);
        Assert.Equal(0.23999999463558197, model.BoundingBox.Width);
        Assert.Equal(0.5555555820465088,  model.BoundingBox.Left);

        Assert.Equal(4.050806522369385, model.Pose.Pitch);

        Assert.Equal(37.601699829101562, model.Quality.Brightness);
    }
}