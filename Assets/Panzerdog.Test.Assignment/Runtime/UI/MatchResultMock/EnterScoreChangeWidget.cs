using System.Collections.Generic;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Types;
using TMPro;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchResultMock
{
    public class EnterScoreChangeWidget : MonoBehaviour
    {
        private const string Zero = "0";

        private static readonly List<string> Reasons = new()
        {
            ChangeScoreReason.Victory.ToString(),
            ChangeScoreReason.Defeat.ToString(),
            ChangeScoreReason.MVP.ToString(),
            ChangeScoreReason.Bonus.ToString(),
        };
        
        [SerializeField] private TMP_Dropdown _reasonDropdown;
        [SerializeField] private TMP_InputField _valueText;

        
        public void Init(ReactiveProperty<ScoreChangeData> changeScoreData)
        {
            _reasonDropdown.ClearOptions();
            _reasonDropdown.AddOptions(Reasons);
            
            _reasonDropdown.onValueChanged.AddListener(x =>
            {
                changeScoreData.Value = new ScoreChangeData(changeScoreData.Value)
                {
                    Reason = (ChangeScoreReason) x
                };
            });
            
            _valueText.onValueChanged.AddListener(x =>
            {
                changeScoreData.Value = new ScoreChangeData(changeScoreData.Value)
                {
                    Value = int.TryParse(x, out var result) ? result : 0
                };
            });

            _valueText.onDeselect.AddListener(ResetValueIfInvalid);
            _valueText.onSubmit.AddListener(ResetValueIfInvalid);

            changeScoreData.Subscribe(x =>
            {
                _reasonDropdown.SetValueWithoutNotify((int) x.Reason);
                _valueText.SetTextWithoutNotify(x.Value.ToString());
            });
        }

        public void Dispose()
        {
            _valueText.onSubmit.RemoveAllListeners();
            _valueText.onDeselect.RemoveAllListeners();
            _valueText.onValueChanged.RemoveAllListeners();
            _reasonDropdown.onValueChanged.RemoveAllListeners();
        }
        
        private void ResetValueIfInvalid(string newValue)
        {
            if (!int.TryParse(newValue, out _))
            {
                _valueText.text = Zero;
            }
        }
    }
}