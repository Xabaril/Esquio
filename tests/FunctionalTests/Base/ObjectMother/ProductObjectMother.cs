using Esquio.Model;
using System;

namespace FunctionalTests.Base.ObjectMother
{
    static class ProductObjectMother
    {
        public static Product Create()
        {
            return new Product($"name-{Guid.NewGuid()}", "description");
        }
    }
}
