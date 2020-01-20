using Esquio.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Blazor
{
    public class FeatureComponent : ComponentBase
    {
        private bool _isEnabled;

        private static readonly char[] FeatureSeparator = new[] { ',' };

        /// <summary>
        /// The product name that determines whether the content can be displayed. If you do not specicify a product name,
        /// default configured product will be used.
        /// </summary>
        [Parameter] public string Product { get; set; }

        /// <summary>
        /// A comma delimited list of feature names that are allowed to display the content.
        /// </summary>
        [Parameter] public string Names { get; set; }

        /// <summary>
        /// The content that will be displayed if the features is enabled.
        /// </summary>
        [Parameter] public RenderFragment Enabled { get; set; }

        /// <summary>
        /// The content that will be displayed if the feature is disabled.
        /// </summary>
        [Parameter] public RenderFragment Disabled { get; set; }

        [Inject] private IFeatureService FeatureService { get; set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_isEnabled)
            {
                builder.AddContent(0, Enabled);
            }
            else
            {
                builder.AddContent(0, Disabled);
            }
        }

        /// <inheritdoc />
        protected override async Task OnParametersSetAsync()
        {
            var features = new StringTokenizer(Names, FeatureSeparator);

            foreach (var item in features)
            {
                var featureName = item.Trim();

                if (featureName.HasValue && featureName.Length > 0)
                {
                    _isEnabled = await FeatureService.IsEnabledAsync(featureName.Value);

                    if (!_isEnabled)
                    {
                        return;
                    }
                }
            }
        }
    }
}
