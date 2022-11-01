using UnityEngine;
using UnityEngine.UI;

public class AppearText : MonoBehaviour {
    private Image _image;
    private void Start() {
        _image = GetComponent<Image>();
    }

    private void FixedUpdate() {
        transform.position += Vector3.up * 1.2f;
        var color = _image.color;
        color.a -= Time.fixedDeltaTime * 1.5f;
        _image.color = color;
        
        if(color.a <= 0) Destroy(gameObject);
    }
}