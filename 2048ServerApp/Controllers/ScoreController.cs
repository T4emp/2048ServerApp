using _2048ServerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace _2048ServerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private static object LockObject = new object();

        public ScoreController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public int GetMax()
        {
            var maxScore = _dataContext.ScoreList.OrderByDescending(x => x.Result).FirstOrDefault();
            if (maxScore != null)
            {
                return maxScore.Result;
            }

            return 0;
        }

        [HttpPost("{id}/{result}")]
        public void Update(Guid id, int result)
        {
            lock (LockObject)
            {
                var score = _dataContext.ScoreList.FirstOrDefault(x => x.Id == id);
                if (score == null)
                {
                    score = new Score
                    {
                        Id = id,
                        Result = result,
                        Date = DateTime.Now
                    };

                    _dataContext.ScoreList.Add(score);
                }
                else
                {
                    score.Result = result;
                    score.Date = DateTime.Now;

                    _dataContext.Update(score);
                }

                _dataContext.SaveChanges();
            }
        }
    }
}