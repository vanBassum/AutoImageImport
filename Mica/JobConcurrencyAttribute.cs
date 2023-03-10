namespace Mica
{
    public class JobConcurrencyAttribute : Attribute
    {
        public bool AllowConcurrent { get; set; }
        public JobConcurrencyAttribute(bool allowConcurrent)
        {
            AllowConcurrent = allowConcurrent;
        }
    }
}
