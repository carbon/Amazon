using System.Text.Json;

namespace Amazon.Rekognition.Tests;

public class DetectLabelsResultTests
{
    [Fact]
    public void A()
    {
        var result = JsonSerializer.Deserialize<DetectLabelsResult>(
            """
            {
              "ImageProperties": {
                "Background": {
                  "DominantColors": [
                    {
                      "Blue": 79,
                      "CSSColor": "darkslategrey",
                      "Green": 79,
                      "HexCode": "#2f4f4f",
                      "PixelPercent": 39.453125,
                      "Red": 47,
                      "SimplifiedColor": "grey"
                    },
                    {
                      "Blue": 47,
                      "CSSColor": "darkolivegreen",
                      "Green": 107,
                      "HexCode": "#556b2f",
                      "PixelPercent": 28.046875,
                      "Red": 85,
                      "SimplifiedColor": "green"
                    },
                    {
                      "Blue": 128,
                      "CSSColor": "gray",
                      "Green": 128,
                      "HexCode": "#808080",
                      "PixelPercent": 10.3125,
                      "Red": 128,
                      "SimplifiedColor": "grey"
                    }
                  ],
                  "Quality": {
                    "Brightness": 63.69063186645508,
                    "Sharpness": 80.79820251464844
                  }
                },
                "DominantColors": [
                  {
                    "Blue": 79,
                    "CSSColor": "darkslategrey",
                    "Green": 79,
                    "HexCode": "#2f4f4f",
                    "PixelPercent": 25.40567970275879,
                    "Red": 47,
                    "SimplifiedColor": "grey"
                  },
                  {
                    "Blue": 0,
                    "CSSColor": "black",
                    "Green": 0,
                    "HexCode": "#000000",
                    "PixelPercent": 22.312374114990234,
                    "Red": 0,
                    "SimplifiedColor": "black"
                  }
                ],
                "Foreground": {
                  "DominantColors": [
                    {
                      "Blue": 0,
                      "CSSColor": "black",
                      "Green": 0,
                      "HexCode": "#000000",
                      "PixelPercent": 44.173912048339844,
                      "Red": 0,
                      "SimplifiedColor": "black"
                    },
                    {
                      "Blue": 79,
                      "CSSColor": "darkslategrey",
                      "Green": 79,
                      "HexCode": "#2f4f4f",
                      "PixelPercent": 31.826086044311523,
                      "Red": 47,
                      "SimplifiedColor": "grey"
                    }
                  ],
                  "Quality": {
                    "Brightness": 44.656272888183594,
                    "Sharpness": 81.89669036865234
                  }
                },
                "Quality": {
                  "Brightness": 59.91098403930664,
                  "Contrast": 80.38261413574219,
                  "Sharpness": 81.02310943603516
                }
              },
              "LabelModelVersion": "3.0",
              "Labels": [
                {
                  "Aliases": [
                    { "Name": "Apparel" }
                  ],
                  "Categories": [
                    { "Name": "Apparel and Accessories" }
                  ],
                  "Confidence": 99.96941375732422,
                  "Instances": [],
                  "Name": "Clothing",
                  "Parents": []
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Apparel and Accessories" }
                  ],
                  "Confidence": 99.96941375732422,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.1954111009836197,
                        "Left": 0.1740584522485733,
                        "Top": 0.49580082297325134,
                        "Width": 0.3222714960575104
                      },
                      "Confidence": 89.8991470336914,
                      "DominantColors": []
                    }
                  ],
                  "Name": "Skirt",
                  "Parents": [
                    { "Name": "Clothing" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 99.76409912109375,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.865358829498291,
                        "Left": 0.1304430067539215,
                        "Top": 0.05370258912444115,
                        "Width": 0.46623924374580383
                      },
                      "Confidence": 99.76409912109375,
                      "DominantColors": []
                    },
                    {
                      "BoundingBox": {
                        "Height": 0.8222351670265198,
                        "Left": 0.5217385292053223,
                        "Top": 0.07574746012687683,
                        "Width": 0.29830336570739746
                      },
                      "Confidence": 98.37744140625,
                      "DominantColors": []
                    }
                  ],
                  "Name": "Female",
                  "Parents": [
                    {
                      "Name": "Person"
                    }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 99.76409912109375,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.865358829498291,
                        "Left": 0.1304430067539215,
                        "Top": 0.05370258912444115,
                        "Width": 0.46623924374580383
                      },
                      "Confidence": 99.76409912109375,
                      "DominantColors": []
                    }
                  ],
                  "Name": "Girl",
                  "Parents": [
                    { "Name": "Female" },
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [
                    { "Name": "Human" }
                  ],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 99.76409912109375,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.865358829498291,
                        "Left": 0.1304430067539215,
                        "Top": 0.05370258912444115,
                        "Width": 0.46623924374580383
                      },
                      "Confidence": 99.76409912109375,
                      "DominantColors": [
                        {
                          "Blue": 0,
                          "CSSColor": "black",
                          "Green": 0,
                          "HexCode": "#000000",
                          "PixelPercent": 48.56321716308594,
                          "Red": 0,
                          "SimplifiedColor": "black"
                        },
                        {
                          "Blue": 105,
                          "CSSColor": "dimgrey",
                          "Green": 105,
                          "HexCode": "#696969",
                          "PixelPercent": 24.13793182373047,
                          "Red": 105,
                          "SimplifiedColor": "grey"
                        },
                        {
                          "Blue": 79,
                          "CSSColor": "darkslategrey",
                          "Green": 79,
                          "HexCode": "#2f4f4f",
                          "PixelPercent": 14.367815971374512,
                          "Red": 47,
                          "SimplifiedColor": "grey"
                        }
                      ]
                    },
                    {
                      "BoundingBox": {
                        "Height": 0.8222351670265198,
                        "Left": 0.5217385292053223,
                        "Top": 0.07574746012687683,
                        "Width": 0.29830336570739746
                      },
                      "Confidence": 98.37744140625,
                      "DominantColors": [
                        {
                          "Blue": 79,
                          "CSSColor": "darkslategrey",
                          "Green": 79,
                          "HexCode": "#2f4f4f",
                          "PixelPercent": 43.00791549682617,
                          "Red": 47,
                          "SimplifiedColor": "grey"
                        }
                      ]
                    }
                  ],
                  "Name": "Person",
                  "Parents": []
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 99.76409912109375,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.865358829498291,
                        "Left": 0.1304430067539215,
                        "Top": 0.05370258912444115,
                        "Width": 0.46623924374580383
                      },
                      "Confidence": 99.76409912109375,
                      "DominantColors": []
                    }
                  ],
                  "Name": "Teen",
                  "Parents": [
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 98.37744140625,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.8222351670265198,
                        "Left": 0.5217385292053223,
                        "Top": 0.07574746012687683,
                        "Width": 0.29830336570739746
                      },
                      "Confidence": 98.37744140625,
                      "DominantColors": []
                    }
                  ],
                  "Name": "Adult",
                  "Parents": [
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 98.37744140625,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.8222351670265198,
                        "Left": 0.5217385292053223,
                        "Top": 0.07574746012687683,
                        "Width": 0.29830336570739746
                      },
                      "Confidence": 98.37744140625,
                      "DominantColors": []
                    }
                  ],
                  "Name": "Woman",
                  "Parents": [
                    { "Name": "Adult" },
                    { "Name": "Female" },
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 81.58651733398438,
                  "Instances": [],
                  "Name": "Head",
                  "Parents": [
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 79.07342529296875,
                  "Instances": [],
                  "Name": "Face",
                  "Parents": [
                    { "Name": "Head" },
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Beauty and Personal Care" }
                  ],
                  "Confidence": 70.3354721069336,
                  "Instances": [],
                  "Name": "Hair",
                  "Parents": [
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [
                    { "Name": "Plaid" }
                  ],
                  "Categories": [
                    { "Name": "Patterns and Shapes" }
                  ],
                  "Confidence": 65.84466552734375,
                  "Instances": [],
                  "Name": "Tartan",
                  "Parents": []
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Expressions and Emotions" }
                  ],
                  "Confidence": 62.37502670288086,
                  "Instances": [],
                  "Name": "Happy",
                  "Parents": [
                    { "Name": "Face" },
                    { "Name": "Head" },
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    {
                      "Name": "Expressions and Emotions"
                    }
                  ],
                  "Confidence": 62.37502670288086,
                  "Instances": [],
                  "Name": "Smile",
                  "Parents": [
                    { "Name": "Face" },
                    { "Name": "Happy" },
                    { "Name": "Head" },
                    { "Name": "Person" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Person Description" }
                  ],
                  "Confidence": 57.59705352783203,
                  "Instances": [],
                  "Name": "Lady",
                  "Parents": [
                    {
                      "Name": "Person"
                    }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Apparel and Accessories" }
                  ],
                  "Confidence": 56.41181564331055,
                  "Instances": [],
                  "Name": "Footwear",
                  "Parents": [
                    { "Name": "Clothing" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    {
                      "Name": "Apparel and Accessories"
                    }
                  ],
                  "Confidence": 56.41181564331055,
                  "Instances": [],
                  "Name": "Shoe",
                  "Parents": [
                    { "Name": "Clothing" },
                    { "Name": "Footwear" }
                  ]
                },
                {
                  "Aliases": [
                    { "Name": "Photo" }
                  ],
                  "Categories": [
                    { "Name": "Hobbies and Interests" }
                  ],
                  "Confidence": 55.2683219909668,
                  "Instances": [],
                  "Name": "Photography",
                  "Parents": []
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Hobbies and Interests" }
                  ],
                  "Confidence": 55.2683219909668,
                  "Instances": [],
                  "Name": "Portrait",
                  "Parents": [
                    { "Name": "Face" },
                    { "Name": "Head" },
                    { "Name": "Person" },
                    { "Name": "Photography" }
                  ]
                }
              ]
            }
            """);

        var background = result.ImageProperties.Background;

        var dc0 = background.DominantColors[0];

        Assert.Equal((47, 79, 79),    dc0.GetRGB());
        Assert.Equal("#2f4f4f",       dc0.HexCode);
        Assert.Equal("darkslategrey", dc0.CSSColor);
        Assert.Equal(39.453125,       dc0.PixelPercent);
        Assert.Equal("grey",          dc0.SimplifiedColor);
    }

