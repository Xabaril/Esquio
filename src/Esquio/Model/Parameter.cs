namespace Esquio.Model
{
    public class Parameter
    {
        public Parameter(string name, string value)
        {
            Ensure.Argument.NotNullOrEmpty(name, nameof(name));
            Ensure.Argument.NotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}