using System.Collections.Generic;
using Panzerdog.Test.Assignment.Attributes;
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

        [SerializeField] private SaveDataInitWidget _saveDataInitWidget;
        [SerializeField] private ScoreChangesInitWidget _ratingChangesInitWidget;
        [SerializeField] private ScoreChangesInitWidget _experienceChangesInitWidget;

        private MatchResultMockViewModel _viewModel;
        
        protected override void Init(IViewModel viewModel)
        {
            _viewModel = (MatchResultMockViewModel) viewModel;
            
            InitMatchResultDropdown();
            _completeMatchButton.onClick.AddListener(_viewModel.CompleteMatch);
            
            _saveDataInitWidget.Init(_viewModel.SaveData);
            _ratingChangesInitWidget.Init(_viewModel.RatingChangeData);
            _experienceChangesInitWidget.Init(_viewModel.ExperienceChangeData);
        }

        protected override void Dispose()
        {
            _experienceChangesInitWidget.Dispose();
            _ratingChangesInitWidget.Dispose();
            _saveDataInitWidget.Dispose();
            
            _completeMatchButton.onClick.RemoveAllListeners();
            _matchResultDropdown.onValueChanged.RemoveAllListeners();
            
            _viewModel.Dispose();
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
    }
}