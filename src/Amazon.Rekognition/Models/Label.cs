namespace Amazon.Rekognition;

public class Label
{
    public LabelAlias[] Aliases { get; set; }

    public LabelCategory[] Categories { get; set; }

    public Instance[] Instances { get; set; }

    public double Confidence { get; set; }

    public string Name { get; set; }

    public Parent[] Parents { get; set; }
}