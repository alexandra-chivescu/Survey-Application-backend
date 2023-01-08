using Microsoft.AspNetCore.Mvc;
using Survey_App.ContextModels;
using Survey_App_be.Models;
using System.Diagnostics;

namespace Survey_App_be.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SurveyContext _context;

        public HomeController(ILogger<HomeController> logger, SurveyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public class UserRegisterDTO
        {
            public int id { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string type { get; set; }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult register([FromBody] UserRegisterDTO user)
        {

            var newUser = new Users
            {
                Id = user.id,
                email = user.email,
                password = user.password,
                type = user.type
            };

            // Add the entity to the context
            _context.Users.Add(newUser);

            // Save the changes to the database
            _context.SaveChanges();

            // Return the saved user
            return Ok(newUser);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.email == email);
            if (user == null || user.password != password)
            {
                _logger.LogWarning("Failed login attempt with email {Email}", email);
                return NotFound();
            }

            _logger.LogInformation("Successful login with email {Email}", email);
            return Ok(user);
        }

        [HttpGet]
        public IActionResult getUserByEmail(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.email == email);
            if (user == null )
            {
                _logger.LogWarning("Failed login attempt with email {Email}", email);
                return NotFound();
            }

            _logger.LogInformation("Successful login with email {Email}", email);
            return Ok(user);
        }

        [HttpGet]
        [Route("Surveys")]
        public IActionResult getSurveys()
        {
            var surveys = _context.Surveys;
            return Ok(surveys); 
        }

        [HttpGet("{id}")]
        [Route("Surveys")]
        public IActionResult getSurveyById(int id)
        {
            var survey = _context.Surveys.SingleOrDefault(survey => survey.Id == id);
            if(survey == null)
            {
                _logger.LogWarning("Failed to find survey with id {Id}", id);
                return NotFound();
            }
            return Ok(survey);
        }

        [HttpGet("{survey_id}")]
        [Route("Questions")]
        public IActionResult getQuestionsBySurveyId(int survey_id)
        {
            var questions = _context.Questions.SingleOrDefault(question => question.Survey_id == survey_id);
            if (questions == null)
            {
                _logger.LogWarning("Failed to find questions with surveyId {Survey_id}", survey_id);
                return NotFound();
            }
            return Ok(questions);
        }

        [HttpGet]
        [Route("Questions")]
        public IActionResult getQuestions()
        {
            var questions = _context.Questions;
            return Ok(questions);
        }

        [HttpGet]
        [Route("Answers")]
        public IActionResult getAnswers()
        {
            var answers = _context.Answers;
            return Ok(answers);
        }

        [HttpGet("{question_id}")]
        [Route("Answers")]
        public IActionResult getAnswersByQuestionId(int question_id)
        {
            var answers = _context.Answers.SingleOrDefault(answer => answer.Question_id == question_id);
            if (answers == null)
            {
                _logger.LogWarning("Failed to find answers with questionId {Question_id}", question_id);
                return NotFound();
            }
            return Ok(answers);
        }

        public class Answer
        {
            public int id { get; set; }
            public int question_id { get; set; }
            public string answer { get; set; }
            public int noResponses { get; set; }
        }

        [HttpPut]
        [Route("Answers")]
        public IActionResult modifyAnswer([FromBody] Answer answer)
        {
            var dbAnswer = _context.Answers.FirstOrDefault(a => a.Id == answer.id);
            if (dbAnswer == null)
            {
                return NotFound();
            }

            dbAnswer.Question_id = answer.question_id;
            dbAnswer.Answer = answer.answer;
            dbAnswer.NoResponses = answer.noResponses;
            _context.Answers.Update(dbAnswer);
            _context.SaveChanges();

            return Ok(dbAnswer); 
        }

        public class CompletedSurveyDTO
        {
            public int user_id { get; set; }
            public int survey_id { get; set; }
        }

        [HttpPost]
        [Route("CompletedSurvey")]
        public IActionResult completedSurvey([FromBody] CompletedSurveyDTO completedSurvey)
        {
   
            Console.WriteLine("ABABABABABA " + completedSurvey.survey_id + " " + completedSurvey.user_id);
            var newSurvey = new CompletedSurveys
            {
                User_id = completedSurvey.user_id,
                Survey_id = completedSurvey.survey_id
            };

            // Add the entity to the context
            _context.CompletedSurveys.Add(newSurvey);

            // Save the changes to the database
            _context.SaveChanges();

            // Return the saved survey
            return Ok(newSurvey);
        }

        [HttpGet("CompletedSurvey")]
        public IActionResult getCompletedSurveyByUserIdAndSurveyId([FromQuery]int user_id, [FromQuery]int survey_id)
        {
            var completedSurvey = _context.CompletedSurveys.SingleOrDefault(completedSurvey => completedSurvey.Survey_id == survey_id && completedSurvey.User_id == user_id);
            if (completedSurvey == null)
            {
                _logger.LogWarning("Failed to find completed survey by user id and survey id");
                return NotFound();
            }

            return Ok(completedSurvey);
        }

        public class Survey
        {
            public int id { get; set; }
            public string title { get; set; }
            public string creator { get; set; }
            public string start_Date { get; set; }
            public string end_Date { get; set; }
        }

        [HttpPut]
        [Route("Surveys")]
        public IActionResult changeEndDateForSurvey([FromBody] Survey survey)
        {
            var dbSurvey = _context.Surveys.FirstOrDefault(s => s.Id == survey.id);
            if (dbSurvey == null)
            {
                return NotFound();
            }

            dbSurvey.End_Date = survey.end_Date;
            _context.Surveys.Update(dbSurvey);
            _context.SaveChanges();

            return Ok(dbSurvey);

        }

        [HttpPost]
        [Route("Surveys")]
        public IActionResult createNewSurvey([FromBody] Survey survey)
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAA");

            Console.WriteLine("id " + survey.id);
            
            var newSurvey = new Surveys
            {
                Title = survey.title,
                Creator = survey.creator,
                Start_Date = survey.start_Date,
                End_Date = survey.end_Date
            };

            // Add the entity to the context
            _context.Surveys.Add(newSurvey);

            // Save the changes to the database
            _context.SaveChanges();

            // Return the saved survey
            return Ok(newSurvey);
        }

        public class Question
        {
            public int survey_id { get; set; }
            public string title { get; set; }
        }

        [HttpPost]
        [Route("Questions")]
        public IActionResult createNewQuestion([FromBody] Question question)
        {
            var newQuestion = new Questions
            {
                Survey_id = question.survey_id,
                Title = question.title
            };

            // Add the entity to the context
            _context.Questions.Add(newQuestion);

            // Save the changes to the database
            _context.SaveChanges();

            // Return the saved survey
            return Ok(newQuestion);
        }

        [HttpPost]
        [Route("Answers")]
        public IActionResult createNewAnswer([FromBody] Answer answer)
        {
      

            var newAnswer = new Answers
            {
                Question_id = answer.question_id,
                Answer = answer.answer,
                NoResponses = answer.noResponses
            };

            // Add the entity to the context
            _context.Answers.Add(newAnswer);

            // Save the changes to the database
            _context.SaveChanges();

            // Return the saved survey
            return Ok(newAnswer);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}