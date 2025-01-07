using Scellecs.Morpeh.Providers;

namespace Gameplay.EnemiesLogicAssembly
{
    public class TeamProvider : MonoProvider<TeamComponent>
    {
        public void SetTeam(byte team)
        {
            ref var data = ref GetData();
            data.team = team;
        }
    }
}