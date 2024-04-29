using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using hackernewsapi.Model;
using AutoMapper;
using System.Collections.Concurrent;
using hackernewsapi.Interfaces;

namespace hackernewsapi.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private IHackerNews _hackerNewsApi;
        private IMapper _mapper;

        private ConcurrentDictionary<int, StoryModel> _storyCache;
        public HackerNewsService([FromServices] IHackerNews hackerNewsApi, IMapper mapper)
        {
            _hackerNewsApi = hackerNewsApi;
            _mapper = mapper;
            _storyCache = new ConcurrentDictionary<int, StoryModel>();
        }

        public void CleanCache()
        {
            _storyCache.Clear();
        }

        public async Task<IEnumerable<OutputStoryModel>> GetOrderedStories(bool disableCache)
        {
            var outputStories = new List<OutputStoryModel>();

            using (var client = new HttpClient())
            {
                var bestStories = await _hackerNewsApi.GetStories();
                var stories = new List<StoryModel>();

                Array.Sort(bestStories);

                var tasks = bestStories.Select(async storyId =>
                {
                    StoryModel story = await GetStoryFromCache(storyId, disableCache);
                    stories.Add(story);
                }).ToList();

                Task.WaitAll(tasks.ToArray());

                outputStories = _mapper.Map<List<OutputStoryModel>>(stories);
            }

            return outputStories.OrderByDescending(s => s.score).Take(20);
        }

        private async Task<StoryModel> GetStoryFromCache(int storyId, bool disableCache)
        {
            StoryModel result;
            if (disableCache == true)
            {
                result = await _hackerNewsApi.GetStoryById(storyId);
            }
            else
            {
                _storyCache.TryGetValue(storyId, out result);

                if (result == null)
                {
                    result = await _hackerNewsApi.GetStoryById(storyId);
                    _storyCache.TryAdd(storyId, result);
                }
            }

            return result;
        }
    }

}