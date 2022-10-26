using System;

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
}