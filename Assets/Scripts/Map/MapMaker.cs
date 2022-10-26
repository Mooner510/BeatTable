using System.Collections;
using UnityEngine;

namespace Map {
    public class MapMaker : SingleMono<MapMaker> {
        public GameObject[] notes;
        public SpriteRenderer[] noteRenderers;
        [SerializeField] private GameObject beatButton;

        private Coroutine[] _routines;

        private void Start() {
            notes = new GameObject[9];
            noteRenderers = new SpriteRenderer[9];
            _routines = new Coroutine[9];
            for (var i = 0; i < 9; i++) {
                noteRenderers[i] = (notes[i] = Instantiate(beatButton, Utils.Locator(i), Quaternion.identity))
                    .GetComponent<SpriteRenderer>();
            }
        }

        public void Click(int note) {
            if(_routines[note] != null) StopCoroutine(_routines[note]);
            _routines[note] = StartCoroutine(Clicking(note));
        }

        private IEnumerator Clicking(int note) {
            noteRenderers[note].color = Color.yellow;
            for (var i = 0f; i <= 0.5f; i += Time.deltaTime) {
                yield return null;
                noteRenderers[note].color = Color.yellow + (Color.white - Color.yellow) * i * 2;
            }
            noteRenderers[note].color = Color.white;
        }
    }
}