Extensibility
=============

Esquio provides lot of toggles Out-of-the-box but sometimes is not enough. Therefore extensibility is a key quality attribute of any experience delivery architecture. Esquio does not aim to solve every problem but rather provides an extensible parts that enables you to adapt it to your needs.

All major components have interfaces which are extensible. Following the inversion of control design they are substitutable either through **Microsoft.Extensions.DependencyInjection** configuration.

Creating your custom toggle
^^^^^^^^^^^^^^^^^^^^^^^^^^^

Imagine that you need to create a toggle to enable features based on the user's browser (`User-Agent <https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/User-Agent>`_ header).

First step is implement the interface ``IToggle``::

    public class UserAgentBrowserToggle : IToggle
    {
        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {

        }
    }

``IsActiveAsync`` returns a boolean if the feature X for the product Y is enabled or not based on this toggle. In our case, depending on the user's browser.

To be able to specify for which browsers will be this feature enabled, you need to add a property::

    public class UserAgentBrowserToggle : IToggle
    {
        public string Browsers { get; set; }

        //code omited for brevity
    }

If you want to an application (`Esquio UI <https://github.com/Xabaril/Esquio/tree/master/src/Esquio.UI>`_) be able to understand these fields and for example built an user interface in order to provide to the users a more friendly way to configure the toggles of a feature, you need to decorate the toggle with some attributes::

    [DesignType(Description = "Toggle that is active depending on request browser information.")]
    [DesignTypeParameter(ParameterName = Browsers, ParameterType = "System.String", ParameterDescription = "Collection of browser names delimited by ';' character.")]
    public class UserAgentBrowserToggle : IToggle
    {
        public string Browsers { get; set; }

        //code omited for brevity
    }

**Attributes**

    * DesignType: *Allow to add a friendly description for the toggle.*
    * DesignTypeParameter: *Allow to add a friendly description for the toggle's parameters.*

