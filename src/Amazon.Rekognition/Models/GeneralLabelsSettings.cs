namespace Amazon.Rekognition;

public sealed class GeneralLabelsSettings
{
    public string[]? LabelCategoryExclusionFilters { get; set; }

    public string[]? LabelCategoryInclusionFilters { get; set; }

    public string[]? LabelExclusionFilters { get; set; }

    public string[]? LabelInclusionFilters { get; set; }
}