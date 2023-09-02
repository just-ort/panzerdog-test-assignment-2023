using System.Collections.Generic;
using UnityEngine;

namespace Panzerdog.Test.Assignment.Data
{
    [CreateAssetMenu(fileName = "Score Thresholds", menuName = "Configs")]
    public class ScoreThresholds : ScriptableObject
    {
        [SerializeField] private int[] _ratingGrades;
        [SerializeField] private int[] _experienceLevels;

        public IReadOnlyList<int> RatingGrades => _ratingGrades;
        public IReadOnlyList<int> ExperienceLevels => _experienceLevels;
    }
}