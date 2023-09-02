using System.Collections.Generic;
using System.Threading.Tasks;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Types;

namespace Panzerdog.Test.Assignment.Services
{
    public class MatchController
    {
        public SaveData SaveData { get; set; }
        public List<ScoreChangeData> RatingChangeData { get; } = new(4);
        public List<ScoreChangeData> ExperienceChangeData { get; } = new(4);
        public MatchResult MatchResult { get; set; }

        public TaskCompletionSource<bool> OnMatchCompleted { get; } = new();

        //TODO: подумать, нормальное ли решение держать это тут
        public void CompleteMatch()
        {
            OnMatchCompleted.TrySetResult(true);
        }
    }
}