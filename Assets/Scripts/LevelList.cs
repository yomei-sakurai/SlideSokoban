using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LevelList : BehaviourBase
    {
        [SerializeField] private Transform listContent = null;
        [SerializeField] private GameObject buttonPrefab = null;
        [SerializeField] private Button prevButton = null;
        [SerializeField] private Button nextButton = null;
        private Subject<int> selectLevelSubject = new Subject<int> ();
        public IObservable<int> SelectLevelObservable => selectLevelSubject;
        private const int LIST_BUTTON_PER_PAGE = 20;
        private int currentPage = 0;
        private LevelDatabase levelDatabase = null;
        private int MaxPage => (this.levelDatabase.levelList.Count - 1) / LIST_BUTTON_PER_PAGE;
        public override void Initialize ()
        {
            this.levelDatabase = Resources.Load<LevelDatabase> (LevelDatabase.LEVEL_DATABASE_PATH);
            GameManager.Instance.Params.MaxLevel = this.levelDatabase.levelList.Count;
            UpdateList ();

            this.prevButton.onClick.AddListener (OnClickPrevButton);
            this.nextButton.onClick.AddListener (OnClickNextButton);
        }

        private void UpdateList ()
        {
            ClearList ();

            foreach (var level in this.levelDatabase.levelList.GetRangeCustom (currentPage * LIST_BUTTON_PER_PAGE, LIST_BUTTON_PER_PAGE))
            {
                var button = Instantiate (buttonPrefab, listContent).GetComponent<LevelListButton> ();
                button.Initialize ();
                button.SetNumber (level.number);
                button.OnClickButtonObservable.Subscribe (SelectLevel);
            }

            UpdateButtons ();
        }

        private void ClearList ()
        {
            foreach (Transform t in this.listContent)
            {
                Destroy (t.gameObject);
            }
        }

        private void UpdateButtons ()
        {
            this.prevButton.gameObject.SetActive (this.currentPage > 0);
            this.nextButton.gameObject.SetActive (this.currentPage < MaxPage);
        }

        private void SelectLevel (int num)
        {
            this.selectLevelSubject.OnNext (num);
        }

        private void OnClickPrevButton ()
        {
            --this.currentPage;
            UpdateList ();
        }

        private void OnClickNextButton ()
        {
            ++this.currentPage;
            UpdateList ();
        }

        private void CheckPage ()
        {
            if (this.currentPage < 0) this.currentPage = 0;
            if (this.currentPage > MaxPage) this.currentPage = MaxPage;
        }
    }
}