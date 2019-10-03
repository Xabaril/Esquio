namespace Esquio.Abstractions
{
    internal class DefaultValuePartitioner
        : IValuePartitioner
    {
        public short ResolvePartition(string value, short partitions = 100)
        {
            return JenkinsHashFunction.ResolveToLogicalPartition(value, partitions);
        }
    }
}
