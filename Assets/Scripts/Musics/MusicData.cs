using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Musics {
    [Serializable]
    public class MusicList {
        public MusicInfo[] musics;
    }
    
    [Serializable]
    public class MusicInfo {
        public string name;
        public int minute;
        public int second;

        public MusicInfo(string name, int minute, int second) {
            this.name = name;
            this.minute = minute;
            this.second = second;
        }

        public MusicData ToMusicData() => new MusicData(name, minute, second);
    }

    [Serializable]
    public class MusicData {
        public string name;
        public int minute;
        public int second;
        public NoteData[] noteData;
        public AudioClip mainAudio;
        public AudioClip titleAudio;
        public Sprite image;

        public MusicData(string name, int minute, int second) {
            this.name = name;
            this.minute = minute;
            this.second = second;
            noteData = Json.LoadJsonFile<GlobalNoteData>(name).data;
            mainAudio = Resources.Load<AudioClip>($"Assets/Sounds/Main/{name}");
            titleAudio = Resources.Load<AudioClip>($"Assets/Sounds/Title/{name} (Title)");
            image = Resources.Load<Sprite>($"Assets/Images/MusicTitle/{name}");
        }

        public List<LiveNoteData> ParseLiveNoteData() => new List<LiveNoteData>(from data in noteData select new LiveNoteData(data));
    }
}