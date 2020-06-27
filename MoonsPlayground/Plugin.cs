using BeatSaberMarkupLanguage.Settings;
using IPA;
using MoonsPlayground.Misc;
using MoonsPlayground.PlaygroundModules;
using MoonsPlayground.PlaygroundModules.Behaviours;
using UnityEngine;
using Logger = IPA.Logging.Logger;

namespace MoonsPlayground
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private HealthAndSafetyHijack healthAndSafetyHijack;
        private BeatmapModifier beatmapModifier;
        private RunProgramOnMiss runProgramOnMiss;

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

            healthAndSafetyHijack = healthAndSafetyHijack ?? new HealthAndSafetyHijack();
            beatmapModifier = beatmapModifier ?? new GameObject(nameof(BeatmapModifier)).AddComponent<BeatmapModifier>();
            runProgramOnMiss = runProgramOnMiss ?? new GameObject(nameof(RunProgramOnMiss)).AddComponent<RunProgramOnMiss>();
        }
    }
}
