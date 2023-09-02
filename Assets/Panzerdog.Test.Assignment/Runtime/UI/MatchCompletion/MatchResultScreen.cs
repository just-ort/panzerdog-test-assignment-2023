using System;
using System.Threading.Tasks;
using Panzerdog.Test.Assignment.Attributes;
using Panzerdog.Test.Assignment.Utils;
using Panzerdog.Test.Assignment.ViewModels;
using TMPro;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchCompletion
{
    [AddressableScreen("MatchResultScreen")]
    public class MatchResultScreen : ScreenBase
    {
        [SerializeField] private TMP_Text _matchResultText;
        [SerializeField] private DisplayScoreChangesWidget _ratingWidget;
        [SerializeField] private DisplayScoreChangesWidget _experienceWidget;

        private TaskQueue _taskQueue;

        private MatchResultViewModel _viewModel;

        private DisplayScoreChangesWidget _lastShownWidget;

        private IDisposable _displayChangesSkip;
        
        protected override void Init(IViewModel viewModel)
        {
            _viewModel = (MatchResultViewModel) viewModel;
            _taskQueue = new TaskQueue();
            
            _viewModel.MatchResult.Subscribe(x => _matchResultText.SetText(x.ToString()));
            
            _ratingWidget.Init(_viewModel.RatingSavedData.Value, _viewModel.CurrentRatingThreshold, _viewModel.RatingScoreChanges, _taskQueue);
            _experienceWidget.Init(_viewModel.ExperienceSavedData.Value, _viewModel.CurrentExperienceThreshold, _viewModel.ExperienceScoreChanges, _taskQueue);
            
            _displayChangesSkip = Observable.EveryUpdate().Subscribe(x =>
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (_lastShownWidget)
                    {
                        _lastShownWidget.SkipAnimations();
                    }
                }
            });
        }

        protected override void Dispose()
        {
            _displayChangesSkip.Dispose();
            _viewModel.Dispose();
        }

        protected override async Task OnShow()
        {
            await _taskQueue.Enqueue(() =>
            {
                _lastShownWidget = _ratingWidget;
                return _ratingWidget.Show();
            });
            
            _viewModel.UpdateRatingSaveData();

            await _taskQueue.Enqueue(() =>
            {
                _lastShownWidget = _experienceWidget;    
                return _experienceWidget.Show();
            });
            
            _viewModel.UpdateExperienceSaveData();
        }
    }
}