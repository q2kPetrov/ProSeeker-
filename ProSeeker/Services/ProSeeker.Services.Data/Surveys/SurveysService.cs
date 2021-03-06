﻿namespace ProSeeker.Services.Data.Quizz
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Quizzes;
    using ProSeeker.Web.ViewModels.Surveys;
    using ProSeeker.Web.ViewModels.Surveys.Questions;

    public class SurveysService : ISurveysService
    {
        private readonly IDeletableEntityRepository<Survey> surveysRepository;
        private readonly IDeletableEntityRepository<Question> questionsRepository;
        private readonly IDeletableEntityRepository<Answer> answersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<UserSurvey> userSurveysRepository;

        public SurveysService(
            IDeletableEntityRepository<Survey> surveysRepository,
            IDeletableEntityRepository<Question> questionsRepository,
            IDeletableEntityRepository<Answer> answersRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<UserSurvey> userSurveysRepository)
        {
            this.surveysRepository = surveysRepository;
            this.questionsRepository = questionsRepository;
            this.answersRepository = answersRepository;
            this.usersRepository = usersRepository;
            this.userSurveysRepository = userSurveysRepository;
        }

        public async Task AddUserToSurveyAsync(string userId, string surveyId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == userId);

            var userSurvey = new UserSurvey
            {
                User = user,
                UserId = user.Id,
                SurveyId = surveyId,
            };

            await this.userSurveysRepository.AddAsync(userSurvey);
            await this.userSurveysRepository.SaveChangesAsync();
        }

        public async Task<string> CreateAnswerAsync(string questionId, string answerText)
        {
            var newAnswer = new Answer
            {
                Text = answerText,
                QuestionId = questionId,
            };

            await this.answersRepository.AddAsync(newAnswer);
            await this.answersRepository.SaveChangesAsync();

            return newAnswer.Id;
        }

        public async Task<string> CreateQuestionAsync(NewQuestionInputModel inputModel)
        {
            var newQuestion = new Question
            {
                Number = inputModel.Number,
                SurveyId = inputModel.SurveyId,
                Text = inputModel.Text,
            };

            await this.questionsRepository.AddAsync(newQuestion);
            await this.questionsRepository.SaveChangesAsync();

            return newQuestion.Id;
        }

        public async Task<string> CreateSurveyAsync(NewSurveyInputModel inputModel)
        {
            var newSurvey = new Survey
            {
                Title = inputModel.Title,
            };

            await this.surveysRepository.AddAsync(newSurvey);
            await this.surveysRepository.SaveChangesAsync();

            return newSurvey.Id;
        }

        public async Task DeleteAllAnswersAsync(string questionId)
        {
            var answers = await this.answersRepository
                .AllWithDeleted()
                .Where(x => x.QuestionId == questionId)
                .ToListAsync();

            foreach (var answer in answers)
            {
                this.answersRepository.Delete(answer);
                await this.answersRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAllQuestionsAsync(string surveyId)
        {
            var questions = await this.questionsRepository
                .AllWithDeleted()
                .Where(x => x.SurveyId == surveyId)
                .ToListAsync();

            foreach (var question in questions)
            {
                this.questionsRepository.Delete(question);
                await this.questionsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAnswerAsync(string answerId)
        {
            var answer = await this.answersRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == answerId);

            this.answersRepository.Delete(answer);
            await this.answersRepository.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(string questionId)
        {
            var question = await this.questionsRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == questionId);

            this.questionsRepository.Delete(question);
            await this.questionsRepository.SaveChangesAsync();
        }

        public async Task DeleteSurveyAsync(string surveyId)
        {
            var survey = await this.surveysRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == surveyId);

            this.surveysRepository.Delete(survey);
            await this.surveysRepository.SaveChangesAsync();
        }

        public async Task EditSurveyAsync(string surveyId, string title)
        {
            var survey = await this.surveysRepository.All().FirstOrDefaultAsync(x => x.Id == surveyId);
            survey.Title = title;
            this.surveysRepository.Update(survey);
            await this.surveysRepository.SaveChangesAsync();
        }

        public async Task EditAnswerAsync(string answerId, string newText)
        {
            var existingAnswer = await this.answersRepository.All().FirstOrDefaultAsync(x => x.Id == answerId);

            existingAnswer.Text = newText;
            this.answersRepository.Update(existingAnswer);
            await this.answersRepository.SaveChangesAsync();
        }

        public async Task EditQuestionAsync(string questionId, string newText)
        {
            var question = await this.questionsRepository.All().FirstOrDefaultAsync(x => x.Id == questionId);
            question.Text = newText;
            this.questionsRepository.Update(question);
            await this.questionsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAnswersByQuestionIdAsync<T>(string questionId)
        {
            var answers = await this.answersRepository
                .All()
                .Where(x => x.QuestionId == questionId)
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

            return answers;
        }

        public async Task<IEnumerable<T>> GetAllQuestionsBySurveyIdAsync<T>(string surveyId)
        {
            var questions = await this.questionsRepository
                .All()
                .Where(x => x.SurveyId == surveyId)
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

            return questions;
        }

        public async Task<IEnumerable<T>> GetAllSurveysAsync<T>()
        {
            var surveys = await this.surveysRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

            return surveys;
        }

        public async Task<IEnumerable<T>> GetAnswersByQuestionIdAsync<T>(string questionId)
        {
            var answers = await this.answersRepository
                .All()
                .Where(x => x.QuestionId == questionId)
                .To<T>()
                .ToListAsync();

            return answers;
        }

        public Task<int> GetQuestionNumberBySurveyIdAsync(string surveyId)
        {
            var questionNumber = this.questionsRepository
                .All()
                .Where(x => x.SurveyId == surveyId)
                .CountAsync();

            return questionNumber;
        }

        public async Task<IEnumerable<T>> GetQuestionsBySurveyIdAsync<T>(string surveyId)
        {
            var quizQuestions = await this.questionsRepository
                .All()
                .Where(x => x.SurveyId == surveyId)
                .To<T>()
                .ToListAsync();

            return quizQuestions;
        }

        public async Task<T> GetQuestionByIdAsync<T>(string questionId)
        {
            var questionText = await this.questionsRepository
                .All()
                .Where(x => x.Id == questionId)
                .To<T>()
                .FirstOrDefaultAsync();

            return questionText;
        }

        public async Task<T> GetSurveyByIdAsync<T>(string surveyId)
        {
            var survey = await this.surveysRepository
                .All()
                .Where(x => x.Id == surveyId)
                .To<T>()
                .FirstOrDefaultAsync();

            return survey;
        }

        public async Task<string> GetSurveyTitleByIdAsync(string surveyId)
        {
            var surveyTitle = await this.surveysRepository
                .All()
                .Where(x => x.Id == surveyId)
                .Select(x => x.Title)
                .FirstOrDefaultAsync();

            return surveyTitle;
        }

        public async Task<bool> HasItBeenCompletedByThisUser(string userId)
        {
            var userSurvey = await this.userSurveysRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return userSurvey != null;
        }

        public async Task<T> GetSingleAnswerAsync<T>(string answerId)
        {
            var answer = await this.answersRepository
                .All()
                .Where(x => x.Id == answerId)
                .To<T>()
                .FirstOrDefaultAsync();

            return answer;
        }
    }
}
