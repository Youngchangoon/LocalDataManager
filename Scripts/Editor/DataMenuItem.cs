using UnityEditor;
using UnityEngine;

namespace YoungPackage.Data
{
    public static class DataMenuItem
    {
        [MenuItem("YoungPackage/Data/Clear All local data")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}