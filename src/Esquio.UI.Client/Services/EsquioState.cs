using System;

namespace Esquio.UI.Client.Services
{
    public class EsquioState
    {
        public bool WindowModalIsActive { get; set; } = false;

        public event Action OnWindowModalActiveChange;

        public void IsApplicationShowingWindowModal(bool showModal)
        {
            WindowModalIsActive = showModal;

            OnWindowModalActiveChange?.Invoke();
        }
    }
}
