using System;

namespace Esquio.Abstractions
{
    public interface IToggleActivator
    {
        IToggle ActivateToggle(Type toggle);
    }
}
