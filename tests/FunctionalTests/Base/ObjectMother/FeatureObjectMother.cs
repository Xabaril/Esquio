using Esquio.Model;

namespace FunctionalTests.Base.ObjectMother
{
    static class FeatureObjectMother
    {
        public static Feature Create()
        {
            return new Feature("name", "description");
        }
    }
}
