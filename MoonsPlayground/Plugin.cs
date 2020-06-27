using BeatSaberMarkupLanguage.Settings;
using IPA;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = IPA.Logging.Logger;

namespace MoonsPlayground
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private BeatmapModifier beatmapModifier;

        public string Name => SharedConstructs.Name;
        public string Version => SharedConstructs.Version;
        public static Logger Log { get; set; }

        [Init]
        public void Init(Logger log) => Log = log;

        [OnStart]

        public void OnStart()
        {
            var settings = UI.ViewControllers.Settings.instance;
            BSMLSettings.instance.AddSettingsMenu(SharedConstructs.Name, settings.ResourceName, settings);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "GameCore")
            {
                beatmapModifier = beatmapModifier ?? new GameObject(nameof(BeatmapModifier)).AddComponent<BeatmapModifier>();

                if (SettingsManager.Instance.ForceOneColor)
                {
                    beatmapModifier.TransformToOneColor();
                }
            }
        }

        public void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == "GameCore")
            {
                if (SyncHandler.Instance != null) SyncHandler.Destroy();
                if (ScoreMonitor.Instance != null) ScoreMonitor.Destroy();
                if (FloatingScoreScreen.Instance != null) FloatingScoreScreen.Destroy();
                if (AntiFail.Instance != null) AntiFail.Destroy();
                if (DisablePause) DisablePause = false; //We can't disable this up above since SyncHandler might need to know info about its status

                if (client != null && client.Connected)
                {
                    (client.Self as Player).PlayState = Player.PlayStates.Waiting;
                    var playerUpdated = new Event();
                    playerUpdated.Type = Event.EventType.PlayerUpdated;
                    playerUpdated.ChangedObject = client.Self;
                    client.Send(new Packet(playerUpdated));
                }
            }
        }
    }
}
