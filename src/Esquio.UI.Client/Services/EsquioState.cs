using Esquio.UI.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Client.Services
{
    public class EsquioState
    {
        public bool WindowModalIsActive { get; set; } = false;
        public IEnumerable<BreadcrumbItemViewModel> Breadcrumb { get; private set; } = Enumerable.Empty<BreadcrumbItemViewModel>();

        public event Action OnWindowModalActiveChange;
        public event Action OnBreadcrumbChange;

        public void IsApplicationShowingWindowModal(bool showModal)
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
