using System.Threading.Tasks;

namespace Amazon
{
    public interface IAwsCredentials
    {
        string AccessKeyId { get; }

        string SecretAccessKey { get; }

        string SecurityToken { get; }

        bool ShouldRenew { get; }

        Task<IAwsCredentials> RenewAsync();
    }
}
