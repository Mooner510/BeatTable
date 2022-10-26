using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class Ticker : SingleMono<Ticker> {
    private long _tick;
    [SerializeField] private float bpm;
    [SerializeField] private Text beatText;
    [SerializeField] private AudioSource musicSound;
    [SerializeField] private AudioSource beatSound;

    public RectTransform canvas;
    public Camera mainCamera;
    private bool _readTick;
    private float _timePerTick;
    // private WaitForSeconds _seconds;
    // private Coroutine _routine;
    private float _writeTime;

    private void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _tick = 0;
        _timePerTick = 15f / bpm;
        // _seconds = new WaitForSeconds(_timePerTick);
    }

    public void Beat() {
        // if (_beat) return;
        beatSound.PlayOneShot(beatSound.clip);
        // _beat = true;
    }

    public float GetTimePerTick() => _timePerTick;

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
        if (!Player.Instance.isPlay || DataLoader.IsTop(0)) return;
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

            if (!Player.Instance.isPlay || DataLoader.IsTop(0)) continue;
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
        // ReSharper disable once IteratorNeverReturns
    }

    // private void FixedUpdate() {
    //     _tick++;
    // }

    public void ResetTick() {
        Debug.Log("Tick Reset");
        _tick = 0L;
    }

    public long GetTick() => _tick;
}