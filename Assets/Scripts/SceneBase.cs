using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class SceneBase : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup = null;
        [SerializeField] protected GameObject tapCoverLayer = null;
        private const float FADE_DURATION = 1f;

        public async UniTask FadeOutScene ()
        {
            this.canvasGroup.alpha = 1f;
            this.tapCoverLayer.SetActive (true);
            await this.canvasGroup.DOFade (0f, FADE_DURATION);
        }

        public async UniTask FadeInScene ()
        {
            this.tapCoverLayer.SetActive (true);
            this.canvasGroup.alpha = 0f;
            await this.canvasGroup.DOFade (1f, FADE_DURATION);
            this.tapCoverLayer.SetActive (false);
        }
    }
}