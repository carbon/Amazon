using System;

namespace Amazon.CodeBuild
{
    public class StopBuildRequest : ICodeBuildRequest
    {
        public StopBuildRequest(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }
    }
}