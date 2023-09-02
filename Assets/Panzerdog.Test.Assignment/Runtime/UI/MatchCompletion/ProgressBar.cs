using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Panzerdog.Test.Assignment.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panzerdog.Test.Assignment.UI.MatchCompletion
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private float _fullBarFillDuration = 2f;
        
        [SerializeField] private Image _bar;
        [SerializeField] private TMP_Text _text;

        [Space] 
        [SerializeField] private int _currentValue;
        [SerializeField] private int _maxValue;

        public void Setup(int currentValue, int maxValue)
        {
            _currentValue = currentValue;
            _maxValue = maxValue;
            _bar.fillAmount = (float)_currentValue / _maxValue;
            _text.SetText($"{_currentValue}/{_maxValue}");
        }

        public async Task SetValueAsync(int value, CancellationToken ct)
        {
            if (value == _currentValue)
            {
                return;
            }

            _currentValue = value;
            
            var duration = Mathf.Max(1, _fullBarFillDuration * _currentValue / _maxValue);
            await _bar.DOFillAmount((float) _currentValue / _maxValue, duration)
                .OnUpdate(UpdateText)
                .Play(ct)
                .AsyncWaitForCompletion();
            
            _text.SetText($"{_currentValue}/{_maxValue}");
            _currentValue = value;
        }

        private void UpdateText()
        {
            _text.SetText($"{(int)(_maxValue * _bar.fillAmount)}/{_maxValue}");
        }
    }
}