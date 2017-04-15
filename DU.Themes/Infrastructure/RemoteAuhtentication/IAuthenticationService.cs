using System.Net.Http;
using System.Threading.Tasks;

namespace DU.Themes.Infrastructure.RemoteAuhtentication
{
    public interface IAuthenticationService
    {
        void ConfigureCertificateValidation();
        bool IsAuthenticated(HttpResponseMessage response);
        Task<HttpResponseMessage> PostCredentials(string login, string password);
    }
}