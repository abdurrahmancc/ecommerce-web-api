namespace Ecommerce_web_api.Configurations
{
    public class _env
    {
        public static string CLOUDINARY_URL => Environment.GetEnvironmentVariable("CLOUDINARY_URL") ?? "";
        public static string APIENDPOINT => Environment.GetEnvironmentVariable("APIENDPOINT") ?? "";
        public static string JWT_SECRET_KEY => Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "";
        public static string JWT_ISSUER => Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "";
        public static string JWT_AUDIENCE => Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "";
        public static int JWT_EXPIRY_MINUTES {
            get
            {
                var expiry = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES");
                return int.TryParse(expiry, out var result) ? result : 60; // Default to 60 if parsing fails
            }
        }
    }
}
