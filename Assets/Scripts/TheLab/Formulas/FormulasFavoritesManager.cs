using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Physiqia.TheLab
{
    public static class FormulasFavoritesManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="Application.persistentDataPath"></param>
        /// <param name=""fav-formulas.json""></param>
        /// <returns></returns>
        private static readonly string filePath = Path.Combine(
            Application.persistentDataPath,
            "fav-formulas.json"
        );

        /// <summary>
        ///
        /// /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        private static HashSet<int> favorites = new HashSet<int>();

        /// <summary>
        ///
        /// /// </summary>
        public static void LoadFavorites()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                FavoriteData data = JsonUtility.FromJson<FavoriteData>(json);
                favorites = new HashSet<int>(data.favorites);
            }
        }

        /// <summary>
        ///
        /// /// </summary>
        public static void SaveFavorites()
        {
            FavoriteData data = new FavoriteData { favorites = new List<int>(favorites) };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        ///
        /// /// </summary>
        /// <param name="id"></param>
        public static void AddFavorite(int id)
        {
            favorites.Add(id);
            SaveFavorites();
        }

        /// <summary>
        ///
        /// /// </summary>
        /// <param name="id"></param>
        public static void RemoveFavorite(int id)
        {
            favorites.Remove(id);
            SaveFavorites();
        }

        /// <summary>
        ///
        /// /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsFavorite(int id) => favorites.Contains(id);

        /// <summary>
        ///
        /// /// </summary>
        [System.Serializable]
        private class FavoriteData
        {
            public List<int> favorites;
        }
    }
}
