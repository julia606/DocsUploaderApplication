namespace DocsUploaderApplication.Models
{
    public class BlobTriggerDataModel
    {
        public byte[] BlobStream { get; set; }
        public string BlobName { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
