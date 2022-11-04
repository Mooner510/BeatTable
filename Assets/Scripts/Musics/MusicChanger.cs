using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Musics {
    public class MusicChanger : SingleMono<MusicChanger> {
        [SerializeField] private Image image;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text title;
        [SerializeField] private Text left;
        [SerializeField] private Text right;
        [SerializeField] private Text artist;
        [SerializeField] private Text artistInfo;
        [SerializeField] private Text arranger;
        [SerializeField] private Text arrangeInfo;
        [SerializeField] private Text duration;
        [SerializeField] private Text durationInfo;
        [SerializeField] private Text clickQInfo;
        [SerializeField] private Text clickRInfo;
        [SerializeField] private AudioSource audioPlayer;

        #region Privates
        private Image _subImage;
        private Text _subTitle;
        #endregion

        #region Location
        private static readonly Vector3 ImageLocation = new Vector3(0, 45);
        private static readonly Vector3 LeftImageLocation = new Vector3(-650, 45);
        private static readonly Vector3 RightImageLocation = new Vector3(650, 45);
        private static readonly Vector3 TitleLocation = new Vector3(0, -100);
        private static readonly Vector3 LeftTitleLocation = new Vector3(-650, -100);
        private static readonly Vector3 RightTitleLocation = new Vector3(650, -100);

        private const float UIOut = -630f;
        private const float TextOut = UIOut + 138.76f;
        private const float ClickQOut = -250f;
        private const float ClickROut = ClickQOut - 22f;
        private const float LeftOut = -440f;
        private const float RightOut = 420f;
        #endregion

        private void Start() {
            var musicData = MusicManager.Instance.GetCurrentMusicData();

            _subImage = Instantiate(image, ImageLocation, Quaternion.identity);
            _subImage.transform.SetParent(Utils.Canvas.transform, false);
            _subImage.sprite = musicData.image;
            
            _subTitle = Instantiate(title, TitleLocation, Quaternion.identity);
            _subTitle.transform.SetParent(Utils.Canvas.transform, false);
            _subTitle.text = musicData.name;
            
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
            if(currentImage != null) currentImage.transform.DOLocalMove(isLeft ? RightImageLocation : LeftImageLocation, 1f).SetEase(Ease.OutCubic);
            if(currentTitle != null) currentTitle.transform.DOLocalMove(isLeft ? RightTitleLocation : LeftTitleLocation, 1f).SetEase(Ease.OutCubic);
            backgroundImage.sprite = musicData.image;

            artist.text = musicData.artist;
            if (musicData.arrange == null) {
                arranger.enabled = false;
                arrangeInfo.enabled = false;
            } else {
                arranger.text = musicData.arrange;
                arranger.enabled = true;
                arrangeInfo.enabled = true;
            }

            duration.text = $"{musicData.minute}:{musicData.second}";

            audioPlayer.Stop();
            audioPlayer.clip = musicData.titleAudio;
            audioPlayer.Play();
        }

        private IEnumerator MoveLeft() {
            var musicData = MusicManager.Instance.Back();
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, LeftImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, LeftTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(Utils.Canvas.transform, false);
            newTitle.transform.SetParent(Utils.Canvas.transform, false);
            
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
            newImage.transform.SetParent(Utils.Canvas.transform, false);
            newTitle.transform.SetParent(Utils.Canvas.transform, false);
            
            _subImage = newImage;
            _subTitle = newTitle;
            
            TextUpdate(newImage, newTitle, currentImage, currentTitle, false, musicData);
            
            yield return new WaitForSeconds(1f);
            
            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
        }

        private IEnumerator StartMusic() {
            artistInfo.transform.DOLocalMoveX(UIOut, 2);
            arrangeInfo.transform.DOLocalMoveX(UIOut, 2);
            durationInfo.transform.DOLocalMoveX(UIOut, 2);
            artist.transform.DOLocalMoveX(TextOut, 2);  
            arranger.transform.DOLocalMoveX(TextOut, 2);  
            duration.transform.DOLocalMoveX(TextOut, 2);
            clickQInfo.transform.DOLocalMoveY(ClickQOut, 2);
            clickRInfo.transform.DOLocalMoveY(ClickROut, 2);
            left.transform.DOLocalMoveX(LeftOut, 2);
            right.transform.DOLocalMoveX(RightOut, 2);
            _subImage.transform.DOLocalMove(Vector3.zero, 2);
            _subImage.transform.DOScale(1.5f, 2);
            yield break;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                StopCoroutine(MoveLeft());
                StartCoroutine(MoveLeft());
            } else if (Input.GetKeyDown(KeyCode.D)) {
                StopCoroutine(MoveRight());
                StartCoroutine(MoveRight());
            } else if (Input.GetKeyDown(KeyCode.Q)) {
                StopCoroutine(StartMusic());
                StartCoroutine(StartMusic());
            }
        }
    }
}