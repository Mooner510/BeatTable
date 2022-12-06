using System.Collections;
using Musics;
using Musics.Data;
using UnityEngine;
using Utils;

namespace Map {
    public class QuadMapMaker : MapMaker {
        private void Start() {
            Notes = new GameObject[4];
            NoteRenderers = new SpriteRenderer[4];
            BackNotes = new GameObject[4];
            BackNoteRenderers = new SpriteRenderer[4];
            Routines = new Coroutine[4];
            for (var i = 0; i < 4; i++) {
                BackNoteRenderers[i] = (BackNotes[i] = Instantiate(beatBackButton, GameUtils.Locator(GameMode.Quad, i), Quaternion.identity))
                    .GetComponent<SpriteRenderer>();
                NoteRenderers[i] = (Notes[i] = Instantiate(beatButton, GameUtils.Locator(GameMode.Quad, i), Quaternion.identity))
                    .GetComponent<SpriteRenderer>();
            }
        }

        public override IEnumerator Beat(int note) {
            for (var time = 0f; time <= 0.5f; time += Time.deltaTime) {
                for (var i = 0; i < 4; i++) {
                    var pos = GameUtils.Locator(GameMode.Quad, i);
                    Notes[i].transform.localPosition = pos * (1 + (0.5f - time) / 8);
                    Notes[i].transform.localScale = Vector3.one * (2 + (0.5f - time) / 3);
                    BackNotes[i].transform.localPosition = pos * (1 + (0.5f - time) / 8);
                    BackNotes[i].transform.localScale = Vector3.one * (0.4f - time * 0.45f / 3);
                }
                yield return null;
            }
        }
    }
}