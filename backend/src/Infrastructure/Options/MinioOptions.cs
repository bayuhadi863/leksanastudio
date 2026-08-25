namespace LeksanaStudio.Infrastructure.Options
{
    public class MinioOptions
    {
        public const string SectionName = "Minio";

        public string Endpoint { get; set; } = string.Empty;
        public int Port { get; set; } = 9000;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BucketName { get; set; } = "p3m-pens";
        public bool UseSSL { get; set; } = false;
    }
}
