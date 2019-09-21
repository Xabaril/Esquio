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

    * **DesignType**: *Allow to add a friendly description for the toggle.*
    * **DesignTypeParameter**: *Allow to add a friendly description for the toggle's parameters.*

Once we have define our attributes, it's time to use the services that our toggle will need to check if is active or not. In the constructor we can specify these services::

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

    * **IRuntimeFeatureStore**: We use this service to retrieve the feature. Once we have the feature, we can retrieve the toggle and her data (The parameters and their values).
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

You can see this sample completed and much more in this `repository <https://github.com/Xabaril/Esquio.Contrib>`_ and of course, any PR is welcome.