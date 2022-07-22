namespace ImageImporter.Helpers
{
    public static class Helpers
    {
        public static String HumanSize(Int64 bytes)
        {
            const String prefixes = " KMGTPEY";
            for (var i = 0; i < prefixes.Length; i++)
                if (bytes < 1024U << (i * 10))
                    return $"{(bytes >> (10 * i - 10)) / 1024:0.###} {prefixes[i]}B";

            throw new ArgumentOutOfRangeException(nameof(bytes));
        }
    }
}
