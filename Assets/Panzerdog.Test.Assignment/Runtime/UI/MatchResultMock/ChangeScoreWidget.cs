using Panzerdog.Test.Assignment.Data;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchResultMock
{
    public class ChangeScoreWidget : MonoBehaviour
    {
        [SerializeField] private ChangeScoreValueWidget[] _changeScoreValueWidgets;

        public void Init(ReactiveCollection<ReactiveProperty<ChangeScoreData>> changeScoreDatas)
        {
            foreach (var widget in _changeScoreValueWidgets)
            {
                var changeScoreData = new ReactiveProperty<ChangeScoreData>();
                changeScoreDatas.Add(changeScoreData);
                widget.Init(changeScoreData);
            }
        }

        public void Dispose()
        {
            foreach (var widget in _changeScoreValueWidgets)
            {
                widget.Dispose();
            }
        }
    }
}