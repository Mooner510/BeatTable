using System.Collections;
using DG.Tweening;
using Listener;
using Musics.Data;
using UnityEngine;
using Utils;

namespace Musics {
    public class QuadPlayer : Player {

        private static readonly Color ClickColor = new Color(0f, 0.8f, 0.8f, 0.5f);
        
        public override IEnumerator Accept(LiveNoteData note, float time) {
            yield return new WaitForSecondsRealtime(time);
            var location = GameUtils.Locator(GameMode.Quad, note.note);
            var obj = Instantiate(beatInspector, location * 5f, Quaternion.identity);
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            obj.transform.DOMove(location, 1f).SetEase(Ease.Linear);
            yield return new WaitForSecondsRealtime(0.5f);
            KeyListener.Instance.Queue(note);
            yield return new WaitForSecondsRealtime(0.35f);
            spriteRenderer.color = ClickColor;
            yield return new WaitForSecondsRealtime(0.3f);
            obj.transform.DOScale(Vector3.one * 2.5f, .25f).SetEase(Ease.Linear);
            yield return new WaitForSecondsRealtime(0.3f);
            Destroy(obj);
        }
    }
}