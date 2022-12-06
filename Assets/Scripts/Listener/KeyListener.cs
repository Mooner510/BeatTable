﻿using System;
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

namespace Listener {
    public class KeyListener : SingleMono<KeyListener> {
        
        [SerializeField] protected Image scoreImage;
        [SerializeField] protected Sprite[] sprites;
        [SerializeField] protected Text increases;
        
        private Sequence _scoreSequence;
        private static readonly Vector3 BeforeScorePos = new Vector3(335f, 144f);
        private static readonly Vector3 ScorePos = new Vector3(346.5f, 144f);

        private KeyCode _code;

        protected Queue<LiveNoteData>[] NoteQueue;
        protected KeyCode[] KeyCodes;

        private void Start() {
            SetUp();
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

        protected virtual void SetUp() {
            KeyCodes = PlayerData.PlayerData.Instance.GetUserData().keyData.keypadKey;
            Debug.Log(KeyCodes);
            NoteQueue = new Queue<LiveNoteData>[9];
            for (var i = 0; i < 9; i++) NoteQueue[i] = new Queue<LiveNoteData>();
        }

        public void Update() {
            if (!MusicManager.Instance.IsPlayMode() && Input.GetKeyDown(KeyCode.Backspace)) {
                Player.Instance.Stop(true);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if(!Ticker.Instance.IsTickReading()) return;
                Player.Instance.Stop(false);
                return;
            }
        
            // if (Input.GetKey(KeyCode.R)) {
            //     NoteManager.Stop();
            //     NoteManager.
            //     return;
            // }

            for (var i = 0; i < KeyCodes.Length; i++) {
                _code = KeyCodes[i];
                if (!Input.GetKeyDown(_code)) continue;
                // Debug.Log($"id: {i}");
                MapMaker.Instance.Click(i);
                Ticker.Instance.Beat();
                if (!MusicManager.Instance.IsPlayMode()) {
                    NoteManager.AddNote(i);
                } else {
                    LiveNoteData liveNoteData = null;
                    while (NoteQueue[i].Count > 0 && liveNoteData == null) {
                        liveNoteData = NoteQueue[i].Dequeue();
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
                    } else if (diff <= 0.175f) {
                        Spawn(liveNoteData, ScoreType.Good);
                    } else if (diff <= 0.3f) {
                        Spawn(liveNoteData, ScoreType.Bad);
                    } else {
                        Spawn(liveNoteData, ScoreType.Miss);
                    }
                }
            }
        }

        private void Spawn(LiveNoteData data, ScoreType score) {
            Debug.Log($"{data.time}, {score.GetTag()}");
            var obj = Instantiate(scoreImage, GameUtils.LocationToCanvas(GameUtils.Locator(MusicManager.GetCurrentGameMode(), data.note)), Quaternion.identity);
            obj.transform.SetParent(GameUtils.Canvas.transform, false);
            obj.sprite = sprites[(int) score];
            increases.text = $"+{Counter.Instance.Count(score):n0}";
            _scoreSequence.Restart();
        }

        public void Queue(LiveNoteData data) => StartCoroutine(Enqueue(data));

        public static float NoteTime {
            get {
                var noteSpeed = NoteManager.GetNoteSpeed();
                if (noteSpeed < 8) return 1 + (8 - noteSpeed) * 0.575f;
                if (noteSpeed > 8) return 1 - (noteSpeed - 8) * 0.065f;
                return 1f;
            }
        }
        public static float AllowedTime => Math.Min(NoteTime / 10, 0.3f);

        private IEnumerator Enqueue(LiveNoteData data) {
            while (NoteQueue[data.note].Count > 1) {
                NoteQueue[data.note].Dequeue().Click();
                Spawn(data, ScoreType.Miss);
            }
            NoteQueue[data.note].Enqueue(data);
            yield return new WaitForSeconds(NoteTime - AllowedTime * 2);
            if (data.clicked) yield break;
            data.Click();
            Spawn(data, ScoreType.Miss);
            NoteQueue[data.note].Dequeue();
        }
    }
}