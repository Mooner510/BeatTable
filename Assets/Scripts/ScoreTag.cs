using UnityEngine;
using UnityEngine.UI;

public class ScoreTag : MonoBehaviour {
    private Text _text;
    private void Start() {
        _text = GetComponent<Text>();
        
    }

    private void FixedUpdate() {
        transform.position += Vector3.up * 1.2f;
        var color = _text.color;
        color.a -= Time.fixedDeltaTime;
        _text.color = color;
        
        if(color.a <= 0) Destroy(gameObject);
    }
}