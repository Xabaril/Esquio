Extensibility
=============

Esquio provides lots of toggles out-of-the-box, but sometimes that is not enough. Here, extensibility becomes a key quality attribute. If Esquio does not solve your problem, it can provide you an extensible part that would enable you to adapt it to your needs.

All main components have interfaces which are extensible. Using the inversion of control design, they can be substituted on **Microsoft.Extensions.DependencyInjection** configuration. 

Creating your custom toggle
^^^^^^^^^^^^^^^^^^^^^^^^^^^

Supose that you need to create a toggle to enable features based on the user's browser (`User-Agent <https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/User-Agent>`_ header).

First, create a new toggle and implement the interface ``IToggle``::

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

If you want an application (`Esquio UI <https://github.com/Xabaril/Esquio/tree/master/src/Esquio.UI>`_) to be able to understand these fields (and for example built an user interface to provide users a more friendly way to configure the toggles of a feature), you need to decorate the toggle with some attributes::

    [DesignType(Description = "Toggle that is active depending on request browser information.")]
    [DesignTypeParameter(ParameterName = Browsers, ParameterType = "System.String", ParameterDescription = "Collection of browser names delimited by ';' character.")]
    public class UserAgentBrowserToggle : IToggle
    {
        public string Browsers { get; set; }

        //code omited for brevity
    }

**Attributes**

    * **DesignType**: *Allow to add a friendly description for the toggle.*
    * **DesignTypeParameter**: *Allow to add a friendly description for the toggle's parameters.*

Once we have defined our attributes, it's time to use the services that our toggle needs to determinate if it is active or not. We can specify these services in the constructor ::

    public UserAgentBrowserToggle(
        IRuntimeFeatureStore featureStore,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserAgentBrowserToggle> logger)
    {
        _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        _contextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

**Services**

    * **IRuntimeFeatureStore**: We use this service to retrieve the feature. Once we have the feature, we can retrieve the toggle and its data (The parameters and their values).
    * **IHttpContextAccessor**: To access the HttpContext.
    * **ILogger<T>**: To log whatever you want.

It's time to finish our feature. We need to complete the ``IsActiveAsync`` method with the code below::

    public async Task<bool> IsActiveAsync(
        string featureName,
        string productName = null,
        CancellationToken cancellationToken = default)
    {
        var feature = await _featureStore.FindFeatureAsync(featureName, productName, cancellationToken);
        var toggle = feature.GetToggle(typeof(UserAgentBrowserToggle).FullName);
        var data = toggle.GetData();

        var allowedBrowsers = data.Browsers.ToString();
        var currentBrowser = GetCurrentBrowser();

        if (allowedBrowsers != null && !String.IsNullOrEmpty(currentBrowser))
        {
            _logger.LogDebug("{nameof(UserAgentBrowserToggle)} is trying to verify if {currentBrowser} is satisfying allowed browser configuration.");

            var tokenizer = new StringTokenizer(allowedBrowsers, split_characters);

            foreach (var segment in tokenizer)
            {
                if (segment.Value?.IndexOf(currentBrowser, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _logger.LogInformation("The browser {currentBrowser} is satisfied using {allowedBrowsers} configuration.");

                    return true;
                }
            }
        }

        _logger.LogInformation("The browser {currentBrowser} is not allowed using current toggle configuration.");

        return false;
    }

    private string GetCurrentBrowser()
    {
        return _contextAccessor.HttpContext
            .Request
            .Headers[UserAgent]
            .FirstOrDefault() ?? string.Empty;
    }

Finally, we can register our custom toggle using the method ``RegisterTogglesFromAssemblyContaining`` in our Startup class::

    services.AddEsquio(setup => setup.RegisterTogglesFromAssemblyContaining<Startup>())

As you can see, Esquio provides a flexible way to customize as you need.

You can see this full sample and much more in this `repository <https://github.com/Xabaril/Esquio.Contrib>`_ and of course, any PR is welcomed ;)