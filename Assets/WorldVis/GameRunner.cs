using DG.Tweening;
using KSP.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldVis
{
    public class GameRunner : MonoBehaviour
    {

        public GameInstance gameInstance;
        public DOTweenAnimation splashAnim;
        public SplashScreensManager splashScreensManager;

        private void Awake()
        {
            Util.LoadGameBurstCode();
        }

        void LateUpdate()
        {
            if (!gameInstance)
            {
                gameInstance = FindAnyObjectByType<GameInstance>();
            }
            if (!splashScreensManager)
            {
                // I can't get the tweener to call ResolveSplashScreens(),
                // possible due to lack of  having the TweenPro editor.
                splashScreensManager = FindAnyObjectByType<SplashScreensManager>();
                splashAnim = FindAnyObjectByType<DOTweenAnimation>();
                splashScreensManager.ResolveSplashScreens();
            }
        }

        public void SplashAnimComplete()
        {
            Debug.Log("got anim complete callback");
        }
    }
}