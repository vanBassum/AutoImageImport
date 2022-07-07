namespace ImageImporter.Helpers
{
    public static class ReflectionExtentions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this Type t, bool inherit)
        {
            foreach (var type in t.GetCustomAttributes(false))
            {
                if (type is T a)
                    yield return a;
            }
        }
    }
}
