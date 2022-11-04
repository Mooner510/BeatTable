using System.Collections;
using UnityEngine;
using Utils;

namespace Map {
    public class MapMaker : SingleMono<MapMaker> {
        private GameObject[] _notes;
        private SpriteRenderer[] _noteRenderers;
        [SerializeField] private GameObject beatButton;

        private Coroutine[] _routines;

        private void Start() {
            _notes = new GameObject[9];
            _noteRenderers = new SpriteRenderer[9];
            _routines = new Coroutine[9];
            for (var i = 0; i < 9; i++) {
                _noteRenderers[i] = (_notes[i] = Instantiate(beatButton, GameUtils.Locator(i), Quaternion.identity))
                    .GetComponent<SpriteRenderer>();
            }
        }

        public GameObject GetNote(int index) => _notes[index];

        public void Click(int note) {
            if(_routines[note] != null) StopCoroutine(_routines[note]);
            _routines[note] = StartCoroutine(Clicking(note));
        }

        private IEnumerator Clicking(int note) {
            _noteRenderers[note].color = Color.yellow;
            for (var i = 0f; i <= 0.5f; i += Time.deltaTime) {
                yield return null;
                _noteRenderers[note].color = Color.yellow + (Color.white - Color.yellow) * i * 2;
            }
            _noteRenderers[note].color = Color.white;
        }

        public IEnumerator Beat() {
            for (var time = 0f; time <= 0.5f; time += Time.deltaTime) {
                for (var i = 0; i < 9; i++) {
                    var pos = GameUtils.Locator(i);
                    _notes[i].transform.localPosition = pos * (1 + (0.5f - time) / 8);
                    _notes[i].transform.localScale = Vector3.one * (2 + (0.5f - time) / 3);
                }
                yield return null;
            }
        }
    }
}