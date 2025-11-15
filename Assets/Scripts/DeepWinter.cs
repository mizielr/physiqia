using UnityEngine;

namespace Physiqia
{
    public class DeepWinter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public static void SavePosition(int position)
        {
            PlayerPrefs.SetInt(MenuCameraPositionEnum.KEY, position);
            PlayerPrefs.Save();
        }

        /// <summary>
        ///
        /// </summary>
        public static void Reset()
        {
            PlayerPrefs.SetInt(MenuCameraPositionEnum.KEY, MenuCameraPositionEnum.DEFAULT);
            PlayerPrefs.Save();
        }
    }
}
