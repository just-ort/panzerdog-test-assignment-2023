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

    //TODO: вынести в отдельный файл
    public struct ScoreAndLevelData
    {
        public int Score { get; set; }
        public int Level { get; set; }

        public ScoreAndLevelData(ScoreAndLevelData other)
        {
            Score = other.Score;
            Level = other.Level;
        }

        public override string ToString()
        {
            return $"{nameof(Score)}={Score}, {nameof(Level)}={Level}";
        }
    }
}