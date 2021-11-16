using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SentimentAnalysisTool.Tests
{
    public class CorpusWordsControllerTests
    {
        private readonly Mock<ICorpusWordsService> mockCorpusWordsService;
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly CorpusWordsController corpusWordsController;
        public CorpusWordsControllerTests()
        {
            mockCorpusWordsService = new Mock<ICorpusWordsService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            corpusWordsController = new CorpusWordsController(
                mockCorpusWordsService.Object, 
                mockConfiguration.Object,
                mockCorpusTypeService.Object);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_FetchCorpusWordsAsync_Returns_Zero()
        {
            //Arrange
            mockCorpusWordsService
                .Setup(m => m.FetchCorpusWordsAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<CorpusWordModel>());

            //Act
            var result = await corpusWordsController.FetchAllCorpusWords(It.IsAny<int>());
            //Assert
            var message = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Empty Corpus List!", message.Value);
        }
        [Fact]
        public async Task Should_Return_Ok_When_FetchCorpusWordsAsync_Returns_CorpusWords()
        {
            //Arrange
            var mockCorpusWords = new List<CorpusWordModel>()
            {
                Mock.Of<CorpusWordModel>(),
                Mock.Of<CorpusWordModel>(),
            };
            mockCorpusWordsService
                .Setup(m => m.FetchCorpusWordsAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(mockCorpusWords);

            //Act
            var result = await corpusWordsController.FetchAllCorpusWords(It.IsAny<int>());
            //Assert
            var message = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockCorpusWords, message.Value);
        }
        [Fact]
        public async Task Should_Return_Ok_When_Successfully_Added_CorpusWords()
        {
            //Arrange
            var mockCorpusList = new List<CorpusWordViewModel>()
            {
                Mock.Of<CorpusWordViewModel>(),
                Mock.Of<CorpusWordViewModel>()
            };
            mockCorpusWordsService
                .Setup(m => m.AddCorpusWordsAsync(It.IsAny<IEnumerable<CorpusWordModel>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await corpusWordsController.AddCorpusWords(mockCorpusList);
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_Unsuccessfully_Added_CorpusWords()
        {
            //Arrange
            var mockCorpusList = new List<CorpusWordViewModel>()
            {
                Mock.Of<CorpusWordViewModel>(),
                Mock.Of<CorpusWordViewModel>()
            };
            mockCorpusWordsService
                .Setup(m => m.AddCorpusWordsAsync(It.IsAny<IEnumerable<CorpusWordModel>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await corpusWordsController.AddCorpusWords(mockCorpusList);
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
