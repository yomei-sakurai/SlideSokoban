using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class GameScene : SceneBase
    {
        [SerializeField] private Board board = null;
        [SerializeField] private GameUI gameUI = null;
        [SerializeField] private OverlayUI overlayUI = null;
        public static readonly string SCENE_NAME = "GameScene";
        private int currentLevelNum = 1;

        private void Start ()
        {
            this.currentLevelNum = GameManager.Instance.Params.CurrentLevel;
            this.board.SetLevel (this.currentLevelNum);
            this.gameUI.Initialize ();
            this.gameUI.SetData (this.currentLevelNum);
            this.overlayUI.Initialize ();
            ObserverSetUp ();
        }

        private void ObserverSetUp ()
        {
            this.board.LevelCompleteObservable.Subscribe (unit =>
            {
                this.overlayUI.ShowResult ();
            });

            this.overlayUI.GoNextLevelObservable.Subscribe (unit => NextLevel ());

            this.gameUI.GoMenuObservable.Subscribe (unit => GoMenu ());
            this.gameUI.RetryLevelObservable.Subscribe (unit => RetryLevel ());
        }

        private void Update ()
        {
            this.board.Update ();
            this.gameUI.Update ();
        }

        private void GoMenu ()
        {
            Debug.Log ("MENU");
            GameManager.Instance.LoadScene (MenuScene.SCENE_NAME);
        }

        private void RetryLevel ()
        {
            Debug.Log ("RETRY!");
            //this.board.SetLevel (this.currentLevelNum);
            GameManager.Instance.LoadScene (GameScene.SCENE_NAME);
        }

        private void NextLevel ()
        {
            if (this.currentLevelNum == GameManager.Instance.Params.MaxLevel)
            {
                GameManager.Instance.LoadScene (MenuScene.SCENE_NAME);
                return;
            }
            GameManager.Instance.Params.CurrentLevel = this.currentLevelNum + 1;
            GameManager.Instance.LoadScene (GameScene.SCENE_NAME);
            /*
            this.board.SetLevel (++this.currentLevelNum);
            this.gameUI.SetData (this.currentLevelNum);
            */
        }
    }
}