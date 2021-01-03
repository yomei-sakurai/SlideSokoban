using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class MenuScene : SceneBase
    {
        [SerializeField] private LevelList levelList = null;
        public static readonly string SCENE_NAME = "MenuScene";
        private int reachedLevel = 0;

        private void Start ()
        {
            this.reachedLevel = PlayerPrefs.GetInt (Constants.REACHED_LEVEL, 0);
            this.levelList.Initialize ();
            this.levelList.SelectLevelObservable.Subscribe (number => GoLevel (number));
        }

        private void Update ()
        {

        }

        private void GoLevel (int levelNumber)
        {
            GameManager.Instance.Params.CurrentLevel = levelNumber;
            GameManager.Instance.LoadScene (GameScene.SCENE_NAME);
        }
    }
}