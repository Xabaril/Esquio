namespace System
{
    public static class TypeExtensions
    {
        public static string ShorthandAssemblyQualifiedName(this Type type)
            => $"{type.FullName},{type.Assembly.GetName().Name}";
    }
}
