using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Musics.Data {
    public static class NoteManager {
        private static List<LiveNoteData> _noteData;
        private static List<NoteData> _writeNoteData = new List<NoteData>();
        private static int _index;
        private static int _noteSpeed = 80;

        public static float GetNoteSpeed() => _noteSpeed / 10f;

        public static float NoteSpeedUp(bool shift) => _noteSpeed = Math.Min(_noteSpeed + (shift ? 10 : 1), 160);

        public static float NoteSpeedDown(bool shift) => _noteSpeed = Math.Max(_noteSpeed - (shift ? 10 : 1), 1);

        public static bool IsTop(int i) => _index + i >= _noteData.Count;

        public static LiveNoteData Pick() => _noteData[_index];

        public static LiveNoteData Pick(int i) => _noteData[_index + i];
        
        public static LiveNoteData Pop() => _noteData[_index++];

        public static void LoadCurrentData() => _noteData = MusicManager.Instance.GetCurrentMusicData().ParseLiveNoteData(MusicManager.GetCurrentGameMode());

        public static List<LiveNoteData> GetNoteData() => _noteData;

        private static void SaveData() {
            var musicData = MusicManager.Instance.GetCurrentMusicData();
            var gameMode = MusicManager.GetCurrentGameMode();
            Json.CreateJsonFile($"Assets/Data/Map/{gameMode.ToString()}/{musicData.name}", new GlobalNoteData(_writeNoteData.ToArray()));
        }

        private static void ClearRecordData() {
            _index = 0;
            _writeNoteData = new List<NoteData>();
        }

        public static void Start() {
            Debug.Log("Data Start");
            Ticker.Instance.Write();
        }

        public static void Stop(bool save) {
            Debug.Log("Stop and Save");
            Ticker.Instance.StopWriteSoftness();
            if (!MusicManager.Instance.IsPlayMode() && save) {
                SaveData();
                MusicManager.Instance.GetCurrentMusicData().Update();
            }
            ClearRecordData();
        }

        public static void AddNote(int note) {
            _writeNoteData.Add(new NoteData(Ticker.Instance.GetPlayTime(), note));
            Debug.Log($"Write note[{_writeNoteData.Count}] : {note}");
            var str = _writeNoteData.Aggregate("", (current, data) => current + data);
            Debug.Log($"Value: {str}");
        }
    }
}
