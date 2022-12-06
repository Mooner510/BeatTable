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
        public int difficulty;

        public MusicInfo(string name, string artist, string arrange, int minute, int second, int difficulty) {
            this.name = name;
            this.artist = artist;
            this.arrange = arrange;
            this.minute = minute;
            this.second = second;
            this.difficulty = difficulty;
        }

        public MusicData ToMusicData() => new MusicData(name, artist, arrange, bpm, minute, second, difficulty);
    }

    [Serializable]
    public class MusicData {
        public string name;
        public string artist;
        public string arrange;
        public float bpm;
        public int minute;
        public int second;
        public int difficulty;
        public NoteData[] quadNoteData;
        public NoteData[] keypadNoteData;
        public AudioClip mainAudio;
        public AudioClip titleAudio;
        public Sprite image;
        public Sprite blurImage;

        private static Color[] _difficultyColors = {
            new Color(0.56470588235294117647058823529412f, 1f, 0.5f),
            new Color(0.98823529411764705882352941176471f, 1f, 0.3921568627450980392156862745098f),
            new Color(1f, 0.67450980392156862745098039215686f, 0.44313725490196078431372549019608f),
            new Color(1f, 0.67450980392156862745098039215686f, 0.44313725490196078431372549019608f),
            new Color(1f, 0.4078431372549019607843137254902f, 0.4078431372549019607843137254902f),
            new Color(0.74117647058823529411764705882353f, 0.4078431372549019607843137254902f, 1f)
        };

        public MusicData(string name, string artist, string arrange, float bpm, int minute, int second,
            int difficulty) {
            this.name = name;
            this.artist = artist;
            this.arrange = arrange;
            this.bpm = bpm;
            this.minute = minute;
            this.second = second;
            this.difficulty = difficulty;
            try {
                var globalNoteData = Json.LoadJsonFile<GlobalNoteData>($"Assets/Data/Map/Keypad/{name}");
                keypadNoteData = globalNoteData.data;
            } catch (FileNotFoundException) {
                keypadNoteData = null;
            }
            try {
                var globalNoteData = Json.LoadJsonFile<GlobalNoteData>($"Assets/Data/Map/Quad/{name}");
                quadNoteData = globalNoteData.data;
            } catch (FileNotFoundException) {
                quadNoteData = null;
            }

            mainAudio = Resources.Load<AudioClip>($"Sounds/{name}");
            titleAudio = Resources.Load<AudioClip>($"Sounds/{name} (Title)");
            image = Resources.Load<Sprite>($"MusicImage/{name}");
            blurImage = Resources.Load<Sprite>($"MusicImage/Blur/{name}");
        }

        public void Update() {
            try {
                var globalNoteData = Json.LoadJsonFile<GlobalNoteData>($"Assets/Data/Map/Keypad/{name}");
                keypadNoteData = globalNoteData.data;
            } catch (FileNotFoundException) {
                keypadNoteData = null;
            }
            try {
                var globalNoteData = Json.LoadJsonFile<GlobalNoteData>($"Assets/Data/Map/Quad/{name}");
                quadNoteData = globalNoteData.data;
            } catch (FileNotFoundException) {
                quadNoteData = null;
            }
        }

        public Color GetDifficultyColor() => _difficultyColors[Math.Min(difficulty / 5, 5)];

        public NoteData[] GetNoteData(GameMode gameMode) => gameMode == GameMode.Keypad ? keypadNoteData : quadNoteData;

        public List<LiveNoteData> ParseLiveNoteData(GameMode gameMode) {
            Update();
            return new List<LiveNoteData>(from data in GetNoteData(gameMode) select new LiveNoteData(data));
        }
    }
}