using System;
using Assets.Scripts;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class GameUI : BehaviourBase
    {
        [SerializeField] private Button menuButton = null;
        [SerializeField] private Button undoButton = null;
        [SerializeField] private Button retryButton = null;
        [SerializeField] private Text levelText = null;
        private Subject<Unit> onClickMenuSubject = new Subject<Unit> ();
        private Subject<Unit> onClickRetrySubject = new Subject<Unit> ();
        public IObservable<Unit> GoMenuObservable => onClickMenuSubject;
        public IObservable<Unit> RetryLevelObservable => onClickRetrySubject;
        public override void Initialize ()
        {
            this.menuButton.onClick.AddListener (Menu);
            this.undoButton.onClick.AddListener (Undo);
            this.retryButton.onClick.AddListener (Retry);
        }

        public void SetData (int level)
        {
            this.levelText.text = $"Level:{level:00}";
        }

        private void Menu ()
        {
            this.onClickMenuSubject.OnNext (Unit.Default);
        }

        private void Retry ()
        {
            this.onClickRetrySubject.OnNext (Unit.Default);
        }

        private void Undo ()
        {
            Debug.Log ("UNDO!");
        }
    }
}