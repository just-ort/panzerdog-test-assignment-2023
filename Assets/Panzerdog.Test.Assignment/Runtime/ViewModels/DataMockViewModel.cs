using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.Types;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Panzerdog.Test.Assignment.ViewModels
{
    //TODO: dispose()
    public class DataMockViewModel : IViewModel
    {
        //TODO: удалить
        private static DataMockViewModel _instance;
        
        //TODO: удалить
#if UNITY_EDITOR
        [MenuItem("Test/Show DataMockViewModel")]
        public static void ShowDataMockViewModel()
        {
            Debug.Log($"[SaveData]: {_instance.SaveData}");
            
            Debug.Log($"[RatingChangeData]:");
            foreach (var reactiveProperty in _instance.RatingChangeData)
            {
                Debug.Log(reactiveProperty.Value);
            }
            
            Debug.Log($"[ExperienceChangeData]: ");
            foreach (var reactiveProperty in _instance.ExperienceChangeData)
            {
                Debug.Log(reactiveProperty.Value);
            }
            
            Debug.Log($"[MatchResult]: {_instance.MatchResult}");
        }
#endif
        private readonly MatchController _matchController;
        
        public ReactiveProperty<SaveData> SaveData { get; set; } = new(new SaveData()
        {
            Experience = new ScoreAndLevelData()
            {
                Level = 1,
                Score = 10
            }
        });
        public ReactiveCollection<ReactiveProperty<ChangeScoreData>> RatingChangeData { get; set; } = new();
        public ReactiveCollection<ReactiveProperty<ChangeScoreData>> ExperienceChangeData { get; set; } = new();
        public ReactiveProperty<MatchResult> MatchResult { get; set; } = new();
        
        public DataMockViewModel(MatchController matchController)
        {
            _instance = this;
            _matchController = matchController;

            SaveData.Subscribe(x => _matchController.SaveData = new SaveData(x));
            
            RatingChangeData.ObserveAdd().Subscribe(x =>
            {
                _matchController.RatingChangeData.Insert(x.Index, x.Value.Value);
                x.Value.Subscribe(y => _matchController.RatingChangeData[x.Index] = new ChangeScoreData(y));
            });
            
            RatingChangeData.ObserveRemove().Subscribe(x =>
            {
                _matchController.RatingChangeData.RemoveAt(x.Index);
            });

            ExperienceChangeData.ObserveAdd().Subscribe(x =>
            {
                _matchController.ExperienceChangeData.Insert(x.Index, x.Value.Value);
                x.Value.Subscribe(y => _matchController.ExperienceChangeData[x.Index] = new ChangeScoreData(y));
            });
            
            ExperienceChangeData.ObserveRemove().Subscribe(x =>
            {
                _matchController.ExperienceChangeData.RemoveAt(x.Index);
            });

            MatchResult.Subscribe(x => _matchController.MatchResult = x);
        }
        
        public void CompleteMatch()
        {
            _matchController.CompleteMatch();
        }
    }
}