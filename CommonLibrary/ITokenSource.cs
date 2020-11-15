using System.Threading.Tasks;

namespace CommonLibrary
{
    public interface ITokenSource
    {
        Task<string> GetTokenAsync(string email);
    }
}

