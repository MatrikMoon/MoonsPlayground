using BS_Utils.Utilities;

namespace MoonsPlayground
{
    public class SettingsManager
    {
        public static readonly SettingsManager Instance = new SettingsManager();
        public static readonly string MainSection = "Main";

        private Config Config = new Config(SharedConstructs.Name);

        public bool ForceOneColor
        {
            get => Config.GetBool(MainSection, nameof(ForceOneColor), false);
            set
            {
                Config.SetBool(MainSection, nameof(ForceOneColor), value);
            }
        }

        public bool Fuckery
        {
            get => Config.GetBool(MainSection, nameof(Fuckery), false);
            set
            {
                Config.SetBool(MainSection, nameof(Fuckery), value);
            }
        }
    }
}
