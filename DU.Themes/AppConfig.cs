using System.Configuration;

namespace DU.Themes
{
    public static class AppConfig
    {
        /// <summary>
        /// Configures components behaviour
        /// </summary>
        public static bool IsDevelopment => bool.Parse(ConfigurationManager.AppSettings["IsDevelopment"]);

        /// <summary>
        /// Gets Authentication Url
        /// </summary>
        public static string AuthenticationUrl => ConfigurationManager.AppSettings["AuthenticationUrl"];
    }
}