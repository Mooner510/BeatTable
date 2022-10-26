using System;
using UnityEngine;

public enum Score {
    Perfect,
    Great,
    Good,
    Bad,
    Miss
}

public static class ScoreExtensions {
    public static string GetTag(this Score score) {
        return score switch {
            Score.Perfect => "PERFECT!",
            Score.Great => "GREAT",
            Score.Good => "GOOD",
            Score.Bad => "BAD",
            Score.Miss => "MISS",
            _ => throw new ArgumentOutOfRangeException(nameof(score), score, null)
        };
    }

    private static readonly Color Perfect = new Color(0.6f, 1f, 1f, 1f);
    private static readonly Color Great = new Color(0.4f, 1f, 0.4f, 1f);
    private static readonly Color Good = new Color(0.3f, 0.6f, 0.9f, 1f);
    private static readonly Color Bad = new Color(1f, 0.27f, 0.27f, 1f);
    private static readonly Color Miss = new Color(0.6f, 0.6f, 0.6f, 1f);
    
    public static Color GetColor(this Score score) {
        return score switch {
            Score.Perfect => Perfect,
            Score.Great => Great,
            Score.Good => Good,
            Score.Bad => Bad,
            Score.Miss => Miss,
            _ => throw new ArgumentOutOfRangeException(nameof(score), score, null)
        };
    }
}