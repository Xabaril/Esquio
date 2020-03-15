using System;

namespace Esquio.UI.Client.Services
{
    public class EsquioState
    {
        public bool ShowingModal { get; set; } = false;

        public event Action OnShowingModalChange;

        public void ShowModal(bool showModal)
        {
            ShowingModal = showModal;

            OnShowingModalChange?.Invoke();
        }
    }
}
