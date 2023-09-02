using System.Collections.Generic;
using System.Threading.Tasks;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Types;
using UnityEditor;
using UnityEngine;

namespace Panzerdog.Test.Assignment.Services
{
    public class MatchController
    {
        //TODO: удалить
        private static MatchController _instance;

#if UNITY_EDITOR
        [MenuItem("Test/Show MatchController")]
        public static void ShowMatchController()
        {
            Debug.Log($"[SaveData]: {_instance.SaveData}");
            Debug.Log($"[RatingChangeData]:");
            _instance.RatingChangeData.ForEach(x => Debug.Log(x));
            
            Debug.Log($"[ExperienceChangeData]: ");
            _instance.ExperienceChangeData.ForEach(x => Debug.Log(x));

            
            Debug.Log($"[MatchResult]: {_instance.MatchResult}");
        }
#endif
        
        public SaveData SaveData { get; set; }
        public List<ChangeScoreData> RatingChangeData { get; } = new(4);
        public List<ChangeScoreData> ExperienceChangeData { get; } = new(4);
        public MatchResult MatchResult { get; set; }

        public TaskCompletionSource<bool> OnMatchCompleted { get; } = new();

        public MatchController()
        {
            _instance = this;

        }
        
        //TODO: подумать, нормальное ли решение держать это тут
        public void CompleteMatch()
        {
            OnMatchCompleted.TrySetResult(true);
        }
    }
}