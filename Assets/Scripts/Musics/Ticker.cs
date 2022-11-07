using System.Collections;
using Musics.Data;
using UnityEngine;

namespace Musics {
    public class Ticker : SingleMono<Ticker> {
        [SerializeField] private AudioSource beatSound;
        [SerializeField] private AudioSource musicSound;

        private bool _readTick;
        private float _writeTime;

        public void Beat() {
            // if (_beat) return;
            beatSound.PlayOneShot(beatSound.clip);
            // _beat = true;
        }

        public float GetStartTime() => _writeTime;

        public float GetPlayTime() => Time.realtimeSinceStartup - _writeTime;

        public void Write() {
            Debug.Log("Write Start");
            _writeTime = Time.realtimeSinceStartup;
            _readTick = true;
            // _routine = StartCoroutine(ReadTick());
            musicSound.PlayOneShot(MusicManager.Instance.GetCurrentMusicData().mainAudio);
        }

        public void StopWrite() {
            Debug.Log("Write Stop");
            if(!_readTick) return;
            _readTick = false;
            // if (_routine == null) return;
            // StopCoroutine(_routine);
            musicSound.Stop();
        }

        private void Update() {
            if (!_readTick) return;
            if (!MusicManager.Instance.IsPlayMode() || NoteManager.IsTop(0)) return;
            var now = GetPlayTime();
            var i = 0;
            do {
                var note = NoteManager.Pick(i);
                if (note.time <= now + 0.5f) {
                    // Debug.Log($"Tick: {note.time}");
                    StartCoroutine(Player.Instance.Accept(NoteManager.Pop(), note.time - (now + 0.5f)));
                } else break;
            } while (!NoteManager.IsTop(++i));
        }

        private IEnumerator ReadTick() {
            while (true) {
                // yield return _seconds;

                if (!MusicManager.Instance.IsPlayMode() || NoteManager.IsTop(0)) continue;
                var now = GetPlayTime();
                var i = 0;
                do {
                    var note = NoteManager.Pick(i);
                    if (note.time <= now + 1) {
                        // Debug.Log($"Tick: {note.time}");
                        StartCoroutine(Player.Instance.Accept(NoteManager.Pop(), note.time - (now + 1)));
                    }
                } while (!NoteManager.IsTop(++i));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}