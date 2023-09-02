using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Utils;
using TMPro;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchCompletion
{
    public class DisplayScoreChangesWidget : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _changesContainer;
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private TMP_Text _levelText;

        [SerializeField] private DisplayScoreChangeWidget displayScoreChangeWidgetPrefab;

        private ScoreAndLevelData _scoreAndLevelData;

        private TaskQueue _taskQueue;

        private CancellationTokenSource _cancellationTokenSource;
        private ReactiveDictionary<ScoreChangeData,List<ScoreChangeStepData>> _scoreChanges;
        
        public void Init(ScoreAndLevelData saveData, int threshold,
            ReactiveDictionary<ScoreChangeData, List<ScoreChangeStepData>> scoreChanges, TaskQueue taskQueue)
        {
            _scoreChanges = scoreChanges;
            _taskQueue = taskQueue;
            _cancellationTokenSource = new CancellationTokenSource();
            
            _canvasGroup.alpha = 0;
            
            _progressBar.Setup(saveData.Score, threshold);
            _levelText.SetText(saveData.Level.ToString());
        }

        public async Task Show()
        {
            await _canvasGroup.DOFade(1, 1).Play(_cancellationTokenSource.Token).AsyncWaitForCompletion();
            
            _scoreChanges.ObserveAdd().Subscribe(async x =>
            {
                await _taskQueue.Enqueue(() => OnScoreChanged(x));
            });
        }

        public void SkipAnimations()
        {
            _cancellationTokenSource.Cancel();
        }
        
        private async Task OnScoreChanged(DictionaryAddEvent<ScoreChangeData, List<ScoreChangeStepData>> scoreChange)
        {
            var changeView = Instantiate(displayScoreChangeWidgetPrefab, _changesContainer);
            _ = changeView.Show(scoreChange.Key, 1, _cancellationTokenSource.Token);

            foreach (var state in scoreChange.Value)
            {
                _progressBar.Setup(state.CurrentScore, state.MaxScore);
                _levelText.SetText(state.Level.ToString());
                await _progressBar.SetValueAsync(state.NewScore, _cancellationTokenSource.Token);
            }
        }
    }
}