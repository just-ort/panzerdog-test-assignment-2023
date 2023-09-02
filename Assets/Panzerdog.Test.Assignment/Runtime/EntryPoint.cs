using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.UI.MatchCompletion;
using Panzerdog.Test.Assignment.UI.MatchResultMock;
using Panzerdog.Test.Assignment.ViewModels;
using UnityEngine;

namespace Panzerdog.Test.Assignment
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private ScreenManager _screenManager;
        [SerializeField] private ScoreThresholds _scoreThresholds;
        
        private async void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            DOTween.Init(useSafeMode:true);
            DOTween.defaultAutoPlay = AutoPlay.None;
            DOTween.defaultAutoKill = false;

            await ProcessMatch(CancellationToken.None);
        }

        private async Task ProcessMatch(CancellationToken ct)
        {
            var matchController = new MatchController();
            
            await _screenManager.Show<MatchResultMockScreen>(new MatchResultMockViewModel(matchController), CancellationToken.None);

            await matchController.OnMatchCompleted.Task;
            
            await _screenManager.Hide<MatchResultMockScreen>(CancellationToken.None);
            
            await _screenManager.Show<MatchResultScreen>(new MatchResultViewModel(matchController, _scoreThresholds.ExperienceLevels, _scoreThresholds.RatingGrades), ct);
        }
    }
}