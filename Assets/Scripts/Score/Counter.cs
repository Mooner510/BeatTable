using System.Collections;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace Score {
    public class Counter : SingleMono<Counter> {
        [SerializeField] private Text comboText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text perfectText;
        // [SerializeField] private Text[] texts;
        private int _combo;
        private double _score;
        private int _total;
        private int[] _beforeSize;
        private int[] _data;

        public void Start() {
            _score = 0d;
            _total = 0;
            _data = new int[5];
            comboText.text = "";
            scoreText.text = "0";
            perfectText.text = "100.00%";
            _beforeSize = new[] {comboText.fontSize, scoreText.fontSize, perfectText.fontSize};
        }

        private IEnumerator Sizing(Text text, int index) {
            var before = _beforeSize[index];
            for (var i = 0f; i <= 0.5f; i += Time.deltaTime) {
                yield return null;
                text.fontSize = (int) (before * (1 - i / 4));
            }
            text.transform.localScale = Vector3.one;
        }

        public void Count(ScoreType scoreType) {
            _total++;
            var value = (int) scoreType;
            
            if (value < 2) _combo++;
            else _combo = 0;

            _data[(int) scoreType]++;
            comboText.text = _combo > 0 ? $"{_combo}" : "";
            
            if(scoreType == ScoreType.Miss) return;
            scoreText.text = $"{_score += scoreType.GetBaseScore() * (1 + _combo / 50d):n0}";
            perfectText.text = $"{_data[0] * 100f / _total:F2}%";
            
            StopCoroutine(Sizing(comboText, 0));
            StartCoroutine(Sizing(comboText, 0));

            StopCoroutine(Sizing(scoreText, 1));
            StartCoroutine(Sizing(scoreText, 1));
            
            StopCoroutine(Sizing(perfectText, 2));
            StartCoroutine(Sizing(perfectText, 2));

            StopCoroutine(MapMaker.Instance.Beat());
            StartCoroutine(MapMaker.Instance.Beat());
        }
    }
}