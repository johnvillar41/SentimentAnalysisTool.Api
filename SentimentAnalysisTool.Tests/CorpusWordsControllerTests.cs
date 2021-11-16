using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
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
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly CorpusWordsController corpusWordsController;
        public CorpusWordsControllerTests()
        {
            mockCorpusWordsService = new Mock<ICorpusWordsService>();
            mockConfiguration = new Mock<IConfiguration>();
            corpusWordsController = new CorpusWordsController(mockCorpusWordsService.Object, mockConfiguration.Object);
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
    }
}
