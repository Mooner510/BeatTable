using System;
using System.Collections;
using System.Collections.Generic;
using Map;
using Musics.Data;
using Score;
using UnityEngine;
using UnityEngine.UI;

public class KeyListener : MonoBehaviour {
    public static KeyListener Instance;

    [SerializeField] private Image scoreImage;
    [SerializeField] private Sprite[] sprites;

    private static readonly KeyCode[] KeyCodes = {
        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3
    };

    private KeyCode _code;

    private Queue<LiveNoteData>[] _noteQueue;

    private void Start() {
        Instance = this;
        _noteQueue = new Queue<LiveNoteData>[9];
        for (var i = 0; i < 9; i++) _noteQueue[i] = new Queue<LiveNoteData>();
    }

    public void Update() {
        if (!Player.Instance.IsPlay() && Input.GetKey(KeyCode.Backspace)) {
            NoteManager.Stop();
            return;
        }

        // if (Input.GetKey(KeyCode.Q)) {
        //     NoteManager.Stop();
        //     Player.Instance.SetPlay(!Player.Instance.IsPlay());
        //     return;
        // }
        
        // if (Input.GetKey(KeyCode.R)) {
        //     NoteManager.Stop();
        //     NoteManager.
        //     return;
        // }

        for (var i = 0; i < 9; i++) {
            _code = KeyCodes[i];
            if (!Input.GetKeyDown(_code)) continue;
            // Debug.Log($"id: {i}");
            MapMaker.Instance.Click(i);
            Ticker.Instance.Beat();
            if (!Player.Instance.IsPlay()) {
                NoteManager.AddNote(i);
            } else {
                LiveNoteData liveNoteData = null;
                while (_noteQueue[i].Count > 0 && liveNoteData == null) {
                    liveNoteData = _noteQueue[i].Dequeue();
                    if (liveNoteData.clicked) liveNoteData = null;
                }

                if (liveNoteData == null) return;
                var ticker = Ticker.Instance;
                liveNoteData.Click();
                var diff = Math.Abs(liveNoteData.time - ticker.GetPlayTime());
                Debug.Log($"{liveNoteData.time}: {diff}s");
                if (diff <= 0.07f) {
                    Spawn(liveNoteData, ScoreType.Perfect);
                } else if (diff <= 0.125f) {
                    Spawn(liveNoteData, ScoreType.Great);
                } else if (diff <= 0.25f) {
                    Spawn(liveNoteData, ScoreType.Good);
                } else if (diff <= 0.45f) {
                    Spawn(liveNoteData, ScoreType.Bad);
                } else {
                    Spawn(liveNoteData, ScoreType.Miss);
                }
            }
        }
    }

    private void Spawn(LiveNoteData data, ScoreType score) {
        Debug.Log($"{data.time}, {score.GetTag()}");
        var obj = Instantiate(scoreImage, Utils.LocationToCanvas(Utils.Locator(data.note)), Quaternion.identity);
        obj.transform.SetParent(Utils.Canvas.transform, false);
        obj.sprite = sprites[(int) score];
        Counter.Instance.Count(score);
    }

    public void Queue(LiveNoteData data) => StartCoroutine(Enqueue(data));

    private IEnumerator Enqueue(LiveNoteData data) {
        _noteQueue[data.note].Enqueue(data);
        yield return new WaitForSeconds(1.5f);
        if (data.clicked) yield break;
        data.Click();
        Spawn(data, ScoreType.Miss);
        _noteQueue[data.note].Dequeue();
    }
}