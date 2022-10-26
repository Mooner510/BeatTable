using System.Collections;
using Data;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance;
    
    [SerializeField] public bool isPlay;
    [SerializeField] private GameObject beatInspector;

    private void Start() {
        Instance = this;
        if (isPlay) DataLoader.LoadData();
        StartCoroutine(Init());
    }

    private static IEnumerator Init() {
        yield return new WaitForSeconds(3);
        DataLoader.Instance.Start();
    }

    public IEnumerator Accept(LiveNoteData note, float time) {
        var colored = false;
        yield return new WaitForSeconds(time);
        KeyBinder.Instance.Queue(note);
        var o = Instantiate(beatInspector, Utils.Locator(note.note), Quaternion.identity);
        for (var delta = 0f; delta <= 1.25; delta += Time.deltaTime) {
            yield return null;
            o.transform.localScale = new Vector3(delta * 2, delta * 2, delta * 2);
            if (colored || !(delta >= 0.9f)) continue;
            o.transform.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 0.3f);
            colored = true;
        }
        Destroy(o);
    }
}