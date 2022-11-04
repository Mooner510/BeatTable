using System.Collections;
using Map;
using Musics.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Musics {
    public class Player : MonoBehaviour {
        public static Player Instance;
    
        [SerializeField] private GameObject beatInspector;
        [SerializeField] private SpriteRenderer hider;
        [SerializeField] private Text recording;

        private static readonly Color ClickColor = new Color(0f, 0.8f, 0.8f, 0.3f);

        private void Start() {
            Instance = this;
            if (MusicManager.Instance.IsPlayMode()) {
                NoteManager.LoadCurrentData();
                recording.enabled = false;
            } else recording.enabled = true;
            StartCoroutine(Init());
        }

        private IEnumerator Init() {
            var color = hider.color;
            for (var i = 0f; i <= 3; i += Time.deltaTime) {
                color.a = 1 - i / 3;
                hider.color = color;
                yield return null;
            }
            hider.color = Color.clear;
            yield return new WaitForSecondsRealtime(1);
            NoteManager.Start();
            var data = MusicManager.Instance.GetCurrentMusicData();
            yield return new WaitForSecondsRealtime(data.minute * 60 + data.second + 1);
            for (var i = 0f; i <= 3; i += Time.deltaTime) {
                color.a = i / 3;
                hider.color = color;
                yield return null;
            }
            hider.color = Color.black;
            SceneManager.LoadScene(2);
        }

        public IEnumerator Accept(LiveNoteData note, float time) {
            var colored = false;
            yield return new WaitForSeconds(time);
            KeyListener.Instance.Queue(note);
            var obj = Instantiate(beatInspector, GameUtils.Locator(note.note), Quaternion.identity);
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            for (var delta = 0f; delta <= 1.25; delta += Time.deltaTime) {
                yield return null;
                obj.transform.position = MapMaker.Instance.GetNote(note.note).transform.position;
                obj.transform.localScale = new Vector3(delta * 2, delta * 2, delta * 2);
                if (colored || !(delta >= 0.9f)) continue;
                spriteRenderer.color = ClickColor;
                colored = true;
            }
            Destroy(obj);
        }
    }
}