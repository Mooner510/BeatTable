using UnityEngine;

namespace Map {
    public class MapMaker : MonoBehaviour {
        [SerializeField] private GameObject beatButton;

        private void Start() {
            for (var i = 0; i < 9; i++)
                Instantiate(beatButton, Utils.Locator(i), Quaternion.identity);
        }
    }
}