using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.Types;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Panzerdog.Test.Assignment.ViewModels
{
    //TODO: dispose()
    public class MatchResultMockViewModel : IViewModel
    {
        {
            
        }

        private readonly MatchController _matchController;
        
        public ReactiveProperty<SaveData> SaveData { get; } = new(new SaveData());
        public ReactiveCollection<ReactiveProperty<ScoreChangeData>> RatingChangeData { get; } = new();
        public ReactiveCollection<ReactiveProperty<ScoreChangeData>> ExperienceChangeData { get; } = new();
        public ReactiveProperty<MatchResult> MatchResult { get; } = new();
        
        public MatchResultMockViewModel(MatchController matchController)
        {
            _matchController = matchController;

            SaveData.Subscribe(x => _matchController.SaveData = new SaveData(x));
            
            RatingChangeData.ObserveAdd().Subscribe(x =>
            {
                _matchController.RatingChangeData.Insert(x.Index, x.Value.Value);
                x.Value.Subscribe(y => _matchController.RatingChangeData[x.Index] = new ScoreChangeData(y));
            });
            
            RatingChangeData.ObserveRemove().Subscribe(x =>
            {
                _matchController.RatingChangeData.RemoveAt(x.Index);
            });

            ExperienceChangeData.ObserveAdd().Subscribe(x =>
            {
                _matchController.ExperienceChangeData.Insert(x.Index, x.Value.Value);
                x.Value.Subscribe(y => _matchController.ExperienceChangeData[x.Index] = new ScoreChangeData(y));
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