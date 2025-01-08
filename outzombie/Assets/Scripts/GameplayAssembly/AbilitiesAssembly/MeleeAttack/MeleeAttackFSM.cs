using HutongGames.PlayMaker;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.AbilitiesAssembly
{
    public class MeleeAttackFSM : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var meleeAttackProvider = Owner.GetComponent<MeleeAttackProvider>();
            var stash = World.Default.GetStash<MeleeAttackTriggerComponent>().AsDisposable();
            if (!stash.Has(meleeAttackProvider.Entity))
            {
                stash.Add(meleeAttackProvider.Entity, new MeleeAttackTriggerComponent(MeleeAttackCompleted));
            }
        }

        private void MeleeAttackCompleted()
        {
            Debug.LogError($"SAD MeleeAttackCompleted");
            Finish();
        }
    }
}