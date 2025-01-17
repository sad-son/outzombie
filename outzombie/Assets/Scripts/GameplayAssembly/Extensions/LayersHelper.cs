using UnityEngine;

namespace Gameplay.Extensions
{
    public static class LayersHelper
    {
        public static int Ground => LayerMask.NameToLayer("Ground");
    }
}