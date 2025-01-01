using UnityEngine.AddressableAssets;

namespace SceneSystem
{
    public class SceneController
    {
        public static void LoadScene(SceneType sceneType)
        {
            var loadHandle = Addressables.LoadSceneAsync(sceneType.ToString());
            loadHandle.WaitForCompletion();
        }
    }
}