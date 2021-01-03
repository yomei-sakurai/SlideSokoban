using System;
using UniRx;

namespace Assets.Scripts
{
    public interface IDialog
    {
        void Open ();
        void Close ();
        IObservable<Unit> OnToggleActiveObservable { get; }
        bool IsOpen { get; }
    }
}