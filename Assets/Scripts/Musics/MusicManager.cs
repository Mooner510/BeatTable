using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Musics {
    public class MusicManager : SingleMono<MusicManager> {
        [SerializeField] private Sprite[] images;
        [SerializeField] private Image image;
        [SerializeField] private Text title;
        
        private int _index;
        private Image _subImage;
        private Text _subTitle;

        private static readonly Vector3 ImageLocation = Utils.LocationToCanvas(new Vector3(0, 45));
        private static readonly Vector3 LeftImageLocation = Utils.LocationToCanvas(new Vector3(-650, 45));
        private static readonly Vector3 RightImageLocation = Utils.LocationToCanvas(new Vector3(650, 45));
        private static readonly Vector3 TitleLocation = Utils.LocationToCanvas(new Vector3(0, -130)); 
        private static readonly Vector3 LeftTitleLocation = Utils.LocationToCanvas(new Vector3(-650, -130)); 
        private static readonly Vector3 RightTitleLocation = Utils.LocationToCanvas(new Vector3(650, -130)); 

        private void Start() {
            _index = 0;
            _subImage = Instantiate(image, ImageLocation, Quaternion.identity);
            _subImage.sprite = images[_index];
            _subTitle = Instantiate(title, TitleLocation, Quaternion.identity);
        }

        private IEnumerator MoveRight() {
            if(_index >= images.Length - 1) yield break;
            _index++;
            var newImage = Instantiate(image, LeftImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, LeftTitleLocation, Quaternion.identity);
            
            newImage.sprite = images[_index];
            newTitle.text = images[_index].name;
            
            var multiply = 1f;
            for (var i = 0f; i <= 2; i += Time.fixedDeltaTime) {
                _subImage.transform.position = (RightImageLocation - ImageLocation) * multiply / 50;
                _subTitle.transform.position = (RightTitleLocation - TitleLocation) * multiply / 50;
                newImage.transform.position = (ImageLocation - LeftImageLocation) * multiply / 50;
                newTitle.transform.position = (TitleLocation - LeftTitleLocation) * multiply / 50;
                multiply *= 0.985f;
                yield return new WaitForFixedUpdate();
            }

            newImage.transform.position = ImageLocation;
            newTitle.transform.position = TitleLocation;
            Destroy(_subImage);
            Destroy(_subTitle);
            _subImage = newImage;
            _subTitle = newTitle;
        }

        private IEnumerator MoveLeft() {
            if(_index <= 0) yield break;
            _index--;
            var newImage = Instantiate(image, RightImageLocation, Quaternion.identity);
            var newTitle = Instantiate(title, RightTitleLocation, Quaternion.identity);
            
            newImage.sprite = images[_index];
            newTitle.text = images[_index].name;
            
            var multiply = 1f;
            for (var i = 0f; i <= 2; i += Time.fixedDeltaTime) {
                _subImage.transform.position = (LeftImageLocation - ImageLocation) * multiply / 50;
                _subTitle.transform.position = (LeftTitleLocation - TitleLocation) * multiply / 50;
                newImage.transform.position = (ImageLocation - RightImageLocation) * multiply / 50;
                newTitle.transform.position = (TitleLocation - RightTitleLocation) * multiply / 50;
                multiply *= 0.985f;
                yield return new WaitForFixedUpdate();
            }

            newImage.transform.position = ImageLocation;
            newTitle.transform.position = TitleLocation;
            Destroy(_subImage);
            Destroy(_subTitle);
            _subImage = newImage;
            _subTitle = newTitle;
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