namespace ImageImporter.Models.Db.ActionItems
{
    public class PictureRemovedItem : ActionItem
    {
        public virtual Picture? Picture { get; set; }
    }
}
