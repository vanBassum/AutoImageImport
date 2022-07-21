namespace ImageImporter.Models.Db
{
    public class PictureImportResult : BaseImportResult
    {
        public virtual Picture ImportedImage { get; set; }

    }

}
