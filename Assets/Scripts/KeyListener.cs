using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Map;
using Musics;
using Musics.Data;
using Score;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class KeyListener : MonoBehaviour {
    public static KeyListener Instance;

    [SerializeField] private Image scoreImage;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Text increases;

    private static readonly KeyCode[] KeyCodes = {
        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3
    };
    private Sequence _scoreSequence;
    private static readonly Vector3 BeforeScorePos = new Vector3(335f, 144f);
    private static readonly Vector3 ScorePos = new Vector3(346.5f, 144f);

    private KeyCode _code;

    private Queue<LiveNoteData>[] _noteQueue;

    private void Start() {
        Instance = this;
        _noteQueue = new Queue<LiveNoteData>[9];
        for (var i = 0; i < 9; i++) _noteQueue[i] = new Queue<LiveNoteData>();

        increases.text = "";
        _scoreSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() => {
                var trans = increases.transform;
                trans.localPosition = BeforeScorePos;
                trans.localScale = Vector3.one * 1.05f;
                increases.color = GameUtils.ClearWhite;
            })
            .Join(increases.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic))
            .Join(increases.transform.DOLocalMove(ScorePos, 0.5f).SetEase(Ease.OutCubic))
            .Join(increases.DOColor(Color.white, 0.5f).SetEase(Ease.OutCubic))
            .OnComplete(() => increases.DOFade(0, 0.5f).SetEase(Ease.OutCubic));
    }

    public void Update() {
        if (!MusicManager.Instance.IsPlayMode() && Input.GetKeyDown(KeyCode.Backspace)) {
            NoteManager.Stop();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            NoteManager.Stop();
            Player.Instance.Stop();
            return;
        }
        
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
            if (!MusicManager.Instance.IsPlayMode()) {
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
        var obj = Instantiate(scoreImage, GameUtils.LocationToCanvas(GameUtils.Locator(data.note)), Quaternion.identity);
        obj.transform.SetParent(GameUtils.Canvas.transform, false);
        obj.sprite = sprites[(int) score];
        increases.text = $"+{Counter.Instance.Count(score):n0}";
        _scoreSequence.Restart();
    }

    public void Queue(LiveNoteData data) => StartCoroutine(Enqueue(data));

    private IEnumerator Enqueue(LiveNoteData data) {
        _noteQueue[data.note].Enqueue(data);
        yield return new WaitForSeconds(1f);
        if (data.clicked) yield break;
        data.Click();
        Spawn(data, ScoreType.Miss);
        _noteQueue[data.note].Dequeue();
    }
}