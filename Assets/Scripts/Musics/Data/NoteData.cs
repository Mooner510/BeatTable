using System;

namespace Musics.Data {
    [Serializable]
    public class GlobalNoteData {
        public NoteData[] data;

        public GlobalNoteData(NoteData[] data) => this.data = data;
    }
    
    [Serializable]
    public class NoteData {
        public float time;
        public int note;

        public NoteData(float time, int note) {
            this.time = time;
            this.note = note;
        }

        public override string ToString() => $"{{time: {time}, note: {note}}}";
    }
    
    [Serializable]
    public class LiveNoteData {
        public float time;
        public int note;
        public bool clicked;

        public LiveNoteData(float time, int note) {
            this.time = time;
            this.note = note;
            clicked = false;
        }

        public LiveNoteData(NoteData data) {
            time = data.time;
            note = data.note;
            clicked = false;
        }
        public void Click() => clicked = true;
    }
}