using UnityEngine;
using UnityEngine.UI;

public class ScoreTag : MonoBehaviour {
    private Text _text;
    private void Start() {
        _text = GetComponent<Text>();
        
    }

    private void FixedUpdate() {
        transform.position += Vector3.up;
        var color = _text.color;
        color.a -= 0.005f;
        _text.color = color;
        
        if(color.a <= 0) Destroy(gameObject);
    }
}