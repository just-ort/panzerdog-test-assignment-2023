﻿using System.Collections.Generic;
using Panzerdog.Test.Assignment.Data;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.Types;
using UniRx;

namespace Panzerdog.Test.Assignment.ViewModels
{
    public class MatchResultViewModel : IViewModel
    {
        private readonly MatchController _matchController;
        private readonly IReadOnlyList<int> _experienceThresholds;
        private readonly IReadOnlyList<int> _ratingThresholds;
        
        public ReactiveProperty<ScoreAndLevelData> RatingSavedData { get; }
        public ReactiveProperty<ScoreAndLevelData> ExperienceSavedData { get; }
        public ReactiveProperty<MatchResult> MatchResult { get; }
        public ReactiveDictionary<ScoreChangeData, List<ScoreChangeStepData>> RatingScoreChanges { get; } = new();
        public ReactiveDictionary<ScoreChangeData, List<ScoreChangeStepData>> ExperienceScoreChanges { get; } = new();
        
        public int CurrentExperienceThreshold => _experienceThresholds[ExperienceSavedData.Value.Level];
        public int CurrentRatingThreshold => _ratingThresholds[RatingSavedData.Value.Level];

        public MatchResultViewModel(MatchController matchController, IReadOnlyList<int> experienceThresholds, IReadOnlyList<int> ratingThresholds)
        {
            _matchController = matchController;
            _experienceThresholds = experienceThresholds;
            _ratingThresholds = ratingThresholds;

            RatingSavedData = new ReactiveProperty<ScoreAndLevelData>(matchController.SaveData.Rating);
            RatingSavedData.Subscribe(x => matchController.SaveData = new SaveData(matchController.SaveData)
            {
                Rating = x
            });
            
            ExperienceSavedData = new ReactiveProperty<ScoreAndLevelData>(matchController.SaveData.Experience);
            ExperienceSavedData.Subscribe(x => matchController.SaveData = new SaveData(matchController.SaveData)
            {
                Experience = x
            });

            MatchResult = new ReactiveProperty<MatchResult>(matchController.MatchResult);
            MatchResult.Subscribe(x =>
            {
                matchController.MatchResult = x;
            });
        }

        public void UpdateRatingSaveData()
        {
            var ratingStates = GetChangeScoreSteps(RatingSavedData, _matchController.RatingChangeData, _ratingThresholds);
            foreach (var ratingState in ratingStates)
            {
                RatingScoreChanges.Add(ratingState.Key, ratingState.Value);
            }
        }
        
        public void UpdateExperienceSaveData()
        {
            var experienceStates = GetChangeScoreSteps(ExperienceSavedData, _matchController.ExperienceChangeData, _experienceThresholds);
            foreach (var experienceState in experienceStates)
            {
                ExperienceScoreChanges.Add(experienceState.Key, experienceState.Value);
            }
        }
        
        private static Dictionary<ScoreChangeData, List<ScoreChangeStepData>> GetChangeScoreSteps(ReactiveProperty<ScoreAndLevelData> saveDataProperty, IReadOnlyList<ScoreChangeData> scoreChanges, IReadOnlyList<int> scoreThresholds)
        {
            var saveData = saveDataProperty.Value;
            var result = new Dictionary<ScoreChangeData, List<ScoreChangeStepData>>(scoreChanges.Count);

            for (var i = 0; i < scoreChanges.Count; i++)
            {
                var scoreChange = scoreChanges[i];

                if (scoreChange.Value == 0)
                {
                    continue;
                }
                
                var changeScoreSteps = new List<ScoreChangeStepData>(1);
                
                while (true)
                {
                    if (scoreChange.Value == 0)
                    {
                        break;
                    }
                    
                    var currentThreshold = scoreThresholds[saveData.Level];
                    var newScore = saveData.Score + scoreChange.Value;

                    if (newScore >= currentThreshold)
                    {
                        changeScoreSteps.AddRange(GetLevelUpSteps(ref saveData, ref scoreChange, scoreThresholds));
                        
                        if (saveData.Level == scoreThresholds.Count - 1 && saveData.Score == scoreThresholds[saveData.Level])
                        {
                            result.Add(scoreChanges[i], changeScoreSteps);
                            return result;                     
                        }
                    }
                    else if (newScore <= 0)
                    {
                        changeScoreSteps.AddRange(GetLevelDownSteps(ref saveData, ref scoreChange, scoreThresholds));
                        
                        if (saveData.Level == 0 && saveData.Score == 0)
                        {
                            result.Add(scoreChanges[i], changeScoreSteps);
                            return result;
                        }                      
                    }
                    else
                    {
                        changeScoreSteps.Add(new ScoreChangeStepData()
                        {
                            CurrentScore = saveData.Score,
                            MaxScore = currentThreshold,
                            Level = saveData.Level,
                            NewScore = newScore
                        });

                        saveData.Score = newScore;

                        break;
                    }
                }
                
                result.Add(scoreChanges[i], changeScoreSteps);
            }

            saveDataProperty.Value = saveData;
            return result;
        }

        private static IReadOnlyList<ScoreChangeStepData> GetLevelUpSteps(ref ScoreAndLevelData saveData, ref ScoreChangeData scoreChange,
            IReadOnlyList<int> scoreThresholds)
        {
            var displayStates = new List<ScoreChangeStepData>(2);
            var currentThreshold = scoreThresholds[saveData.Level];
            var newScore = saveData.Score + scoreChange.Value;

            displayStates.Add(new ScoreChangeStepData()
            {
                CurrentScore = saveData.Score,
                MaxScore = currentThreshold,
                Level = saveData.Level,
                NewScore = currentThreshold
            });

            if (saveData.Level == scoreThresholds.Count - 1)
            {
                saveData.Score = currentThreshold;
                return displayStates;
            }

            saveData.Level++;
            saveData.Score = 0;

            scoreChange.Value = newScore - currentThreshold;

            displayStates.Add(new ScoreChangeStepData()
            {
                CurrentScore = 0,
                MaxScore = scoreThresholds[saveData.Level],
                Level = saveData.Level,
                NewScore = 0
            });
            
            return displayStates;
        }
        
        private static IReadOnlyList<ScoreChangeStepData> GetLevelDownSteps(ref ScoreAndLevelData saveData, ref ScoreChangeData scoreChange,
            IReadOnlyList<int> scoreThresholds)
        {
            var displayStates = new List<ScoreChangeStepData>(2);
            var currentThreshold = scoreThresholds[saveData.Level];
            var newScore = saveData.Score + scoreChange.Value;
            
            displayStates.Add(new ScoreChangeStepData()
            {
                CurrentScore = saveData.Score,
                MaxScore = currentThreshold,
                Level = saveData.Level,
                NewScore = 0
            });

            if (saveData.Level == 0)
            {
                saveData.Score = 0;
                return displayStates;
            }

            saveData.Level--;
            saveData.Score = scoreThresholds[saveData.Level];
                        
            scoreChange.Value = newScore;

            displayStates.Add(new ScoreChangeStepData()
            {
                CurrentScore = scoreThresholds[saveData.Level],
                MaxScore = scoreThresholds[saveData.Level],
                Level = saveData.Level,
                NewScore = scoreThresholds[saveData.Level]
            });

            return displayStates;
        }
    }
}