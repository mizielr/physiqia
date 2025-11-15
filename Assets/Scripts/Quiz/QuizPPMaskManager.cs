using UnityEngine;

/// <summary>
///
/// </summary>
namespace Physiqia.Quiz
{
    /// <summary>
    ///
    /// </summary>
    public class QuizPPMaskManager : MonoBehaviour
    {
        private int _physicsMask = 0;
        private int _chemistryMask = 0;
        private int _mathMask = 0;
        private int _quizConfigMask = 0;

        public void SetPhysicsCategory(int index, bool selected) =>
            SetCategory(ref _physicsMask, index, selected);

        public bool IsPhysicsCategorySelected(int index) => GetCategory(_physicsMask, index);

        public void SetChemistryCategory(int index, bool selected) =>
            SetCategory(ref _chemistryMask, index, selected);

        public bool IsChemistryCategorySelected(int index) => GetCategory(_chemistryMask, index);

        public void SetMathCategory(int index, bool selected) =>
            SetCategory(ref _mathMask, index, selected);

        public bool IsMathCategorySelected(int index) => GetCategory(_mathMask, index);

        public bool IsMathematicsCategorySelected(int index) => GetCategory(_mathMask, index);

        public void SetQuizConfigFlag(int index, bool value) =>
            SetCategory(ref _quizConfigMask, index, value);

        public bool IsQuizConfigFlagSet(int index) => GetCategory(_quizConfigMask, index);

        /// <summary>
        ///
        /// </summary>
        public void Save()
        {
            SaveAll();
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveAll()
        {
            PlayerPrefs.SetInt(SparkEnum.QUIZ_CONFIG_PHYSICS_MASK, _physicsMask);
            PlayerPrefs.SetInt(SparkEnum.QUIZ_CONFIG_CHEMISTRY_MASK, _chemistryMask);
            PlayerPrefs.SetInt(SparkEnum.QUIZ_CONFIG_MATHEMATICS_MASK, _mathMask);
            PlayerPrefs.SetInt(SparkEnum.QUIZ_CONFIG_MASK, _quizConfigMask);
            PlayerPrefs.Save();
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveConfig()
        {
            PlayerPrefs.SetInt(SparkEnum.QUIZ_CONFIG_MASK, _quizConfigMask);
            PlayerPrefs.Save();
        }

        /// <summary>
        ///
        /// </summary>
        public void SavePhysicsCategories()
        {
            PlayerPrefs.SetInt(SparkEnum.QUIZ_CONFIG_PHYSICS_MASK, _physicsMask);
            PlayerPrefs.Save();
        }

        /// <summary>
        ///
        /// </summary>
        public void Load()
        {
            _physicsMask = PlayerPrefs.GetInt(SparkEnum.QUIZ_CONFIG_PHYSICS_MASK, 0);
            _chemistryMask = PlayerPrefs.GetInt(SparkEnum.QUIZ_CONFIG_CHEMISTRY_MASK, 0);
            _mathMask = PlayerPrefs.GetInt(SparkEnum.QUIZ_CONFIG_MATHEMATICS_MASK, 0);
            _quizConfigMask = PlayerPrefs.GetInt(SparkEnum.QUIZ_CONFIG_MASK, 0);
        }

        /// <summary>
        ///
        /// </summary>
        public void ResetAll()
        {
            _physicsMask = 0;
            _chemistryMask = 0;
            _mathMask = 0;
            _quizConfigMask = 0;
            SaveAll();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="index"></param>
        /// <param name="selected"></param>
        private void SetCategory(ref int mask, int index, bool selected)
        {
            if (index < 0 || index >= 32)
                return;

            if (selected)
                mask |= 1 << index;
            else
                mask &= ~(1 << index);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetCategory(int mask, int index)
        {
            if (index < 0 || index >= 32)
                return false;
            return (mask & (1 << index)) != 0;
        }
    }
}
