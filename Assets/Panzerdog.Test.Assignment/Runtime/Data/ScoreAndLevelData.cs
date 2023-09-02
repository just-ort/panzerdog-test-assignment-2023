namespace Panzerdog.Test.Assignment.Data
{
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