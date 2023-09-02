using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Utils;
using TMPro;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchCompletion
{
    public class DisplayScoreChangeWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _reasonText;
        [SerializeField] private TMP_Text _valueText;

        [SerializeField] private CanvasGroup _canvasGroup;
        
        public async Task Show(ScoreChangeData scoreChangeData, float fadeDuration, CancellationToken ct)
        {
            _canvasGroup.alpha = 0;
            _reasonText.SetText(scoreChangeData.Reason.ToString());
            _valueText.SetText(scoreChangeData.Value.ToString());

            await _canvasGroup.DOFade(1, fadeDuration).Play(ct).AsyncWaitForCompletion();
        }
    }
}