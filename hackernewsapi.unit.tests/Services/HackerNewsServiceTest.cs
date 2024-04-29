using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using hackernewsapi.Interfaces;
using hackernewsapi.Model;
using hackernewsapi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace hackernewsapi.unit.tests.Services
{
    [TestClass]
    public class HackerNewsServiceTest
    {
        private HackerNewsService _hackerNewsService;
        private Mock<IHackerNews> _mockHackerNewsApi;
        private Mock<IMapper> _mockMapper;

        public HackerNewsServiceTest(){
            _mockHackerNewsApi = new Mock<IHackerNews>();
            _mockMapper = new Mock<IMapper>();
            _hackerNewsService = new HackerNewsService(_mockHackerNewsApi.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task  GetOrderedStories_DisablingCache_ReturnsTwoOrderedStories()
        {
            //Arrange
            var firstOutputStory = new OutputStoryModel() { score = 100 };
            var secondOutputStory = new OutputStoryModel() { score = 50 };
            var outputs = new List<OutputStoryModel> { firstOutputStory, secondOutputStory }; 

            _mockHackerNewsApi.Setup(s => s.GetStories()).ReturnsAsync(new int[]{1, 2});
            _mockHackerNewsApi.Setup(s => s.GetStoryById(It.IsAny<int>())).ReturnsAsync(new StoryModel());
            _mockMapper.Setup(s => s
                                .Map<List<OutputStoryModel>>(It.IsAny<List<StoryModel>>()))
                                .Returns(outputs);

            //Act
            var stories = await _hackerNewsService.GetOrderedStories(false);

            //Asset
            Assert.IsNotNull(stories);
            Assert.AreEqual(2, stories.Count());
            Assert.AreEqual(100, stories.First().score);
            Assert.AreEqual(50, stories.Last().score);
        }

        [TestMethod]
        public async Task  GetOrderedStories_EnablingCache_ReturnsTwoOrderedStories()
        {
            //Arrange
            var firstOutputStory = new OutputStoryModel() { score = 100 };
            var secondOutputStory = new OutputStoryModel() { score = 50 };
            var outputs = new List<OutputStoryModel> { firstOutputStory, secondOutputStory }; 

            _mockHackerNewsApi.Setup(s => s.GetStories()).ReturnsAsync(new int[]{1, 2});
            _mockHackerNewsApi.Setup(s => s.GetStoryById(It.IsAny<int>())).ReturnsAsync(new StoryModel());
            _mockMapper.Setup(s => s
                                .Map<List<OutputStoryModel>>(It.IsAny<List<StoryModel>>()))
                                .Returns(outputs);

            //Act
            var stories = await _hackerNewsService.GetOrderedStories(true);

            //Asset
            Assert.IsNotNull(stories);
            Assert.AreEqual(2, stories.Count());
            Assert.AreEqual(100, stories.First().score);
            Assert.AreEqual(50, stories.Last().score);
        }

        [TestMethod]
        public void  CleanCache_Default_ClearServiceCache()
        {
            //Act
            _hackerNewsService.CleanCache();

            //Asset
            Assert.IsNotNull(_hackerNewsService);
        }
    }
}
