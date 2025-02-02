using Cysharp.Threading.Tasks;
using Gameplay.CardAssembly;
using Gameplay.EnemiesLogicAssembly;
using Gameplay.Extensions;
using Gameplay.ObjectPoolAssembly;
using Scellecs.Morpeh;
using ServiceLocatorSystem;
using UnityEngine;

namespace Gameplay.SpawnAssembly
{
    public class SpawnCardSystem :  EcsSystem
    {
        private Filter _filter;
        private Stash<CardUiComponent> _stash;
        private Stash<TeamComponent> _teamStash;
        private MyUnitsPoolContainer unitsPoolContainer;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<CardUiComponent>()
                .Without<DisabledComponent>().Build();
            _stash = World.GetStash<CardUiComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            unitsPoolContainer = ServiceLocatorController.Resolve<GameplaySystemLocator>()
                .Resolve<MyUnitsPoolContainer>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var cardUiComponent = ref _stash.Get(entity);
                ref var teamComponent = ref _teamStash.Get(entity);

                if (cardUiComponent.readyToSpawn)
                {
                    var spawnPosition = cardUiComponent.position;
                    Spawn(cardUiComponent.unitId, spawnPosition, teamComponent.team).Forget();
                    cardUiComponent.readyToSpawn = false;
                }
            }
        }

        private async UniTaskVoid Spawn(string id, Vector3 position, byte team)
        {
            var enemy = await unitsPoolContainer.Get(id);
            enemy.transform.position = position;

            if (enemy.TryGetComponent(out TeamProvider teamProvider))
            {
                teamProvider.SetTeam(team);
            }
        }
    }
}