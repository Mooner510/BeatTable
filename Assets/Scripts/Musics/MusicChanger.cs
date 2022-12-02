using System;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using Musics.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Musics {
    public class MusicChanger : SingleMono<MusicChanger> {
        [SerializeField] private Image image;
        [SerializeField] private Text title;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text left;
        [SerializeField] private Text right;
        [SerializeField] private Text artist;
        [SerializeField] private Text artistInfo;
        [SerializeField] private Text arranger;
        [SerializeField] private Text arrangeInfo;
        [SerializeField] private Text duration;
        [SerializeField] private Text durationInfo;
        [SerializeField] private Text difficulty;
        [SerializeField] private Text difficultyInfo;
        [SerializeField] private Text suggestion1;
        [SerializeField] private Text suggestion2;
        [SerializeField] private Text suggestion3;
        [SerializeField] private Text selectorUp;
        [SerializeField] private Text selectorDown;
        [SerializeField] private Text modeText;
        [SerializeField] private SpriteRenderer hider;
        [SerializeField] private AudioSource audioPlayer;

        #region Location

        private static readonly Vector3 ImageLocation = new Vector3(0, 45);
        private static readonly Vector3 LeftImageLocation = new Vector3(-650, 45);
        private static readonly Vector3 RightImageLocation = new Vector3(650, 45);
        private static readonly Vector3 TitleLocation = new Vector3(0, -100);
        private static readonly Vector3 LeftTitleLocation = new Vector3(-650, -100);
        private static readonly Vector3 RightTitleLocation = new Vector3(650, -100);

        private const float UIOut = -630f;
        private const float TextOut = UIOut + 138.76f;
        private const float Suggest1Out = -300f;
        private const float Suggest2Out = Suggest1Out - 22f;
        private const float Suggest3Out = Suggest2Out - 17f;
        private const float TitleOut = Suggest1Out + 35f;
        private const float LeftOut = -440f;
        private const float RightOut = 420f;
        private const float SelectorOut = 460f;

        #endregion

        #region Privates

        private Image _subImage;
        private Text _subTitle;
        private bool _canStart;
        private Sequence _clickSequence;
        private GameMode _mode;

        #endregion

        private void Start() {
            hider.color = Color.clear;
            audioPlayer.volume = 1;
            _canStart = false;
            _clickSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(() => suggestion2.transform.localScale = Vector3.one * 1.2f)
                .Append(suggestion2.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutCubic));

            MusicManager.Instance.UpdateCurrentMusicData();
            var musicData = MusicManager.Instance.GetCurrentMusicData();
            
            _subImage = Instantiate(image, ImageLocation, Quaternion.identity);
            if (_subImage.transform != null) {
                _subImage.transform.SetParent(GameUtils.Canvas, false);
                _subImage.sprite = musicData.image;
            }

            _subTitle = Instantiate(title, TitleLocation, Quaternion.identity);
            if (_subTitle != null) {
                _subTitle.transform.SetParent(GameUtils.Canvas, false);
                _subTitle.text = musicData.name;
            }
            
            _mode = GameMode.Keypad;
            modeText.color = _mode.GetColor();
            modeText.text = $"{_mode.ToString()} Mode";

            TextUpdate(null, null, null, null, false, musicData);
        }

        private void TextUpdate([CanBeNull] Image newImage, [CanBeNull] Text newTitle, [CanBeNull] Component currentImage, [CanBeNull] Component currentTitle, bool isLeft, MusicData musicData) {
            if (newImage != null) {
                newImage.sprite = musicData.image;
                newImage.transform.DOLocalMove(ImageLocation, 1f).SetEase(Ease.OutCubic);
            }

            if (newTitle != null) {
                newTitle.text = musicData.name;
                newTitle.transform.DOLocalMove(TitleLocation, 1f).SetEase(Ease.OutCubic);
            }

            if (currentImage != null)
                currentImage.transform.DOLocalMove(isLeft ? RightImageLocation : LeftImageLocation, 1f)
                    .SetEase(Ease.OutCubic);
            if (currentTitle != null)
                currentTitle.transform.DOLocalMove(isLeft ? RightTitleLocation : LeftTitleLocation, 1f)
                    .SetEase(Ease.OutCubic);
            backgroundImage.sprite = musicData.blurImage;

            artist.text = musicData.artist;
            if (musicData.arrange == null) {
                arranger.enabled = false;
                arrangeInfo.enabled = false;
            } else {
                arranger.text = musicData.arrange;
                arranger.enabled = true;
                arrangeInfo.enabled = true;
            }

            UpdateSuggestion(musicData);

            duration.text = $"{musicData.minute}:{musicData.second:00}";
            difficulty.text = $"{StringUtils.ToRoman(musicData.difficulty)}";
            difficulty.color = musicData.GetDifficultyColor();

            audioPlayer.Stop();
            audioPlayer.clip = musicData.titleAudio;
            audioPlayer.Play();
        }

        private void UpdateSuggestion(MusicData musicData) {
            var noteData = musicData.GetNoteData(_mode);
            if (noteData == null || noteData.Length <= 0) {
                suggestion1.text = "Press E to Record";
                suggestion2.text = "This music doesn't have note map!";
                _canStart = false;
            } else {
                suggestion1.text = "Press Q / Enter to Start";
                suggestion2.text = "Or Press E to Edit";
                _canStart = true;
            }
        }

        private IEnumerator MoveLeft() {
            var musicData = MusicManager.Instance.Back();
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, LeftImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, LeftTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(GameUtils.Canvas.transform, false);
            newTitle.transform.SetParent(GameUtils.Canvas.transform, false);

            _subImage = newImage;
            _subTitle = newTitle;

            TextUpdate(newImage, newTitle, currentImage, currentTitle, true, musicData);

            yield return new WaitForSeconds(1f);

            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
        }

        private IEnumerator MoveRight() {
            var musicData = MusicManager.Instance.Next();
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, RightImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, RightTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(GameUtils.Canvas.transform, false);
            newTitle.transform.SetParent(GameUtils.Canvas.transform, false);

            _subImage = newImage;
            _subTitle = newTitle;

            TextUpdate(newImage, newTitle, currentImage, currentTitle, false, musicData);

            yield return new WaitForSeconds(1f);

            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
        }

        private IEnumerator StartMusic(GameMode gameMode) {
            artistInfo.transform.DOLocalMoveX(UIOut, 2).SetEase(Ease.OutCubic);
            arrangeInfo.transform.DOLocalMoveX(UIOut, 2).SetEase(Ease.OutCubic);
            durationInfo.transform.DOLocalMoveX(UIOut, 2).SetEase(Ease.OutCubic);
            difficultyInfo.transform.DOLocalMoveX(UIOut, 2).SetEase(Ease.OutCubic);
            artist.transform.DOLocalMoveX(TextOut, 2).SetEase(Ease.OutCubic);
            arranger.transform.DOLocalMoveX(TextOut, 2).SetEase(Ease.OutCubic);
            duration.transform.DOLocalMoveX(TextOut, 2).SetEase(Ease.OutCubic);
            difficulty.transform.DOLocalMoveX(TextOut, 2).SetEase(Ease.OutCubic);
            suggestion1.transform.DOLocalMoveY(Suggest1Out, 2).SetEase(Ease.OutCubic);
            suggestion2.transform.DOLocalMoveY(Suggest2Out, 2).SetEase(Ease.OutCubic);
            suggestion3.transform.DOLocalMoveY(Suggest3Out, 2).SetEase(Ease.OutCubic);
            selectorUp.transform.DOLocalMoveX(SelectorOut, 2).SetEase(Ease.OutCubic);
            selectorDown.transform.DOLocalMoveX(SelectorOut, 2).SetEase(Ease.OutCubic);
            modeText.transform.DOLocalMoveX(SelectorOut, 2).SetEase(Ease.OutCubic);
            difficultyInfo.transform.DOLocalMoveX(UIOut, 2).SetEase(Ease.OutCubic);
            left.transform.DOLocalMoveX(LeftOut, 2).SetEase(Ease.OutCubic);
            right.transform.DOLocalMoveX(RightOut, 2).SetEase(Ease.OutCubic);
            _subImage.transform.DOLocalMove(Vector3.zero, 3).SetEase(Ease.OutCubic);
            _subImage.transform.DOScale(1.75f, 3).SetEase(Ease.OutCubic);
            _subTitle.transform.DOLocalMoveY(TitleOut, 2).SetEase(Ease.OutCubic);
            audioPlayer.DOFade(0, 3);
            yield return new WaitForSecondsRealtime(1);
            hider.DOColor(Color.black, 2).SetEase(Ease.OutCubic);
            yield return new WaitForSecondsRealtime(3);
            switch (gameMode) {
                case GameMode.Keypad:
                    SceneManager.LoadScene(1);
                    break;
                case GameMode.Quad:
                    SceneManager.LoadScene(2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                StopCoroutine(MoveLeft());
                StartCoroutine(MoveLeft());
            } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                StopCoroutine(MoveRight());
                StartCoroutine(MoveRight());
            } else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                _mode = _mode == GameMode.Keypad ? GameMode.Quad : GameMode.Keypad;
                modeText.color = _mode.GetColor();
                modeText.text = $"{_mode.ToString()} Mode";
                UpdateSuggestion(MusicManager.Instance.GetCurrentMusicData());
                MusicManager.SetGameMode(_mode);
            } else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Return)) {
                if (!_canStart) {
                    _clickSequence.Restart();
                    return;
                }
                MusicManager.Instance.SetPlayMode(true);
                MusicManager.Instance.SetPlayMode(true);
                StopCoroutine(StartMusic(_mode));
                StartCoroutine(StartMusic(_mode));
            } else if (Input.GetKeyDown(KeyCode.E)) {
                MusicManager.Instance.SetPlayMode(false);
                StopCoroutine(StartMusic(_mode));
                StartCoroutine(StartMusic(_mode));
            }
        }
    }
}