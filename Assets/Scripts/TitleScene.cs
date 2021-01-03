using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TitleScene : SceneBase
    {
        [SerializeField] private Button tapScreen = null;
        public static readonly string SCENE_NAME = "TitleScene";

        private void Start ()
        {
            this.tapScreen.onClick.AddListener (OnClickScreen);
        }

        private void Update ()
        {

        }

        private void OnClickScreen ()
        {
            GameManager.Instance.LoadScene (MenuScene.SCENE_NAME);
        }
    }
}