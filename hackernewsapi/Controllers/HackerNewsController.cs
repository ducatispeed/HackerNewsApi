using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hackernewsapi.Services;
using hackernewsapi.Model;
using Microsoft.AspNetCore.Http;

namespace hackernewsapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private IHackerNewsService _hackerNewsService;
        public HackerNewsController( IHackerNewsService hackerNewsService){
            _hackerNewsService = hackerNewsService;
        }

        /// <summary>
        ///     Will get back the first 20 news
        /// </summary>
        /// <returns>
        ///     A list of OutputStory ordered by score
        /// </returns>
        /// <response code="200">The best hacker news</response>
        /// <response code="500">Server internal error</response>
        [HttpGet]
        [Route("/hackernews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(IEnumerable<OutputStoryModel>))]
        public async Task<IEnumerable<OutputStoryModel>> Get()
        {
            var disableCache = false;
            bool.TryParse(Request.Headers["DisableCache"], out disableCache);

            return await _hackerNewsService.GetOrderedStories(disableCache);
        }


        /// <summary>
        ///     Clean the news cache
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /clean
        /// </remarks>        
        /// <response code="200">Cache cleaned successfully</response>
        /// <response code="500">Server internal error</response>
        [HttpGet]
        [Route("/clean")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CleanCache()
        {
            _hackerNewsService.CleanCache();
            return Ok();
        }
    }
}
