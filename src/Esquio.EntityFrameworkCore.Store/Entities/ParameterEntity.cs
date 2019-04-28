namespace Esquio.EntityFrameworkCore.Store.Entities
{
    internal class ParameterEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ToggleId { get; set; }
        public ToggleEntity Toggle { get; set; }
    }
}
