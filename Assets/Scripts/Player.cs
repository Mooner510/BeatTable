using System.Collections;
using Data;
using Map;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance;
    
    [SerializeField] private bool isPlay;
    [SerializeField] private GameObject beatInspector;

    private static readonly Color ClickColor = new Color(0f, 0.8f, 0.8f, 0.3f);

    private void Start() {
        Instance = this;
        if (isPlay) DataLoader.LoadData();
        StartCoroutine(Init());
    }

    public bool IsPlay() => isPlay;

    public void SetPlay(bool play) => isPlay = play;

    private static IEnumerator Init() {
        yield return new WaitForSeconds(3);
        DataLoader.Start();
    }

    public IEnumerator Accept(LiveNoteData note, float time) {
        var colored = false;
        yield return new WaitForSeconds(time);
        KeyListener.Instance.Queue(note);
        var obj = Instantiate(beatInspector, Utils.Locator(note.note), Quaternion.identity);
        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        for (var delta = 0f; delta <= 1.25; delta += Time.deltaTime) {
            yield return null;
            obj.transform.position = MapMaker.Instance.GetNote(note.note).transform.position;
            obj.transform.localScale = new Vector3(delta * 2, delta * 2, delta * 2);
            if (colored || !(delta >= 0.9f)) continue;
            spriteRenderer.color = ClickColor;
            colored = true;
        }
        Destroy(obj);
    }
}