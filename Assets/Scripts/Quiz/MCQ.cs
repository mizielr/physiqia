using System;
using UnityEngine;

namespace Physiqia.Quiz.MCQ
{
    public class MCQ
    {
        private string id;
        private string question;
        private string[] answers;
        public byte category;
        private byte difficulty;
        private string latexEquation;
        private bool[] latexOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        /// <param name="answers"></param>
        /// <param name="category"></param>
        /// <param name="difficulty"></param>
        /// <param name="latexEquation"></param>
        /// <param name="latexOptions"></param>
        public MCQ(string id, string question, string[] answers, byte category, byte difficulty, string latexEquation = null, bool[] latexOptions = null)
        {
            this.id = id;
            this.question = question;
            this.answers = answers;
            this.category = category;
            this.difficulty = difficulty;
            this.latexEquation = latexEquation;
            this.latexOptions = latexOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetQuestion()
        {
            return question;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte GetCategory()
        {
            return category;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetAnswers()
        {
            return answers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool hasLatexEquation()
        {
            return !string.IsNullOrEmpty(latexEquation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLatexEquation()
        {
            return latexEquation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetGameID()
        {
            return id;
        }
    }
}
