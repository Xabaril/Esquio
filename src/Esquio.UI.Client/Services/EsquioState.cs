using Esquio.UI.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Client.Services
{
    public class EsquioState
    {
        public bool WindowModalIsActive { get; private set; }

        public IEnumerable<BreadcrumbItemViewModel> Breadcrumb { get; private set; }

        public event Action OnWindowModalActiveChange;
        public event Action OnBreadcrumbChange;

        public EsquioState()
        {
            WindowModalIsActive = false;
            Breadcrumb = Enumerable.Empty<BreadcrumbItemViewModel>();
        }

        public void SetWindowModal(bool showModal)
        {
            WindowModalIsActive = showModal;

            OnWindowModalActiveChange?.Invoke();
        }

        public void SetBreadcrumb(params BreadcrumbItemViewModel[] breadcrumb)
        {
            Breadcrumb = breadcrumb ?? throw new ArgumentNullException(nameof(breadcrumb));

            OnBreadcrumbChange?.Invoke();
        }
    }
}
