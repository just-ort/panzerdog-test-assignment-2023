using System.Linq;
using System.Threading.Tasks;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.Types;
using UniRx;

namespace Panzerdog.Test.Assignment.ViewModels
{
    public class MatchResultMockViewModel : IViewModel
    {
        public ReactiveProperty<SaveData> SaveData { get; }
        public ReactiveCollection<ReactiveProperty<ScoreChangeData>> RatingChangeData { get; }
        public ReactiveCollection<ReactiveProperty<ScoreChangeData>> ExperienceChangeData { get; }
        public ReactiveProperty<MatchResult> MatchResult { get; }

        public TaskCompletionSource<bool> OnMatchCompleted { get; } = new();

        public MatchResultMockViewModel(MatchController matchController)
        {
            SaveData = new ReactiveProperty<SaveData>(matchController.SaveData);
            RatingChangeData = new ReactiveCollection<ReactiveProperty<ScoreChangeData>>(
                matchController.RatingChangeData.Select(x => new ReactiveProperty<ScoreChangeData>(x)));
            ExperienceChangeData = new ReactiveCollection<ReactiveProperty<ScoreChangeData>>(
                matchController.ExperienceChangeData.Select(x => new ReactiveProperty<ScoreChangeData>(x)));
            MatchResult = new ReactiveProperty<MatchResult>(matchController.MatchResult);
            
            Subscribe(matchController);
        }

        public void Dispose()
        {
            SaveData.Dispose();
            RatingChangeData.Dispose();
            ExperienceChangeData.Dispose();
            MatchResult.Dispose();
        }

        public void CompleteMatch()
        {
            OnMatchCompleted.TrySetResult(true);
        }

        private void Subscribe(MatchController matchController)
        {
            SaveData.Subscribe(x => matchController.SaveData = new SaveData(x));
            
            RatingChangeData.ObserveAdd().Subscribe(x =>
            {
                matchController.RatingChangeData.Insert(x.Index, x.Value.Value);
                x.Value.Subscribe(y => matchController.RatingChangeData[x.Index] = new ScoreChangeData(y));
            });
            
            RatingChangeData.ObserveRemove().Subscribe(x =>
            {
                matchController.RatingChangeData.RemoveAt(x.Index);
            });

            ExperienceChangeData.ObserveAdd().Subscribe(x =>
            {
                matchController.ExperienceChangeData.Insert(x.Index, x.Value.Value);
                x.Value.Subscribe(y => matchController.ExperienceChangeData[x.Index] = new ScoreChangeData(y));
            });
            
            ExperienceChangeData.ObserveRemove().Subscribe(x =>
            {
                matchController.ExperienceChangeData.RemoveAt(x.Index);
            });

            MatchResult.Subscribe(x => matchController.MatchResult = x);
        }
    }
}