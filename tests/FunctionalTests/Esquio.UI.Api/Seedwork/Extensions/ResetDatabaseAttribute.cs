using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Extensions
{
    public class ResetDatabaseAttribute
        : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            ServerFixture.ResetDatabase();
        }
    }
}
