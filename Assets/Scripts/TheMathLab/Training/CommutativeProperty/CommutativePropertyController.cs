using System.Collections.Generic;
using UnityEngine;

namespace Physiqia.MathLab.Training.CommutativeProperty
{
    public class CommutativePropertyController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CommutativePropertyTrainingConfig config;

        [SerializeField]
        private ConfigPanel configPanel;

        [SerializeField]
        private QuestionPanel questionPanel;

        [SerializeField]
        private ResultPanel resultPanel;

        [SerializeField]
        private TimerController timerController;

        private QuestionGenerator generator;
        private QuestionValidator validator = new QuestionValidator();
        private ExerciseStatistics stats = new ExerciseStatistics();

        private List<MathOperation> activeOperations;
        private DifficultyLevel difficulty;
        private AssistanceLevel assistance;
        private int letterCount;
        private int totalQuestions;
        private int currentQuestionIndex;

        private GeneratedQuestion currentQuestion;
        private float questionStartTime;

        /// <summary>
        ///
        /// </summary>
        private void Start()
        {
            configPanel.OnStartRequested += StartExercise;
            questionPanel.OnValidateClicked += OnValidate;
            resultPanel.OnRestart += RestartExercise;
            resultPanel.OnBackToConfig += BackToConfig;

            timerController.OnQuestionTimerExpired += OnQuestionExpired;

            ShowConfig();
        }

        /// <summary>
        ///
        /// </summary>
        private void ShowConfig()
        {
            configPanel.Show();
            questionPanel.Hide();
            resultPanel.Hide();
            timerController.Pause();
        }

        /// <summary>
        ///
        /// </summary>
        private void StartExercise()
        {
            activeOperations = configPanel.GetSelectedOperations();
            difficulty = configPanel.CurrentDifficulty;
            assistance = configPanel.CurrentAssistance;
            letterCount = configPanel.LetterCount;
            totalQuestions = configPanel.QuestionCount;

            generator = new QuestionGenerator(config, letterCount);
            stats = new ExerciseStatistics();
            currentQuestionIndex = 0;

            timerController.Configure(
                configPanel.QuestionTimerDuration,
                configPanel.GlobalReferenceDuration,
                configPanel.QuestionTimerEnabled,
                configPanel.GlobalTimerEnabled
            );

            timerController.ResetGlobal();
            timerController.Resume();

            configPanel.Hide();
            resultPanel.Hide();
            NextQuestion();
        }

        /// <summary>
        ///
        /// </summary>
        private void NextQuestion()
        {
            if (currentQuestionIndex >= totalQuestions)
            {
                EndExercise();
                return;
            }

            currentQuestionIndex++;
            currentQuestion = generator.Generate(activeOperations, difficulty, assistance);

            string progress = $"Question {currentQuestionIndex} / {totalQuestions}";
            questionPanel.ShowQuestion(
                progress,
                currentQuestion.DisplayText,
                currentQuestion.ExpectedAnswer,
                assistance,
                difficulty
            );

            if (configPanel.QuestionTimerEnabled)
                timerController.StartQuestionTimer(configPanel.QuestionTimerDuration);
            else
                timerController.StopQuestionTimer();

            questionStartTime = Time.time;
        }

        /// <summary>
        ///
        /// </summary>
        private void OnValidate()
        {
            float timeTaken = Time.time - questionStartTime;
            timerController.StopQuestionTimer();

            MathTerm student = questionPanel.GetStudentAnswer();
            bool correct = validator.Validate(student, currentQuestion.ExpectedAnswer);

            if (correct)
            {
                stats.RegisterCorrect(timeTaken);
                questionPanel.ShowFeedback("Correct !", Color.green);
            }
            else
            {
                stats.RegisterWrong(timeTaken);
                questionPanel.ShowFeedback(
                    $"Incorrect. Answer : {currentQuestion.ExpectedAnswer}",
                    Color.red
                );
            }

            Invoke(nameof(NextQuestion), 0f);
        }

        /// <summary>
        ///
        /// </summary>
        private void OnQuestionExpired()
        {
            float timeTaken = configPanel.QuestionTimerDuration;
            stats.RegisterExpired(timeTaken);
            questionPanel.ShowFeedback("Time's up!", Color.yellow);
            Invoke(nameof(NextQuestion), 1.0f);
        }

        /// <summary>
        ///
        /// </summary>
        private void EndExercise()
        {
            timerController.Pause();
            timerController.StopQuestionTimer();

            var result = stats.BuildResult(
                totalQuestions,
                configPanel.GlobalReferenceDuration,
                difficulty,
                assistance,
                letterCount,
                activeOperations
            );

            questionPanel.Hide();
            resultPanel.Show(result, timerController);
        }

        /// <summary>
        ///
        /// </summary>
        private void RestartExercise()
        {
            StartExercise();
        }

        /// <summary>
        ///
        /// </summary>
        private void BackToConfig()
        {
            CancelInvoke();
            ShowConfig();
        }

        /// <summary>
        ///
        /// </summary>
        private void Update()
        {
            if (!questionPanel.gameObject.activeSelf)
                return;

            string qTime = configPanel.QuestionTimerEnabled
                ? timerController.FormatTime(timerController.QuestionTimeRemaining)
                : "--:--";

            string gTime = timerController.FormatTime(timerController.GlobalElapsed);
            string rTime = timerController.FormatTime(timerController.ReferenceTime);

            questionPanel.UpdateTimers($"Time : {qTime}", $"Time : {gTime}", $"Ref. : {rTime}");
        }

        /// <summary>
        ///
        /// </summary>
        private void OnDestroy()
        {
            CancelInvoke();
        }
    }
}
