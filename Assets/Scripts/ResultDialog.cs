using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class ResultDialog : BehaviourBase, IDialog
    {
        private Subject<Unit> goNextLevelSubject = new Subject<Unit> ();
        private Subject<Unit> onToggleActiveSubject = new Subject<Unit> ();
        public IObservable<Unit> GoNextLevelObservable => goNextLevelSubject;
        public IObservable<Unit> OnToggleActiveObservable => onToggleActiveSubject;
        public bool IsOpen => this.gameObject.activeSelf;
        public override void Initialize ()
        {

        }

        public void Open ()
        {
            this.gameObject.SetActive (true);
            Initialize ();
        }

        public void Close ()
        {
            if (!this.gameObject.activeSelf) return;
            this.gameObject.SetActive (false);
        }

        private void OnEnable ()
        {
            onToggleActiveSubject.OnNext (Unit.Default);
        }

        private void OnDisable ()
        {
            onToggleActiveSubject.OnNext (Unit.Default);
        }

        public void GoNext ()
        {
            this.goNextLevelSubject.OnNext (Unit.Default);
            Close ();
        }
    }
}