    [Fact]
    public void CanDeserialize()
    {
        var model = JsonSerializer.Deserialize<DetectLabelsResult>(
            """
            {
              "LabelModelVersion": "3.0",
              "Labels": [
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Animals and Pets" }
                  ],
                  "Confidence": 99.20304870605469,
                  "Instances": [],
                  "Name": "Animal",
                  "Parents": []
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Animals and Pets" }
                  ],
                  "Confidence": 99.20304870605469,
                  "Instances": [],
                  "Name": "Mammal",
                  "Parents": [
                    { "Name": "Animal" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    {
                      "Name": "Animals and Pets"
                    }
                  ],
                  "Confidence": 98.50152587890625,
                  "Instances": [
                    {
                      "BoundingBox": {
                        "Height": 0.9008922576904297,
                        "Left": 0.23735451698303223,
                        "Top": 0.05270782485604286,
                        "Width": 0.7433378100395203
                      },
                      "Confidence": 95.6014633178711
                    },
                    {
                      "BoundingBox": {
                        "Height": 0.8849012851715088,
                        "Left": 0.04084322601556778,
                        "Top": 0.028554916381835938,
                        "Width": 0.562747061252594
                      },
                      "Confidence": 93.14213562011719
                    }
                  ],
                  "Name": "Horse",
                  "Parents": [
                    { "Name": "Animal" },
                    { "Name": "Mammal" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Animals and Pets" }
                  ],
                  "Confidence": 98.50152587890625,
                  "Instances": [],
                  "Name": "Stallion",
                  "Parents": [
                    { "Name": "Animal" },
                    { "Name": "Horse" },
                    { "Name": "Mammal" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    {
                      "Name": "Animals and Pets"
                    }
                  ],
                  "Confidence": 96.70740509033203,
                  "Instances": [],
                  "Name": "Andalusian Horse",
                  "Parents": [
                    { "Name": "Animal" },
                    { "Name": "Horse" },
                    { "Name": "Mammal" }
                  ]
                },
                {
                  "Aliases": [],
                  "Categories": [
                    { "Name": "Animals and Pets" }
                  ],
                  "Confidence": 76.61595916748047,
                  "Instances": [],
                  "Name": "Colt Horse",
                  "Parents": [
                    { "Name": "Animal" },
                    { "Name": "Horse" },
                    { "Name": "Mammal" }
                  ]
                }
              ]
            }
            """);


        Assert.Equal("3.0", model.LabelModelVersion);
        Assert.Empty(model.Labels[0].Aliases);
        Assert.Empty(model.Labels[0].Instances);
        Assert.Empty(model.Labels[0].Parents);

        var last = model.Labels[^1];

        Assert.Equal("Animals and Pets", last.Categories[0].Name);

        Assert.Equal(3, last.Parents.Length);
        Assert.Equal("Animal", last.Parents[0].Name);
        Assert.Equal("Horse", last.Parents[1].Name);   
    }
}