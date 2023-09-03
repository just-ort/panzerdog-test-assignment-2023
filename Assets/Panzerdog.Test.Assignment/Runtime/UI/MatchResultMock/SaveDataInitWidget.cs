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
        
        public void Init(ReactiveProperty<SaveData> saveData)
        {
            var saveDataValue = saveData.Value;
            ParseSaveData(saveDataValue);
            AddInputListeners(saveData);
            saveData.Subscribe(ParseSaveData);
        }

        public void Dispose()
        {
            _experienceScore.onValueChanged.RemoveAllListeners();
            _experienceLevel.onValueChanged.RemoveAllListeners();
            _ratingScore.onValueChanged.RemoveAllListeners();
            _ratingLevel.onValueChanged.RemoveAllListeners();
        }

        private void ParseSaveData(SaveData saveData)
        {
            _ratingLevel.SetTextWithoutNotify(saveData.Rating.Level.ToString());
            _ratingScore.SetTextWithoutNotify(saveData.Rating.Score.ToString());
            _experienceLevel.SetTextWithoutNotify(saveData.Experience.Level.ToString());
            _experienceScore.SetTextWithoutNotify(saveData.Experience.Score.ToString());
        }
        
        private void AddInputListeners(ReactiveProperty<SaveData> saveData)
        {
            var saveDataValue = saveData.Value;
            
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