using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu (fileName = "LevelDatabase", menuName = "SlideSokoban/LevelDatabase")]
    public sealed class LevelDatabase : ScriptableObject
    {
        public static readonly string LEVEL_DATABASE_PATH = "LevelDatabase";
        public List<LevelData> levelList = new List<LevelData> ();
    }
}