using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Musics {
    public class MusicManager : SingleTon<MusicManager> {
        private int _selection;
        private List<MusicData> _musicDataList;
        private bool _isPlayMode;

        public MusicData GetCurrentMusicData() => _musicDataList[_selection];

        public MusicManager() => Load();

        public MusicData Next() => IsLast() ? _musicDataList[_selection = 0] : _musicDataList[++_selection];

        public MusicData Back() => IsFirst() ? _musicDataList[_selection = _musicDataList.Count - 1] : _musicDataList[--_selection];

        public bool IsLast() => _selection >= _musicDataList.Count - 1;

        public bool IsFirst() => _selection <= 0;

        public bool IsPlayMode() => _isPlayMode;

        public void SetPlayMode(bool play) => _isPlayMode = play;

        private void Load() {
            _selection = 0;
            var musics = Json.LoadJsonFile<MusicList>("Assets/Data/data");
            _musicDataList = new List<MusicData>(from info in musics.musics select info.ToMusicData());
        }
    }
}