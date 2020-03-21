using Esquio.UI.Api.Shared.Models.Users.My;
using Esquio.UI.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<BreadcrumbItemViewModel> Breadcrumb => state.Breadcrumb;

        public event Action OnWindowModalActiveChange;
        public event Action OnBreadcrumbChange;

        public void SetPolicy(MyResponse my)
        {
            if (my == null) throw new ArgumentNullException(nameof(my));
            state.Policy = policyBuilder.Build(my) as Policy;
            StateHasChanged();
        }

        public void SetWindowModal(bool showModal)
        {
            state.WindowModalIsActive = showModal;
            StateHasChanged();
            OnWindowModalActiveChange?.Invoke();
        }

        public void SetBreadcrumb(params BreadcrumbItemViewModel[] breadcrumb)
        {
            state.Breadcrumb = breadcrumb ?? throw new ArgumentNullException(nameof(breadcrumb));
            StateHasChanged();
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
        public State()
        {
            WindowModalIsActive = false;
            Breadcrumb = Enumerable.Empty<BreadcrumbItemViewModel>();
        }

        public Policy Policy { get; set; }
        public bool WindowModalIsActive { get; set; }
        public IEnumerable<BreadcrumbItemViewModel> Breadcrumb { get; set; }
    }
}
