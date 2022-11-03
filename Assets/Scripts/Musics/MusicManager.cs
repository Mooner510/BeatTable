using System.Collections;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Musics {
    public class MusicManager : SingleMono<MusicManager> {
        [SerializeField] private Sprite[] images;
        [SerializeField] private Image image;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text title;
        
        private int _index;
        private Image _subImage;
        private Text _subTitle;
        private bool _moving;

        private static readonly Vector3 ImageLocation = new Vector3(0, 45);
        private static readonly Vector3 LeftImageLocation = new Vector3(-650, 45);
        private static readonly Vector3 RightImageLocation = new Vector3(650, 45);
        private static readonly Vector3 TitleLocation = new Vector3(0, -100);
        private static readonly Vector3 LeftTitleLocation = new Vector3(-650, -100);
        private static readonly Vector3 RightTitleLocation = new Vector3(650, -100);

        private void Start() {
            _index = 0;
            _moving = false;
            
            _subImage = Instantiate(image, ImageLocation, Quaternion.identity);
            _subImage.transform.SetParent(Utils.Canvas.transform, false);
            _subImage.sprite = images[_index];
            
            _subTitle = Instantiate(title, TitleLocation, Quaternion.identity);
            _subTitle.transform.SetParent(Utils.Canvas.transform, false);
            _subTitle.text = images[_index].name;

            backgroundImage.sprite = images[_index];
        }

        private IEnumerator MoveLeft() {
            if(_moving || _index <= 0) yield break;
            _index--;
            _moving = true;
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, LeftImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, LeftTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(Utils.Canvas.transform, false);
            newTitle.transform.SetParent(Utils.Canvas.transform, false);

            DataLoader.SetMusicName(newTitle.text = (backgroundImage.sprite = newImage.sprite = images[_index]).name);
            
            var multiply = 1f;
            for (var i = 0f; i <= 1; i += Time.fixedDeltaTime) {
                // 이동할 위치 + (현재 위치 - 이동할 위치) * 임계값
                currentImage.transform.localPosition = RightImageLocation + (ImageLocation - RightImageLocation) * multiply;
                currentTitle.transform.localPosition = RightTitleLocation + (TitleLocation - RightTitleLocation) * multiply;
                newImage.transform.localPosition = ImageLocation + (LeftImageLocation - ImageLocation) * multiply;
                newTitle.transform.localPosition = TitleLocation + (LeftTitleLocation - TitleLocation) * multiply;
                multiply *= 0.92f;
                yield return new WaitForFixedUpdate();
            }

            newImage.transform.localPosition = ImageLocation;
            newTitle.transform.localPosition = TitleLocation;
            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
            _subImage = newImage;
            _subTitle = newTitle;
            _moving = false;
        }

        private IEnumerator MoveRight() {
            if(_moving || _index >= images.Length - 1) yield break;
            _index++;
            _moving = true;
            var currentImage = _subImage;
            var currentTitle = _subTitle;
            var newImage = Instantiate(image, RightImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, RightTitleLocation, Quaternion.identity);
            newImage.transform.SetParent(Utils.Canvas.transform, false);
            newTitle.transform.SetParent(Utils.Canvas.transform, false);
            
            DataLoader.SetMusicName(newTitle.text = (backgroundImage.sprite = newImage.sprite = images[_index]).name);
            
            var multiply = 1f;
            for (var i = 0f; i <= 1; i += Time.fixedDeltaTime) {
                currentImage.transform.localPosition = LeftImageLocation + (ImageLocation - LeftImageLocation) * multiply;
                currentTitle.transform.localPosition = LeftTitleLocation + (TitleLocation - LeftTitleLocation) * multiply;
                newImage.transform.localPosition = ImageLocation + (RightImageLocation - ImageLocation) * multiply;
                newTitle.transform.localPosition = TitleLocation + (RightTitleLocation - TitleLocation) * multiply;
                multiply *= 0.92f;
                yield return new WaitForFixedUpdate();
            }

            newImage.transform.localPosition = ImageLocation;
            newTitle.transform.localPosition = TitleLocation;
            Destroy(currentImage.gameObject);
            Destroy(currentTitle.gameObject);
            _subImage = newImage;
            _subTitle = newTitle;
            _moving = false;
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