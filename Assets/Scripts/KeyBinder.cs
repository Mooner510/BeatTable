using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinder : MonoBehaviour {
    public static KeyBinder Instance;
    
    [SerializeField] private Text scoreTag;
    
    private static readonly KeyCode[] KeyCodes = {
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9
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
            if (!Player.Instance.isPlay) {
                DataLoader.AddNote(i);
            } else {
                if (_noteQueue[i].Count <= 0) continue;
                var ticker = Ticker.Instance;
                var liveNoteData = _noteQueue[i].Dequeue();
                liveNoteData.Click();
                var diff = Math.Abs(liveNoteData.time - ticker.GetPlayTime());
                Debug.Log($"{liveNoteData.time}: {diff}s");
                if (diff <= 0.075f) {
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
            Ticker.Instance.Beat();
        }
    }

    private void Spawn(LiveNoteData data, Score score) {
        Debug.Log($"{data.time}, {score.GetTag()}");
        var o = Instantiate(scoreTag, Utils.LocationToCanvas(Utils.Locator(data.note)), Quaternion.identity);
        o.transform.SetParent(Ticker.Instance.canvas.transform, false);
        o.GetComponent<Text>().text = score.GetTag();
    }

    public void Queue(LiveNoteData data) => StartCoroutine(Enqueue(data));

    private IEnumerator Enqueue(LiveNoteData data) {
        yield return new WaitForSeconds(1.5f);
        _noteQueue[data.note].Enqueue(data);
        yield return new WaitForSeconds(1f);
        if (!data.clicked) Spawn(data, Score.Miss);
    }
}