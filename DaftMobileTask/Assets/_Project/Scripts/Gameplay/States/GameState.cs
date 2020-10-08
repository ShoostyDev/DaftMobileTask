
namespace Assets.Scripts.States
{
    public class GameState
    {
        private float timeScore;
        public float TimeScore { get => timeScore; set => timeScore = value > 0 ? value : 0; }
        private float bestTimeScore;
        public float BestTimeScore { get => bestTimeScore; set => bestTimeScore = value > bestTimeScore ? value : bestTimeScore; }
    }
}