using System;

namespace Amazon.CodeBuild
{
    public sealed class StopBuildRequest : ICodeBuildRequest
    {
        public StopBuildRequest(string id)
        {
            ArgumentNullException.ThrowIfNull(id);

            Id = id;
        }

        public string Id { get; }
    }
}