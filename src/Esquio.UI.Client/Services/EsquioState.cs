using Esquio.UI.Client.ViewModels;
using System;
using System.Collections.Generic;

namespace Esquio.UI.Client.Services
{
    public class EsquioState
    {
        private const string LOCAL_STORAGE_STATE_KEY = "state";
        private readonly IPolicyBuilder policyBuilder;
        private readonly ILocalStorage storage;
        private State state;

        public EsquioState(IPolicyBuilder policyBuilder, ILocalStorage storage)
        {
            this.policyBuilder = policyBuilder;
            this.storage = storage;
            this.state = new State();
        }

        public IPolicy Policy => state.Policy;
        public bool WindowModalIsActive => state.WindowModalIsActive;
        public IEnumerable<BreadcrumbItemViewModel> Breadcrumb { get; private set; } = Array.Empty<BreadcrumbItemViewModel>();

        public event Action OnWindowModalActiveChange;
        public event Action OnBreadcrumbChange;

        public void IsApplicationShowingWindowModal(bool showModal)
        {
            state.WindowModalIsActive = showModal;
            StateHasChanged();
            OnWindowModalActiveChange?.Invoke();
        }

        public void SetPolicy(MyResponse my)
        {
            if (my == null) throw new ArgumentNullException(nameof(my));
            state.Policy = policyBuilder.Build(my) as Policy;
            StateHasChanged();
        }

        public void SetBreadcrumb(params BreadcrumbItemViewModel[] breadcrumb)
        {
            Breadcrumb = breadcrumb ?? throw new ArgumentNullException(nameof(breadcrumb));
            OnBreadcrumbChange?.Invoke();
        }

        public void Hydrate()
        {
            var state = storage.GetItem<State>(LOCAL_STORAGE_STATE_KEY);
            if (state != null)
            {
                this.state = state;
            }
        }

        private void StateHasChanged()
        {
            storage.SetItem(LOCAL_STORAGE_STATE_KEY, state);
        }
    }

    public class State
    {
        public Policy Policy { get; set; }
        public bool WindowModalIsActive { get; set; }
    }
}
