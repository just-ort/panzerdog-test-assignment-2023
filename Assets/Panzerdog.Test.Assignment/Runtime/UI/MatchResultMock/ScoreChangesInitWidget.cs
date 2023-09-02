using Panzerdog.Test.Assignment.Data;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchResultMock
{
    public class ScoreChangesInitWidget : MonoBehaviour
    {
        [SerializeField] private EnterScoreChangeWidget[] _enterChangeScoreValueWidget;

        public void Init(ReactiveCollection<ReactiveProperty<ScoreChangeData>> changeScoreDatas)
        {
            foreach (var widget in _enterChangeScoreValueWidget)
            {
                var changeScoreData = new ReactiveProperty<ScoreChangeData>();
                changeScoreDatas.Add(changeScoreData);
                widget.Init(changeScoreData);
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