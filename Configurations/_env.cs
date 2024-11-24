namespace Ecommerce_web_api.Configurations
{
    public class _env
    {
        public static string CLOUDINARY_URL => Environment.GetEnvironmentVariable("CLOUDINARY_URL") ?? "";
        public static string APIENDPOINT => Environment.GetEnvironmentVariable("APIENDPOINT") ?? "";
    }
}
