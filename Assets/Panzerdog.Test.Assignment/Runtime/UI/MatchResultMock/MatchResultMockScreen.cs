using System.Collections.Generic;
using Panzerdog.Test.Assignment.Attributes;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.Types;
using Panzerdog.Test.Assignment.ViewModels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Panzerdog.Test.Assignment.UI.MatchResultMock
{
    [AddressableScreen("MatchResultMockScreen")]
    public class MatchResultMockScreen : ScreenBase
    {
        private static readonly List<string> PossibleMatchResults = new()
        {
            MatchResult.Victory.ToString(),
            MatchResult.Defeat.ToString()
        };
        
        [SerializeField] private TMP_Dropdown _matchResultDropdown;
        [SerializeField] private Button _completeMatchButton;

        [SerializeField] private SavedDataWidget _savedDataWidget;
        [SerializeField] private ChangeScoreWidget _ratingChangeWidget;
        [SerializeField] private ChangeScoreWidget _experienceChangeWidget;

        private DataMockViewModel _viewModel;
        
        protected override void Init(IViewModel viewModel)
        {
            _viewModel = (DataMockViewModel) viewModel;
            
            InitMatchResultDropdown();
            _completeMatchButton.onClick.AddListener(_viewModel.CompleteMatch);
            
            _savedDataWidget.Init(_viewModel.SaveData);
            _ratingChangeWidget.Init(_viewModel.RatingChangeData);
            _experienceChangeWidget.Init(_viewModel.ExperienceChangeData);
        }

        protected override void Dispose()
        {
            _experienceChangeWidget.Dispose();
            _ratingChangeWidget.Dispose();
            _savedDataWidget.Dispose();
            
            _completeMatchButton.onClick.RemoveAllListeners();
            DisposeMatchResultDropdown();
        }
        
        private void InitMatchResultDropdown()
        {
            _matchResultDropdown.ClearOptions();
            _matchResultDropdown.AddOptions(PossibleMatchResults);

            var matchResult = _viewModel.MatchResult;
            
            _matchResultDropdown.onValueChanged.AddListener(value =>
            {
                var newMatchResult = (MatchResult) value;
                matchResult.Value = newMatchResult;
            });

            matchResult.Subscribe(x => _matchResultDropdown.SetValueWithoutNotify((int) x));
        }

        private void DisposeMatchResultDropdown()
        {
            _viewModel.MatchResult.Dispose();
            _matchResultDropdown.onValueChanged.RemoveAllListeners();
        }
    }
}