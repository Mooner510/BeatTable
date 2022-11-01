using UnityEngine;

public static class Utils {
    
    public static Vector2 Locator(int n) => new Vector2(-3 + 3 * (n % 3), 3 - 3 * (n / 3));

    private static RectTransform _canvas;

    public static RectTransform Canvas => _canvas ??= GameObject.Find("Canvas").GetComponent<RectTransform>();
    
    public static Vector2 LocationToCanvas(Vector2 vector) {
        if (Camera.main == null) return new Vector2();
        Vector2 viewPosition = Camera.main.WorldToViewportPoint(vector);
        if (_canvas == null) _canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        var delta = _canvas.sizeDelta;
        return new Vector2(viewPosition.x * delta.x - delta.x / 2, viewPosition.y * delta.y - delta.y / 2);
    }
    
    public static Vector2 TransferCanvasLocation(Vector2 vector) => new Vector2(vector.x + 366, vector.y + 305.8f);
}