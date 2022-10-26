using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data {
    public class DataLoader {
        private static DataLoader _init;

        public static DataLoader Instance => _init ??= new DataLoader();

        private static List<NoteData> _noteData;
        private static List<NoteData> _writeNoteData = new List<NoteData>();
        private static int _index;

        public static List<NoteData> GetNoteData() => _noteData;

        public static int GetIndex() => _index;

        public static bool IsTop(int i) => _index + i >= _noteData.Count;

        public static NoteData Pick() => _noteData[_index];

        public static NoteData Pick(int i) => _noteData[_index + i];
        
        public static NoteData Pop() => _noteData[_index++];

        public static void LoadData() {
            _noteData = new List<NoteData>(Json.LoadJsonFile<GlobalNoteData>("data").data);
            Debug.Log(string.Join(", ", _noteData));
        }

        private static void SaveData() {
            Debug.Log(Json.CreateJsonFile("data", new GlobalNoteData(_writeNoteData.ToArray())));
        }

        public void Start() {
            Debug.Log("Data Start");
            Ticker.Instance.ResetTick();
            Ticker.Instance.Write();
        }

        public void Stop() {
            Debug.Log("Stop and Save");
            Ticker.Instance.StopWrite();
            SaveData();
        }

        public static void AddNote(int note) {
            _writeNoteData.Add(new NoteData(Ticker.Instance.GetPlayTime(), note));
            Debug.Log($"Write note[{_writeNoteData.Count}] : {note}");
            var str = _writeNoteData.Aggregate("", (current, data) => current + data);
            Debug.Log($"Value: {str}");
        }
    }
}
