using HutongGames.PlayMaker;
using Scellecs.Morpeh;

namespace Gameplay.EnemiesLogicAssembly
{
    public class MoveToBuildingFSM : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var navigationUnitProvider = Owner.GetComponent<NavigationUnitProvider>();
            var stash = World.Default.GetStash<MoveToBuildingComponent>().AsDisposable();
            if (!stash.Has(navigationUnitProvider.Entity))
            {
                ref var serializedData = ref navigationUnitProvider.GetData();
                stash.Add(navigationUnitProvider.Entity, new MoveToBuildingComponent(serializedData.agent, MoveToBuildingComplete));
            }
        }

        private void MoveToBuildingComplete()
        {
            Finish();
        }
    }
}