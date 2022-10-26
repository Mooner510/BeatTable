using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Map;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinder : MonoBehaviour {
    public static KeyBinder Instance;
    
    [SerializeField] private Text scoreTag;
    
    private static readonly KeyCode[] KeyCodes = {
        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3
    };

    private KeyCode _code;

    private Queue<LiveNoteData>[] _noteQueue;

    private void Start() {
        Instance = this;
        _noteQueue = new Queue<LiveNoteData>[9];
        for (var i = 0; i < 9; i++) _noteQueue[i] = new Queue<LiveNoteData>();
    }

    public void Update() {
        if (!Player.Instance.isPlay && Input.GetKey(KeyCode.Backspace)) {
            DataLoader.Instance.Stop();
            return;
        }
        for (var i = 0; i < 9; i++) {
            _code = KeyCodes[i];
            if (!Input.GetKeyDown(_code)) continue;
            // Debug.Log($"id: {i}");
            MapMaker.Instance.Click(i);
            Ticker.Instance.Beat();
            if (!Player.Instance.isPlay) {
                DataLoader.AddNote(i);
            } else {
                LiveNoteData liveNoteData = null;
                while (_noteQueue[i].Count > 0 && liveNoteData == null) {
                    liveNoteData = _noteQueue[i].Dequeue();
                    if (liveNoteData.clicked) liveNoteData = null;
                }
                if(liveNoteData == null) return;
                var ticker = Ticker.Instance;
                liveNoteData.Click();
                var diff = Math.Abs(liveNoteData.time - ticker.GetPlayTime() + 1f);
                Debug.Log($"{liveNoteData.time}: {diff}s");
                if (diff <= 0.1f) {
                    Spawn(liveNoteData, Score.Perfect);
                } else if (diff <= 0.2f) {
                    Spawn(liveNoteData, Score.Great);
                } else if (diff <= 0.325f) {
                    Spawn(liveNoteData, Score.Good);
                } else if (diff <= 0.45f) {
                    Spawn(liveNoteData, Score.Bad);
                } else {
                    Spawn(liveNoteData, Score.Miss);
                }
            }
        }
    }

    private void Spawn(LiveNoteData data, Score score) {
        Debug.Log($"{data.time}, {score.GetTag()}");
        var o = Instantiate(scoreTag, Utils.LocationToCanvas(Utils.Locator(data.note)), Quaternion.identity);
        o.transform.SetParent(Ticker.Instance.canvas.transform, false);
        var text = o.GetComponent<Text>();
        text.text = score.GetTag();
        text.color = score.GetColor();
    }

    public void Queue(LiveNoteData data) => StartCoroutine(Enqueue(data));

    private IEnumerator Enqueue(LiveNoteData data) {
        yield return new WaitForSeconds(1f);
        _noteQueue[data.note].Enqueue(data);
        yield return new WaitForSeconds(0.5f);
        if (data.clicked) yield break;
        data.Click();
        Spawn(data, Score.Miss);
    }
}