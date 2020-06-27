using System.Collections;
using System.Linq;
using UnityEngine;

namespace MoonsPlayground
{
    class BeatmapModifier : MonoBehaviour
    {
        private IEnumerator WaitForDataLoad()
        {
            var beatmapObjectCallbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().First();
            var player = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();

            yield return new WaitUntil(() => beatmapObjectCallbackController.GetField<BeatmapData>("_beatmapData") != null);
            yield return new WaitUntil(() => player.rightSaber != null && player.leftSaber != null);

            var beatmapData = beatmapObjectCallbackController.GetField<BeatmapData>("_beatmapData");

            Plugin.Log?.Info("Disabling submission on One Color No Arrows transformation");
            BS_Utils.Gameplay.ScoreSubmission.DisableSubmission(SharedConstructs.Name);

            // Transform the map to One Color and No Arrows
            //NoteType undesiredNoteType = ConfigOptions.instance.LeftHanded ? NoteType.NoteB : NoteType.NoteA;
            NoteType undesiredNoteType = NoteType.NoteA;
            foreach (BeatmapLineData line in beatmapData.beatmapLinesData)
            {
                var objects = line.beatmapObjectsData;
                foreach (BeatmapObjectData beatmapObject in objects)
                {
                    if (beatmapObject.beatmapObjectType == BeatmapObjectType.Note)
                    {
                        var note = beatmapObject as NoteData;
                        note.SetNoteToAnyCutDirection();

                        if (note.noteType == undesiredNoteType) note.SwitchNoteType();
                    }
                }
            }

            // Change the other saber to desired type
            var desiredSaberType = SaberType.SaberB;
            var saberObject = new GameObject("SaberTypeObject").AddComponent<SaberTypeObject>();
            saberObject.SetField("_saberType", desiredSaberType);

            var saberToSwap = player.leftSaber;
            saberToSwap.SetField("_saberType", saberObject);

            beatmapObjectCallbackController.SetNewBeatmapData(beatmapData);
        }

        public void TransformToOneColor()
        {
            StartCoroutine(WaitForDataLoad());
        }
    }
}
