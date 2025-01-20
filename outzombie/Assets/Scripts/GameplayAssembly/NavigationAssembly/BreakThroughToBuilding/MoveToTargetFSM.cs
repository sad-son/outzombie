using HutongGames.PlayMaker;
using Scellecs.Morpeh;

namespace Gameplay.EnemiesLogicAssembly.BreakThroughToBuilding
{
    public class MoveToTargetFSM : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var navigationUnitProvider = Owner.GetComponent<NavigationUnitProvider>();
            var stash = World.Default.GetStash<MoveToTargetComponent>().AsDisposable();
            if (!stash.Has(navigationUnitProvider.Entity))
            {
                ref var serializedData = ref navigationUnitProvider.GetData();
                stash.Add(navigationUnitProvider.Entity, new MoveToTargetComponent(serializedData.agent, MoveToBuildingComplete));
            }
        }

        private void MoveToBuildingComplete()
        {
            Finish();
        }
    }
}