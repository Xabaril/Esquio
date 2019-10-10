namespace System
{
    public static class TypeExtensions
    {
        public static string ShorthandAssemblyQualifiedName(this Type type)
        {
            var segments = type.AssemblyQualifiedName
                .Split(',');

            return $"{segments[0]},{segments[1]}";
        }
    }
}
