using System.Collections;
using Musics;
using Musics.Data;
using UnityEngine;
using Utils;

namespace Map {
    public class MapMaker : SingleMono<MapMaker> {
        protected GameObject[] Notes;
        protected SpriteRenderer[] NoteRenderers;
        [SerializeField] protected GameObject beatButton;

        protected Coroutine[] Routines;

        private void Start() {
            Notes = new GameObject[9];
            NoteRenderers = new SpriteRenderer[9];
            Routines = new Coroutine[9];
            for (var i = 0; i < 9; i++) {
                NoteRenderers[i] = (Notes[i] = Instantiate(beatButton, GameUtils.Locator(GameMode.Keypad, i), Quaternion.identity))
                    .GetComponent<SpriteRenderer>();
            }
        }

        public GameObject GetNote(int index) => Notes[index];

        public void Click(int note) {
            if(Routines[note] != null) StopCoroutine(Routines[note]);
            Routines[note] = StartCoroutine(Clicking(note));
        }

        private IEnumerator Clicking(int note) {
            NoteRenderers[note].color = Color.yellow;
            for (var i = 0f; i <= 0.5f; i += Time.deltaTime) {
                yield return null;
                NoteRenderers[note].color = Color.yellow + (Color.white - Color.yellow) * i * 2;
            }
            NoteRenderers[note].color = Color.white;
        }

        public virtual IEnumerator Beat() {
            for (var time = 0f; time <= 0.5f; time += Time.deltaTime) {
                for (var i = 0; i < 9; i++) {
                    var pos = GameUtils.Locator(MusicManager.Instance.GetCurrentMusicData().gameMode, i);
                    Notes[i].transform.localPosition = pos * (1 + (0.5f - time) / 8);
                    Notes[i].transform.localScale = Vector3.one * (2 + (0.5f - time) / 3);
                }
                yield return null;
            }
        }
    }
}