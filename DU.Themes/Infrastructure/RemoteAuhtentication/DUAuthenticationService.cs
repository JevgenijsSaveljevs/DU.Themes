using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace DU.Themes.Infrastructure.RemoteAuhtentication
{
    public class DUAuthenticationService : IAuthenticationService
    {
        private readonly RemoteCertificateValidationCallback remoteCertificateValidationCallback;
        private string authenticationUrl;

        public DUAuthenticationService(string authenticationUrl)
        {
            if (string.IsNullOrEmpty(authenticationUrl))
            {
                throw new ArgumentException("authenticationUrl");
            }

            this.authenticationUrl = authenticationUrl;
            this.remoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public bool IsAuthenticated(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode;
        }

        public async Task<HttpResponseMessage> PostCredentials(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("login");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password");
            }

            var content = new FormUrlEncodedContent(new[]
                        {
                new KeyValuePair<string, string>("student_name", login),
                new KeyValuePair<string, string>("student_password", password)
            });

            using (var client = new HttpClient())
            {
                return await client.PostAsync(this.authenticationUrl, content);
            }
        }

        public void ConfigureCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = this.remoteCertificateValidationCallback;
        }
    }
}
