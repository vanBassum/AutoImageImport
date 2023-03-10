namespace MediaImporter.Application.Hashing
{
    public interface IHasher
    {
        public string Hash(FileInfo file);
    }
}
