﻿using System;
using UnityEngine;

namespace PlayerData.Entity {
    [Serializable]
    public class UserData {
        public KeyData keyData = new KeyData();
        public NoteData noteData = new NoteData();
    }

    [Serializable]
    public class KeyData {
        public KeyCode[] keypadKey = {
            KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
            KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3
        };
        public KeyCode[] quadKey = {
            KeyCode.R, KeyCode.I, KeyCode.F, KeyCode.J
        };
    }

    [Serializable]
    public class NoteData {
        public float noteSpeed = 0.5f;
    }
}