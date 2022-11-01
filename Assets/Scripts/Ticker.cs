﻿using System.Collections;
using Data;
using UnityEngine;

public class Ticker : SingleMono<Ticker> {
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource musicSound;
    [SerializeField] private AudioSource beatSound;

    private bool _readTick;
    private float _writeTime;

    public void Beat() {
        // if (_beat) return;
        beatSound.PlayOneShot(beatSound.clip);
        // _beat = true;
    }

    public float GetStartTime() => _writeTime;

    public float GetPlayTime() => Time.realtimeSinceStartup - _writeTime;

    public void Write() {
        Debug.Log("Write Start");
        _writeTime = Time.realtimeSinceStartup;
        _readTick = true;
        // _routine = StartCoroutine(ReadTick());
        musicSound.PlayOneShot(musicSound.clip);
    }

    public void StopWrite() {
        Debug.Log("Write Stop");
        if(!_readTick) return;
        _readTick = false;
        // if (_routine == null) return;
        // StopCoroutine(_routine);
        musicSound.Stop();
    }

    private void Update() {
        if (!_readTick) return;
        if (!Player.Instance.IsPlay() || DataLoader.IsTop(0)) return;
        var now = GetPlayTime();
        var i = 0;
        do {
            var note = DataLoader.Pick(i);
            if (note.time <= now + 1) {
                // Debug.Log($"Tick: {note.time}");
                StartCoroutine(Player.Instance.Accept(DataLoader.Pop(), note.time - (now + 1)));
            } else break;
        } while (!DataLoader.IsTop(++i));
    }

    private IEnumerator ReadTick() {
        while (true) {
            // yield return _seconds;

            if (!Player.Instance.IsPlay() || DataLoader.IsTop(0)) continue;
            var now = GetPlayTime();
            var i = 0;
            do {
                var note = DataLoader.Pick(i);
                if (note.time <= now + 1) {
                    // Debug.Log($"Tick: {note.time}");
                    StartCoroutine(Player.Instance.Accept(DataLoader.Pop(), note.time - (now + 1)));
                }
            } while (!DataLoader.IsTop(++i));
        }
        // ReSharper disable once FunctionNeverReturns
    }
}