using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Esquio.UI.Client.Components
{
    public class ContainerComponentBase : OwningComponentBase<IEsquioClient>
    {
        [Inject]
        protected NavigationManager Navigation { get; set; }

        public IEsquioClient Client => GetService<IEsquioClient>();

        public void NavigateTo(string uri) => Navigation.NavigateTo(uri);

        private TService GetService<TService>()
            => (TService)ScopedServices.GetService(typeof(TService));
    }
}
