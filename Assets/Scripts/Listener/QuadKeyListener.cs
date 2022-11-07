using System.Collections.Generic;
using Musics.Data;
using UnityEngine;

namespace Listener {
    public class QuadKeyListener : KeyListener {
        protected override void SetUp() {
            KeyCodes = PlayerData.PlayerData.Instance.GetUserData().keyData.quadKey;
            Debug.Log(KeyCodes);
            NoteQueue = new Queue<LiveNoteData>[4];
            for (var i = 0; i < 4; i++) NoteQueue[i] = new Queue<LiveNoteData>();
        }
    }
}