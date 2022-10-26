using UnityEngine;

public static class Utils {
    public static Vector2 Locator(int n) {
        return new Vector2(-3 + 3 * (n % 3), 3 - 3 * (n / 3));
    }

    public static Vector2 LocationToCanvas(Vector2 vector) {
        Vector2 viewPosition = Ticker.Instance.mainCamera.WorldToViewportPoint(vector);
        var canvas = Ticker.Instance.canvas;
        var delta = canvas.sizeDelta;
        return new Vector2(viewPosition.x * delta.x - delta.x / 2, viewPosition.y * delta.y - delta.y / 2);
    }
}