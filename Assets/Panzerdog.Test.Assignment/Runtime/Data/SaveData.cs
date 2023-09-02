using System;

namespace Panzerdog.Test.Assignment.Data
{
    [Serializable]
    public struct SaveData
    {
        public ScoreAndLevelData Rating { get; set; }
        public ScoreAndLevelData Experience { get; set; }
        
        public SaveData(SaveData other)
        {
            Rating = other.Rating;
            Experience = other.Experience;
        }

        public override string ToString()
        {
            return $"{nameof(Rating)}=[{Rating}], {nameof(Experience)}=[{Experience}]";
        }
    }
}