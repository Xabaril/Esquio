using System;
using System.Threading.Tasks;

namespace Esquio.UI.Client.Services
{
    public class ConfirmState
    {
        private readonly EsquioState _esquioState;
        private string _confirmModalTitle;
        private string _confirmModalDescription;
        private bool _confirmModalIsActive;
        private Guid _actionId;

        public event Action OnConfirmModalActiveChange;
        public event Func<Guid, ValueTask> OnConfirm;
        public event Action OnCancel;

        public ConfirmState(EsquioState esquioState)
        {
            _esquioState = esquioState;
        }

        public string ConfirmModalTitle
        {
            get
            {
                return _confirmModalTitle;
            }
        }

        public string ConfirmModalDescription
        {
            get
            {
                return _confirmModalDescription;
            }
        }

        public bool ConfirmModalIsActive
        {
            get
            {
                return _confirmModalIsActive;
            }
        }

        public void ShowConfirmModal(string title, string description, Guid actionId)
        {
            _confirmModalTitle = title;
            _confirmModalDescription = description;
            _actionId = actionId;
            _confirmModalIsActive = true;
            _esquioState.SetWindowModal(true);
            OnConfirmModalActiveChange?.Invoke();
        }

        public void HideConfirmModal()
        {
            _confirmModalTitle = default;
            _confirmModalDescription = default;
            _actionId = default;
            _confirmModalIsActive = false;
            _esquioState.SetWindowModal(false);
            OnConfirmModalActiveChange?.Invoke();
        }

        public void Cancel()
        {
            OnCancel?.Invoke();
            HideConfirmModal();
        }

        public void Confirm()
        {
            OnConfirm?.Invoke(_actionId);
            HideConfirmModal();
        }
    }
}
