using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Musics {
    public class MusicChanger : SingleMono<MusicChanger> {
        [SerializeField] private Image image;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text title;
        [SerializeField] private Text left;
        [SerializeField] private Text right;

        private Image _subImage;
        private Text _subTitle;

        private static readonly Vector3 ImageLocation = new Vector3(0, 45);
        private static readonly Vector3 LeftImageLocation = new Vector3(-650, 45);
        private static readonly Vector3 RightImageLocation = new Vector3(650, 45);
        private static readonly Vector3 TitleLocation = new Vector3(0, -100);
        private static readonly Vector3 LeftTitleLocation = new Vector3(-650, -100);
        private static readonly Vector3 RightTitleLocation = new Vector3(650, -100);

        private void Start() {
            var musicData = MusicManager.Instance.GetCurrentMusicData();

            _subImage = Instantiate(image, ImageLocation, Quaternion.identity);
            _subImage.transform.SetParent(Utils.Canvas.transform, false);
            _subImage.sprite = musicData.image;
            
            _subTitle = Instantiate(title, TitleLocation, Quaternion.identity);
            _subTitle.transform.SetParent(Utils.Canvas.transform, false);
            _subTitle.text = musicData.name;

            backgroundImage.sprite = musicData.image;
        }

        private IEnumerator MoveLeft() {
            var musicData = MusicManager.Instance.Back();
            if (musicData == null) yield break;
            if (MusicManager.Instance.IsLast()) left.enabled = false;
            right.enabled = true;
            
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, LeftImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, LeftTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(Utils.Canvas.transform, false);
            newTitle.transform.SetParent(Utils.Canvas.transform, false);

            newTitle.text = (backgroundImage.sprite = newImage.sprite = musicData.image).name;

            currentImage.transform.DOLocalMove(RightImageLocation, 1f).SetEase(Ease.OutCubic);
            currentTitle.transform.DOLocalMove(RightTitleLocation, 1f).SetEase(Ease.OutCubic);
            newImage.transform.DOLocalMove(ImageLocation, 1f).SetEase(Ease.OutCubic);
            newTitle.transform.DOLocalMove(TitleLocation, 1f).SetEase(Ease.OutCubic);
            
            _subImage = newImage;
            _subTitle = newTitle;

            yield return new WaitForSeconds(1f);

            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
        }

        private IEnumerator MoveRight() {
            var musicData = MusicManager.Instance.Back();
            if (musicData == null) yield break;
            if (MusicManager.Instance.IsFirst()) right.enabled = false;
            left.enabled = true;
            
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, RightImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, RightTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(Utils.Canvas.transform, false);
            newTitle.transform.SetParent(Utils.Canvas.transform, false);
            
            newTitle.text = (backgroundImage.sprite = newImage.sprite = musicData.image).name;
            
            currentImage.transform.DOLocalMove(LeftImageLocation, 1f).SetEase(Ease.OutCubic);
            currentTitle.transform.DOLocalMove(LeftTitleLocation, 1f).SetEase(Ease.OutCubic);
            newImage.transform.DOLocalMove(ImageLocation, 1f).SetEase(Ease.OutCubic);
            newTitle.transform.DOLocalMove(TitleLocation, 1f).SetEase(Ease.OutCubic);
            
            _subImage = newImage;
            _subTitle = newTitle;
            
            yield return new WaitForSeconds(1f);
            
            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                StopCoroutine(MoveLeft());
                StartCoroutine(MoveLeft());
            } else if (Input.GetKeyDown(KeyCode.D)) {
                StopCoroutine(MoveRight());
                StartCoroutine(MoveRight());
            }
        }
    }
}