using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public sealed class GameManager : SingletonBehaviour<GameManager>
    {
        private GameParams gameParams = new GameParams ();
        public GameParams Params => this.gameParams;
        private const string SCENE_TAG = "Scene";

        public void LoadScene (string sceneName)
        {
            LoadSceneAsync (sceneName);
        }

        public async void LoadSceneAsync (string sceneName)
        {
            {
                var currentScene = GameObject.FindWithTag (SCENE_TAG).GetComponent<SceneBase> ();
                await currentScene.FadeOutScene ();
            }
            await SceneManager.LoadSceneAsync (sceneName);
            {
                var currentScene = GameObject.FindWithTag (SCENE_TAG).GetComponent<SceneBase> ();
                await currentScene.FadeInScene ();
            }
        }
    }
}