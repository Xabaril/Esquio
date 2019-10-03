namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent a value partitioner used to set value in buckets. Typically this partitioner is used by gradual rollout
    /// toggles in order to create determinist partitions.
    /// </summary>
    public interface IValuePartitioner
    {
        /// <summary>
        /// Assign the logical partition for <paramref name="value"/> on <paramref name="partitions"/> buckets.
        /// </summary>
        /// <param name="value">The value to be assigned on partition.</param>
        /// <param name="entityPartitionCount">The number of partitions to use.</param>
        /// <returns>The resoved bucket for specified value.</returns>
        short ResolvePartition(string value, short partitions = 100);
    }
}
