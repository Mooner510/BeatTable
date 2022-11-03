using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Musics {
    public class MusicManager : SingleTon<MusicManager> {
        private int _selection;
        private List<MusicData> _musicDataList;

        public MusicData GetCurrentMusicData() => _musicDataList[_selection];

        public MusicManager() => Load();

        [CanBeNull] public MusicData Next() => IsLast() ? null : _musicDataList[++_selection];
        
        [CanBeNull] public MusicData Back() => IsFirst() ? null : _musicDataList[--_selection];

        public bool IsLast() => _selection >= _musicDataList.Count - 1;

        public bool IsFirst() => _selection <= 0;

        private void Load() {
            _selection = 0;
            _musicDataList = new List<MusicData>(from info in Json.LoadJsonFile<MusicList>("Assets/Data/data").musics select info.ToMusicData());
        }
    }
}