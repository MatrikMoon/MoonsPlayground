using HMUI;
using MoonsPlayground.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonsPlayground.PlaygroundModules
{
    class HealthAndSafetyHijack
    {
        private HealthWarningFlowCoordinator healthWarningFlowCoordinator;
        private HealthWarningViewController healthWarningViewController;
        private EulaViewController eulaViewController;

        public HealthAndSafetyHijack()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "HealthWarning")
            {
                SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;

                SharedCoroutineStarter.instance.StartCoroutine(HijackHealthAndSafety());
            }
        }

        private IEnumerator HijackHealthAndSafety()
        {
            healthWarningFlowCoordinator = Resources.FindObjectsOfTypeAll<HealthWarningFlowCoordinator>().First();
            healthWarningViewController = healthWarningFlowCoordinator.GetField<HealthWarningViewController>("_healthWarningViewContoller");
            eulaViewController = healthWarningFlowCoordinator.GetField<EulaViewController>("_eulaViewController");

            yield return new WaitUntil(() => healthWarningViewController.isInViewControllerHierarchy);

            healthWarningViewController.didFinishEvent -= healthWarningFlowCoordinator.HandleHealthWarningViewControllerdidFinish;
            healthWarningViewController.didFinishEvent += HandleHealtWarningViewControllerdidFinish;
        }

        public void HandleHealtWarningViewControllerdidFinish()
        {
            healthWarningViewController.didFinishEvent -= HandleHealtWarningViewControllerdidFinish;
            eulaViewController.didFinishEvent -= healthWarningFlowCoordinator.HandleEulaViewControllerdidFinish;
            eulaViewController.didFinishEvent += HandleEulaViewControllerdidFinish;

            healthWarningFlowCoordinator.InvokeMethod("PresentViewController", eulaViewController, null, false);
            SharedCoroutineStarter.instance.StartCoroutine(RewriteHealthAndSafety());
        }

        private IEnumerator RewriteHealthAndSafety()
        {
            yield return new WaitUntil(() => eulaViewController.isInViewControllerHierarchy);

            healthWarningFlowCoordinator.SetProperty("title", "Moon's Playground");

            var textPageScrollView = eulaViewController.GetField<TextPageScrollView>("_textPageScrollView");
            textPageScrollView.SetText("By using my playground mod, you understand that:\n\n" +
                "1. You may experience problems that don't exist in the vanilla game.\n\n" +
                "2. Some features (see: meme features such as \"WTF On Miss\") may cause real damage to your computer\n\n" +
                "3. By agreeing to this, you waive your right to blame me for issues and/or damage caused by this plugin");
        }

        public void HandleEulaViewControllerdidFinish(bool agreed)
        {
            eulaViewController.didFinishEvent -= HandleEulaViewControllerdidFinish;

            if (agreed) healthWarningFlowCoordinator.HandleHealthWarningViewControllerdidFinish();
            else healthWarningFlowCoordinator.HandleEulaViewControllerdidFinish(agreed);
        }
    }
}
