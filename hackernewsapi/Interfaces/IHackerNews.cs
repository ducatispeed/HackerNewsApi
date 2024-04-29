using System.Threading.Tasks;
using hackernewsapi.Model;
using Refit;

namespace hackernewsapi.Interfaces
{
    public interface IHackerNews
    {
        [Get("/v0/beststories.json")]
        Task<int[]> GetStories();

        [Get("/v0/item/{id}.json")]
        Task<StoryModel> GetStoryById(int id);
    }
}