using System.Collections.Generic;
using System.Linq;
using Musics.Data;
using UnityEngine;
using Utils;

namespace Musics {
    public class MusicManager : SingleTon<MusicManager> {
        private int _selection;
        private List<MusicData> _musicDataList;
        private bool _isPlayMode;
        private MusicList _musics;
        private static GameMode _latestGameMode;

        public static void SetGameMode(GameMode gameMode) => _latestGameMode = gameMode;

        public static GameMode GetCurrentGameMode() => _latestGameMode;
        // if (SceneManager.GetActiveScene().name.Equals("End")) return _latestGameMode;   
        // return _latestGameMode = SceneManager.GetActiveScene().name.Equals("InGame") ? GameMode.Keypad : GameMode.Quad;

        public int GetCurrentMusicId() => _selection;

        public MusicData GetCurrentMusicData() => _musicDataList[_selection];

        public void UpdateCurrentMusicData() => _musicDataList[_selection] = _musics.musics[_selection].ToMusicData();

        // public MusicManager() => ReloadAll();

        public MusicData Next() => IsLast() ? _musicDataList[_selection = 0] : _musicDataList[++_selection];

        public MusicData Back() => IsFirst() ? _musicDataList[_selection = _musicDataList.Count - 1] : _musicDataList[--_selection];

        public bool IsLast() => _selection >= _musicDataList.Count - 1;

        public bool IsFirst() => _selection <= 0;

        public bool IsPlayMode() => _isPlayMode;

        public void SetPlayMode(bool play) => _isPlayMode = play;

        public void ReloadAll() {
            Debug.Log("Reloaded All Musics");
            _musics = Json.LoadJsonFile<MusicList>("Assets/Data/data");
            _musicDataList = new List<MusicData>(from info in _musics.musics select info.ToMusicData());
        }
    }
}