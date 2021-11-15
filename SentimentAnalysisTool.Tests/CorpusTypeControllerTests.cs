using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SentimentAnalysisTool.Tests
{
    public class CorpusTypeControllerTests
    {
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly CorpusTypeController corpusTypeController;
        public CorpusTypeControllerTests()
        {
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            corpusTypeController = new CorpusTypeController(
                mockCorpusTypeService.Object,
                mockConfiguration.Object);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_CorpusTypeViewModel_Is_Null()
        {
            //Act
            var result = await corpusTypeController.AddCorpusType(null);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Should_Return_Ok_When_AddCorpusType_Is_True()
        {
            //Arrange
            mockCorpusTypeService
                .Setup(m => m.AddCorpusTypeAsync(It.IsAny<CorpusTypeModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await corpusTypeController.AddCorpusType(Mock.Of<CorpusTypeViewModel>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddCorpusType_Is_False()
        {
            //Arrange
            mockCorpusTypeService
                .Setup(m => m.AddCorpusTypeAsync(It.IsAny<CorpusTypeModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await corpusTypeController.AddCorpusType(Mock.Of<CorpusTypeViewModel>());
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task Should_Return_Ok_When_DeleteCorpusType_Is_True()
        {
            //Arrange
            mockCorpusTypeService
                .Setup(m => m.DeleteCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await corpusTypeController.DeleteCorpusType(It.IsAny<int>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_DeleteCorpusType_Is_False()
        {
            //Arrange
            mockCorpusTypeService
                .Setup(m => m.DeleteCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await corpusTypeController.DeleteCorpusType(It.IsAny<int>());
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
