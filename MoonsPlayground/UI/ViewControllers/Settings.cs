using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BS_Utils.Utilities;

namespace MoonsPlayground.UI.ViewControllers
{
    public class Settings : PersistentSingleton<Settings>
    {
        public string ResourceName => $"MoonsPlayground.UI.Views.{GetType().Name}.bsml";

        private SettingsManager SettingsManager => SettingsManager.Instance;

        [UIValue("force-one-color")]
        public bool ForceOneColor { get; set; }

        [UIValue("fuckery")]
        public bool Fuckery { get; set; }

        [UIAction("#cancel")]
        private void Cancel() => LoadSettings();

        private void Awake() => LoadSettings();

        private void LoadSettings()
        {
            ForceOneColor = SettingsManager.ForceOneColor;
            Fuckery = SettingsManager.Fuckery;
        }
    }
}
