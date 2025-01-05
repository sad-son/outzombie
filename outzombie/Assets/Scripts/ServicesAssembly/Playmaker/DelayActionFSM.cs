using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HutongGames.PlayMaker;

namespace ServicesAssembly.Playmaker
{
    public class DelayActionFSM : FsmStateAction
    {
        public float waitForSeconds;
        public int waitForFrame;
        
        public override void OnEnter()
        {
            base.OnEnter();
            Wait(Owner.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid Wait(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waitForSeconds), cancellationToken: cancellationToken);
            await UniTask.DelayFrame(waitForFrame, cancellationToken: cancellationToken);
            Finish();
        }
    }
}