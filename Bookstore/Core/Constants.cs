namespace Core
{
    public static class Constants
    {
        public const int MaxAuthorSize = 128;
        public const int MaxTitleSize = 128;
        public const int MaxDescriptionSize = 2000;
        public const int MaxImageSize = 10485760; // 10MB
        public static readonly string[] AllowedImageContentTypes = new[] { "image/jpg", "image/png", "image/jpeg" };
    }
}
