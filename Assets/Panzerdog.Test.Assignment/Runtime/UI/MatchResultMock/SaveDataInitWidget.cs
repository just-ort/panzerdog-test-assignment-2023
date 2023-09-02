using Panzerdog.Test.Assignment.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI.MatchResultMock
{
    public class SaveDataInitWidget : MonoBehaviour
    {
        private const string Zero = "0";
    
        [SerializeField] private TMP_InputField _ratingLevel;
        [SerializeField] private TMP_InputField _ratingScore;
        [SerializeField] private TMP_InputField _experienceLevel;
        [SerializeField] private TMP_InputField _experienceScore;

        private ReactiveProperty<SaveData> _saveData;
        
        public void Init(ReactiveProperty<SaveData> saveData)
        {
            _saveData = saveData;
            
            var saveDataValue = saveData.Value;
            
            //TODO; тут едет верстка на больших числах
            _ratingLevel.SetTextWithoutNotify(saveDataValue.Rating.Level.ToString());
            _ratingScore.SetTextWithoutNotify(saveDataValue.Rating.Score.ToString());
            _experienceLevel.SetTextWithoutNotify(saveDataValue.Experience.Level.ToString());
            _experienceScore.SetTextWithoutNotify(saveDataValue.Experience.Score.ToString());

            _ratingLevel.onValueChanged.AddListener(x =>
            {
                var newRatingLevel = ValidateInputValue(x, _ratingLevel);

                var newRating = new ScoreAndLevelData(saveDataValue.Rating)
                {
                    Level = newRatingLevel
                };
                saveDataValue.Rating = newRating;
                saveData.Value = saveDataValue;
            });
            
            _ratingScore.onValueChanged.AddListener(x =>
            {
                var newRatingScore = ValidateInputValue(x, _ratingScore);

                var newRating = new ScoreAndLevelData(saveDataValue.Rating)
                {
                    Score = newRatingScore
                };
                saveDataValue.Rating = newRating;
                saveData.Value = saveDataValue;
            });

            _experienceLevel.onValueChanged.AddListener(x =>
            {
                var newExperienceLevel = ValidateInputValue(x, _experienceLevel);

                var newExperience = new ScoreAndLevelData(saveDataValue.Experience)
                {
                    Level = newExperienceLevel
                };
                saveDataValue.Experience = newExperience;
                saveData.Value = saveDataValue;
            });
            
            _experienceScore.onValueChanged.AddListener(x =>
            {
                var newExperienceScore = ValidateInputValue(x, _experienceScore);

                var newExperience = new ScoreAndLevelData(saveDataValue.Experience)
                {
                    Score = newExperienceScore
                };
                saveDataValue.Experience = newExperience;
                saveData.Value = saveDataValue;
            });

            saveData.Subscribe(x =>
            {
                _ratingLevel.SetTextWithoutNotify(x.Rating.Level.ToString());
                _ratingScore.SetTextWithoutNotify(x.Rating.Score.ToString());
                _experienceLevel.SetTextWithoutNotify(x.Experience.Level.ToString());
                _experienceScore.SetTextWithoutNotify(x.Experience.Score.ToString());
            });
        }

        public void Dispose()
        {
            _saveData.Dispose();
            _experienceScore.onValueChanged.RemoveAllListeners();
            _experienceLevel.onValueChanged.RemoveAllListeners();
            _ratingScore.onValueChanged.RemoveAllListeners();
            _ratingLevel.onValueChanged.RemoveAllListeners();
        }

        private static int ValidateInputValue(string value, TMP_InputField input)
        {
            int validatedValue;
            if (int.TryParse(value, out var result))
            {
                validatedValue = result;
            }
            else
            {
                validatedValue = 0;
                input.SetTextWithoutNotify(Zero);
            }

            return validatedValue;
        }
    }
}