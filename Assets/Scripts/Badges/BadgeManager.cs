using System.Collections.Generic;
using Physiqia.API;
using UnityEngine;

namespace Physiqia.Badges
{
    /// <summary>
    ///
    /// </summary>
    public class BadgeManager : MonoBehaviour
    {
        public static BadgeManager Instance { get; private set; }

        public IReadOnlyList<HBadge> Badges => _badges;
        private List<HBadge> _badges = new List<HBadge>();

        /// <summary>
        ///
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="badges"></param>
        public void SetBadges(List<HBadge> badges)
        {
            _badges = badges ?? new List<HBadge>();
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearBadges()
        {
            _badges.Clear();
        }
    }
}
