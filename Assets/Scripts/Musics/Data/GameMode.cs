using System;
using UnityEngine;

namespace Musics.Data {
    public enum GameMode {
        Keypad,
        Quad
    }

    public static class GameModeExtension {
        public static Color GetColor(this GameMode gameMode) {
            return gameMode switch {
                GameMode.Keypad => new Color(0.572549f, 1f, 1f),
                GameMode.Quad => new Color(0.672549f, 1f, 0.572549f),
                _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
            };
        }
    }
}