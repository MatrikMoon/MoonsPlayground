using BeatSaberMarkupLanguage.Attributes;

namespace MoonsPlayground.UI.ViewControllers
{
    public class Settings : PersistentSingleton<Settings>
    {
        public string ResourceName => $"MoonsPlayground.UI.Views.{GetType().Name}.bsml";

        private SettingsManager SettingsManager => SettingsManager.Instance;

        [UIValue("force-one-color")]
        public bool ForceOneColor { get; set; }

        [UIValue("wtf-on-miss")]
        public bool WTFOnMiss { get; set; }

        [UIAction("#apply")]
        private void Apply()
        {
            SettingsManager.ForceOneColor = ForceOneColor;
            SettingsManager.WTFOnMiss = WTFOnMiss;
        }

        [UIAction("#cancel")]
        private void Cancel() => LoadSettings();

        private void Awake() => LoadSettings();

        private void LoadSettings()
        {
            ForceOneColor = SettingsManager.ForceOneColor;
            WTFOnMiss = SettingsManager.WTFOnMiss;
        }
    }
}
