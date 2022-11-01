using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data {
    public static class DataLoader {
        private static List<LiveNoteData> _noteData;
        private static List<NoteData> _writeNoteData = new List<NoteData>();
        private static int _index;

        public static List<LiveNoteData> GetNoteData() => _noteData;

        public static int GetIndex() => _index;

        public static bool IsTop(int i) => _index + i >= _noteData.Count;

        public static LiveNoteData Pick() => _noteData[_index];

        public static LiveNoteData Pick(int i) => _noteData[_index + i];
        
        public static LiveNoteData Pop() => _noteData[_index++];

        public static int GetNotes() => _noteData.Count;

        public static void LoadData() {
            _noteData = new List<LiveNoteData>();
            foreach (var noteData in Json.LoadJsonFile<GlobalNoteData>("data").data) {
                _noteData.Add(new LiveNoteData(noteData));
            }
            Debug.Log(string.Join(", ", _noteData));
        }

        private static void SaveData() {
            Debug.Log(Json.CreateJsonFile("data", new GlobalNoteData(_writeNoteData.ToArray())));
        }

        public static void Start() {
            Debug.Log("Data Start");
            Ticker.Instance.Write();
        }

        public static void Stop() {
            Debug.Log("Stop and Save");
            Ticker.Instance.StopWrite();
            if(!Player.Instance.IsPlay()) SaveData();
        }

        public static void AddNote(int note) {
            _writeNoteData.Add(new NoteData(Ticker.Instance.GetPlayTime(), note));
            Debug.Log($"Write note[{_writeNoteData.Count}] : {note}");
            var str = _writeNoteData.Aggregate("", (current, data) => current + data);
            Debug.Log($"Value: {str}");
        }
    }
}
