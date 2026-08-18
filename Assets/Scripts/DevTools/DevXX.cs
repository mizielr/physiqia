using UnityEngine;

namespace Physiqia.DevTools
{
    public class DevXX
    {
        void KillApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
