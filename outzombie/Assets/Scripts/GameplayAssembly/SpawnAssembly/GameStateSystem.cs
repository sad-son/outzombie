using Scellecs.Morpeh;

namespace Gameplay.SpawnAssembly
{
    public class GameStateSystem// : ISystem
    {
        /*public World World { get; set; }

        private Filter _filter;
        private Stash<BuildingComponent> _stash;
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private EnemiesPoolContainer _enemiesPoolContainer;

        private bool _spawning;
        private CancellationTokenSource _cancellationTokenSource;

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<BuildingComponent>().Build();
            _stash = World.GetStash<BuildingComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _enemiesPoolContainer = ServiceLocatorController.Resolve<GameplaySystemLocator>()
                .Resolve<EnemiesPoolContainer>();

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_spawning)
                return;

            if (_enemiesPoolContainer.IsEmpty())
                return;

            foreach (var entity in _filter)
            {
                ref var buildingComponent = ref _stash.Get(entity);

                if (buildingComponent.isMain)
                {
                    
                }
                ref var teamComponent = ref _teamStash.Get(entity);

                SpawnWaves(buildingComponent, spawnPosition, teamComponent).Forget();
            }
        }*/
    }
}