using Panzerdog.Test.Assignment.Data;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchResultMock
{
    public class ScoreChangesInitWidget : MonoBehaviour
    {
        [SerializeField] private EnterScoreChangeWidget[] _enterChangeScoreValueWidget;

        public void Init(ReactiveCollection<ReactiveProperty<ScoreChangeData>> scoreChanges)
        {
            foreach (var widget in _enterChangeScoreValueWidget)
            {
                var scoreChangeData = new ReactiveProperty<ScoreChangeData>();
                scoreChanges.Add(scoreChangeData);
                widget.Init(scoreChangeData);
            }
        }

        public void Dispose()
        {
            foreach (var widget in _enterChangeScoreValueWidget)
            {
                widget.Dispose();
            }
        }
    }
}