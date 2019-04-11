using System;

namespace Esquio.Abstractions
{
    //TODO: Activator is not a good name here!!!!!!!!
    public interface IToggleActivator
    {
        IToggle ActivateToggle(Type toggle);
    }
}
