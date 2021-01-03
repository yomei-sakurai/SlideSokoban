using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class OverlayUI : BehaviourBase
    {
        [SerializeField] private GameObject modal = null;
        [SerializeField] private ResultDialog result = null;
        private List<IDialog> dialogList = new List<IDialog> ();
        public IObservable<Unit> GoNextLevelObservable => this.result.GoNextLevelObservable;
        private bool IsDialogOpen => this.dialogList.Any (d => d.IsOpen);
        public override void Initialize ()
        {
            CloseAll ();
            this.dialogList.Clear ();
            this.dialogList.Add (result);
            this.result.OnToggleActiveObservable.Subscribe (unit => UpdateModal ());
        }

        public void ShowResult ()
        {
            this.result.Open ();
        }

        private void UpdateModal ()
        {
            this.modal.SetActive (IsDialogOpen);
        }

        private void CloseAll ()
        {
            this.result.Close ();
        }
    }
}