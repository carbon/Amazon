using System.Threading.Tasks;

namespace Amazon
{
    public interface IAwsCredential
    {
        string AccessKeyId { get; }

        string SecretAccessKey { get; }

        string SecurityToken { get; }

        bool ShouldRenew { get; }

        Task<bool> RenewAsync();
    }
}