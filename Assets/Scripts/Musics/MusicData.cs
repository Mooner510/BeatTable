using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Musics.Data;
using UnityEngine;
using Utils;

namespace Musics {
    [Serializable]
    public class MusicList {
        public MusicInfo[] musics;

        public MusicList(MusicInfo[] musics) => this.musics = musics;
    }
    
    [Serializable]
    public class MusicInfo {
        public string name;
        public string artist;
        public string arrange;
        public float bpm;
        public int minute;
        public int second;

        public MusicInfo(string name, string artist, string arrange, int minute, int second) {
            this.name = name;
            this.artist = artist;
            this.arrange = arrange;
            this.minute = minute;
            this.second = second;
        }

        public MusicData ToMusicData() => new MusicData(name, artist, arrange, bpm, minute, second);
    }

    [Serializable]
    public class MusicData {
        public string name;
        public string artist;
        public string arrange;
        public float bpm;
        public int minute;
        public int second;
        public NoteData[] noteData;
        public AudioClip mainAudio;
        public AudioClip titleAudio;
        public Sprite image;
        public Sprite blurImage;
        public GameMode gameMode;

        public MusicData(string name, string artist, string arrange, float bpm, int minute, int second) {
            this.name = name;
            this.artist = artist;
            this.arrange = arrange;
            this.bpm = bpm;
            this.minute = minute;
            this.second = second;
            try {
                var globalNoteData = Json.LoadJsonFile<GlobalNoteData>($"Assets/Data/Map/{name}");
                noteData = globalNoteData.data;
                gameMode = globalNoteData.gameMode;
            } catch (FileNotFoundException) {
                noteData = null;
            }
            mainAudio = Resources.Load<AudioClip>($"Sounds/{name}");
            titleAudio = Resources.Load<AudioClip>($"Sounds/{name} (Title)");
            image = Resources.Load<Sprite>($"MusicImage/{name}");
            blurImage = Resources.Load<Sprite>($"MusicImage/Blur/{name}");
        }

        public List<LiveNoteData> ParseLiveNoteData() => new List<LiveNoteData>(from data in noteData select new LiveNoteData(data));
    }
}