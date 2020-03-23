using Esquio.UI.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Client.Services
{
    public class EsquioState
    {
        private const string LOCAL_STORAGE_STATE_KEY = "esquio-persisted-state";

        private readonly IPolicyBuilder _policyBuilder;
        private readonly ILocalStorage _storage;

        private LocalContext _localContext = new LocalContext();

        public event Action OnWindowModalActiveChange;
        public event Action OnBreadcrumbChange;

        public Policy Policy
        {
            get
            {
                return _localContext.Policy;
            }
        }

        public bool WindowModalIsActive
        {
            get
            {
                return _localContext.WindowModalIsActive;
            }
        }

        public IEnumerable<BreadcrumbItemViewModel> Breadcrumb
        {
            get
            {
                return _localContext.Breadcrumb;
            }
        }

        public EsquioState(IPolicyBuilder policyBuilder, ILocalStorage storage)
        {
            this._policyBuilder = policyBuilder;
            this._storage = storage;
        }

        public void SetPolicy(Policy policy)
        {
            _ = policy ?? throw new ArgumentNullException(nameof(policy));

            _localContext.Policy = policy;

            PersistState();
        }

        public void SetWindowModal(bool showModal)
        {
            _localContext.WindowModalIsActive = showModal;
            OnWindowModalActiveChange?.Invoke();

            PersistState();
        }

        public void SetBreadcrumb(params BreadcrumbItemViewModel[] breadcrumb)
        {
            _localContext.Breadcrumb = breadcrumb ?? throw new ArgumentNullException(nameof(breadcrumb));
            OnBreadcrumbChange?.Invoke();

            PersistState();
        }

        public void ReloadState()
        {
            var context = _storage.GetItem<LocalContext>(LOCAL_STORAGE_STATE_KEY);

            if (context != null)
            {
                this._localContext = context;
            }
        }

        public void PersistState()
        {
            _storage.SetItem(LOCAL_STORAGE_STATE_KEY, _localContext);
        }

        public void ClearState()
        {
            _storage.RemoveItem(LOCAL_STORAGE_STATE_KEY);
        }

        private class LocalContext
        {
            public LocalContext()
            {
                WindowModalIsActive = false;
                Breadcrumb = Enumerable.Empty<BreadcrumbItemViewModel>();
            }

            public Policy Policy { get; set; }

            public bool WindowModalIsActive { get; set; }

            public IEnumerable<BreadcrumbItemViewModel> Breadcrumb { get; set; }
        }
    }
}
