using BS_Utils.Utilities;
using MoonsPlayground.Misc;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonsPlayground.PlaygroundModules.Behaviours
{
    class BeatmapModifier : MonoBehaviour
    {
        private NoteType _currentNoteType = NoteType.NoteB;

        void Awake()
        {
            DontDestroyOnLoad(this);

            BSEvents.noteWasMissed += NoteWasMissed;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void NoteWasMissed(NoteData noteData, int multiplier)
        {
            /*if (noteData.noteType == NoteType.NoteA || noteData.noteType == NoteType.NoteB)
            {
                _currentNoteType = _currentNoteType == NoteType.NoteA ? NoteType.NoteB : NoteType.NoteA;
                Plugin.Log?.Info("RE-FORCING");
                ForceOneColor(_currentNoteType);
            }*/
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "GameCore")
            {
                if (SettingsManager.Instance.ForceOneColor) ForceOneColorWhenLoaded();
            }
        }

        private IEnumerator WaitForSongLoad(Action runWhenLoaded)
        {
            var standardLevelGameplayManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().First();
            yield return new WaitUntil(() => standardLevelGameplayManager.GetField<StandardLevelGameplayManager.GameState>("_gameState") == StandardLevelGameplayManager.GameState.Playing);
            yield return new WaitUntil(() => standardLevelGameplayManager.GetField<PauseController>("_pauseController").GetProperty<bool>("canPause"));

            runWhenLoaded?.Invoke();
        }

        public void ForceOneColorWhenLoaded()
        {
            StartCoroutine(WaitForSongLoad(() => ForceOneColor(NoteType.NoteB)));
        }

        private void ForceOneColor(NoteType desiredNoteType)
        {
            var audioTimeSyncController = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().First();
            var beatmapObjectCallbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            var player = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();
            var beatmapData = beatmapObjectCallbackController.GetField<BeatmapData>("_beatmapData");

            //Plugin.Log?.Info("Disabling submission on One Color No Arrows transformation");
            BS_Utils.Gameplay.ScoreSubmission.DisableSubmission(SharedConstructs.Name);

            // Transform the map to One Color and No Arrows
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                var objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.time < audioTimeSyncController.songTime + 1) continue;
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        var note = beatmapObject as NoteData;
                        note.SetNoteToAnyCutDirection();

                        if (note.noteType != desiredNoteType) note.SwitchNoteType();
                    }
                }
            }

            // Change the other saber to desired type
            var desiredSaberType = SaberType.SaberB;
            var saberTypeObject = new GameObject("SaberTypeObject").AddComponent<SaberTypeObject>();
            saberTypeObject.SetField("_saberType", desiredSaberType);

            var saberToSwap = player.leftSaber;
            saberToSwap.SetField("_saberType", saberTypeObject);

            beatmapObjectCallbackController.SetNewBeatmapData(beatmapData);
        }
    }
}
