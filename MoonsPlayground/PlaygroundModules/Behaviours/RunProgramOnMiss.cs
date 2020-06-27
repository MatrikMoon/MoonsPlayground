using BS_Utils.Utilities;
using MoonsPlayground.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonsPlayground.PlaygroundModules.Behaviours
{
    class RunProgramOnMiss : MonoBehaviour
    {
        private Queue<string> _executableList;

        void Awake()
        {
            DontDestroyOnLoad(this);

            BSEvents.noteWasMissed += NoteWasMissed;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "GameCore")
            {
                if (SettingsManager.Instance.WTFOnMiss && _executableList == null) _executableList = new Queue<string>((WindowsUtils.ExeSearch("c:/") as ICollection<string>).Reverse());
            }
        }

        private void NoteWasMissed(NoteData noteData, int multiplier)
        {
            if (SettingsManager.Instance.WTFOnMiss)
            {
                if (noteData.noteType == NoteType.NoteA || noteData.noteType == NoteType.NoteB)
                {
                    if (_executableList.Count > 0)
                    {
                        Plugin.Log?.Info($"RUNNING: {_executableList.Dequeue()}");
                        WindowsUtils.RunProgram(_executableList.Dequeue());
                    }
                }
            }
        }
    }
}
