namespace Esquio.Model
{
    public class Parameter
    {
        public Parameter(string name, object value)
        {
            Ensure.Argument.NotNullOrEmpty(name, nameof(name));
            Ensure.Argument.NotNull(value, nameof(value));

            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object Value { get; }
    }
}