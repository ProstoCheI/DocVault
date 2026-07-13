using System.ComponentModel;

namespace DocVaultLocal.Models
{
    public class Document
    {
        [Browsable(false)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Tags { get; set; }

        [Browsable(false)]
        public string FileType { get; set; }

        [Browsable(false)]
        public string FilePath { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastModified { get; set; }
    }
}