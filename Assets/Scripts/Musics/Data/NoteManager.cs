using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Musics.Data {
    public static class NoteManager {
        private static List<LiveNoteData> _noteData;
        private static List<NoteData> _writeNoteData = new List<NoteData>();
        private static int _index;

        public static bool IsTop(int i) => _index + i >= _noteData.Count;

        public static LiveNoteData Pick() => _noteData[_index];

        public static LiveNoteData Pick(int i) => _noteData[_index + i];
        
        public static LiveNoteData Pop() => _noteData[_index++];

        public static void LoadCurrentData() => _noteData = MusicManager.Instance.GetCurrentMusicData().ParseLiveNoteData();

        private static void SaveData() {
            Json.CreateJsonFile($"Assets/Data/Map/{MusicManager.Instance.GetCurrentMusicData().name}", new GlobalNoteData(_writeNoteData.ToArray()));
        }

        public static void Start() {
            Debug.Log("Data Start");
            Ticker.Instance.Write();
        }

        public static void Stop() {
            Debug.Log("Stop and Save");
            Ticker.Instance.StopWrite();
            if (Player.Instance.IsPlay()) return;
            SaveData();
            _writeNoteData = new List<NoteData>();
        }

        public static void AddNote(int note) {
            _writeNoteData.Add(new NoteData(Ticker.Instance.GetPlayTime(), note));
            Debug.Log($"Write note[{_writeNoteData.Count}] : {note}");
            var str = _writeNoteData.Aggregate("", (current, data) => current + data);
            Debug.Log($"Value: {str}");
        }
    }
}
