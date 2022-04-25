namespace Script.Utilities.SceneDataPacks
{
    public abstract class SceneDataPack
    {
        public abstract GameScenes PreviousGameScene { get; }
    }

    public class DefaultSceneDataPack : SceneDataPack
    {
        private readonly GameScenes _previousGameScenes;

        public override GameScenes PreviousGameScene
        {
            get { return _previousGameScenes; }
        }

        public DefaultSceneDataPack(GameScenes prev)
        {
            _previousGameScenes = prev;
        }
    }
}
