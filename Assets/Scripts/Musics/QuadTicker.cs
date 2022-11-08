using Musics.Data;

namespace Musics {
    public class QuadTicker : Ticker {
        protected override void Tick() {
            var now = GetPlayTime();
            var i = 0;
            do {
                var note = NoteManager.Pick(i);
                if (note.time <= now + 1f) {
                    StartCoroutine(Player.Instance.Accept(NoteManager.Pop(), note.time - (now + 1f)));
                } else break;
            } while (!NoteManager.IsTop(++i));
        }
    }
}