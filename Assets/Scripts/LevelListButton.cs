using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LevelListButton : BehaviourBase
    {
        [SerializeField] private Text text = null;
        [SerializeField] private Button button = null;
        private Subject<int> onClickButtonSubject = new Subject<int> ();
        public IObservable<int> OnClickButtonObservable => onClickButtonSubject;
        private int levelNumber = 0;

        public override void Initialize ()
        {
            this.button.onClick.AddListener (OnClick);
        }

        public void SetNumber (int num)
        {
            this.levelNumber = num;
            this.text.text = num.ToString ("00");
        }

        private void OnClick ()
        {
            this.onClickButtonSubject.OnNext (this.levelNumber);
        }
    }
}