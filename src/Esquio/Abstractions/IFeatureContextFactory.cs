namespace Esquio.Abstractions
{
    public interface IFeatureContextFactory
    {
        IFeatureContext Create(string applicationName, string featureName);
    }
}
