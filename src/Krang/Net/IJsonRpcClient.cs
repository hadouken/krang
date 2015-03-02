using System.Threading.Tasks;

namespace Krang.Net
{
    public interface IJsonRpcClient
    {
        Task InvokeAsync(string method, params object[] parameters);

        Task<T> InvokeAsync<T>(string method, params object[] parameters);
    }
}
