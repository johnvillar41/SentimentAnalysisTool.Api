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
    public class CorpusRecordsControllerTests
    {
        private readonly Mock<ICorpusRecordService> mockCorpusRecordService;
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IRecordService> mockRecordService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly CorpusRecordsController corpusRecordsController;
        public CorpusRecordsControllerTests()
        {
            mockCorpusRecordService = new Mock<ICorpusRecordService>();
            mockRecordService = new Mock<IRecordService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            corpusRecordsController = new CorpusRecordsController(
                mockCorpusRecordService.Object,
                mockRecordService.Object,
                mockCorpusTypeService.Object,
                mockConfiguration.Object);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_CorpusRecordViewModel_Is_Null()
        {
            //Act
            var result = await corpusRecordsController.AddCorpusRecord(null);
            //Assert
            var message = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Empty Corpus!", message.Value);
        }
        [Fact]
        public async Task Should_Return_Ok_When_AddCorpusRecordAsync_Returns_True()
        {
            //Arrange
            mockCorpusRecordService
                .Setup(m => m.AddCorpusRecordAsync(It.IsAny<CorpusRecordModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await corpusRecordsController.AddCorpusRecord(Mock.Of<CorpusRecordViewModel>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddCorpusRecordAsync_Returns_False()
        {
            //Arrange
            mockCorpusRecordService
                .Setup(m => m.AddCorpusRecordAsync(It.IsAny<CorpusRecordModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await corpusRecordsController.AddCorpusRecord(Mock.Of<CorpusRecordViewModel>());
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
