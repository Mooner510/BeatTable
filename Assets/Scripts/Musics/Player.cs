using System.Collections;
using Listener;
using Map;
using Musics.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Musics {
    public class Player : SingleMono<Player> {
        [SerializeField] protected GameObject beatInspector;
        [SerializeField] private SpriteRenderer hider;
        [SerializeField] private Text recording;

        private static readonly Color ClickColor = new Color(0f, 0.8f, 0.8f, 0.3f);

        private void Start() {
            if (MusicManager.Instance.IsPlayMode()) {
                NoteManager.LoadCurrentData();
                Debug.Log(NoteManager.GetNoteData().Count);
                Debug.Log("Data Loaded");
                recording.enabled = false;
            } else recording.enabled = true;
            StartCoroutine(Init());
        }

        public void Stop(bool save) {
            StopCoroutine(Init());
            StartCoroutine(End(save));
        }

        private IEnumerator End(bool save) {
            NoteManager.Stop(save);
            var color = hider.color;
            for (var i = 0f; i <= 3; i += Time.deltaTime) {
                color.a = i / 3;
                hider.color = color;
                yield return null;
            }
            hider.color = Color.black;
            SceneManager.LoadScene(MusicManager.Instance.IsPlayMode() ? 3 : 0);
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
            StartCoroutine(End(true));
        }

        public virtual IEnumerator Accept(LiveNoteData note, float time) {
            var colored = false;
            if(time > 0) yield return new WaitForSecondsRealtime(time);
            KeyListener.Instance.Queue(note);
            var obj = Instantiate(beatInspector, GameUtils.Locator(GameMode.Keypad, note.note), Quaternion.identity);
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            var noteTime = KeyListener.NoteTime / 2;
            var fullNoteTime = KeyListener.NoteTime / 2 + KeyListener.AllowedTime;
            for (var delta = 0f; delta <= fullNoteTime; delta += Time.deltaTime) {
                yield return null;
                obj.transform.position = MapMaker.Instance.GetNote(note.note).transform.position;
                obj.transform.localScale = new Vector3(delta * 4, delta * 4, delta * 4);
                if (colored || !(delta >= noteTime)) continue;
                spriteRenderer.color = ClickColor;
                colored = true;
            }
            Destroy(obj);
        }
    }
}