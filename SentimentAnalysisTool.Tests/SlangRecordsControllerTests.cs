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
    public class SlangRecordsControllerTests
    {
        private Mock<ISlangRecordsService> mockSlangRecordsService;
        private Mock<ICorpusTypeService> mockCorpusTypeService;
        private Mock<IConfiguration> mockConfiguration;
        private SlangRecordsController slangRecordsController;
        public SlangRecordsControllerTests()
        {
            mockSlangRecordsService = new Mock<ISlangRecordsService>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            slangRecordsController = new SlangRecordsController(
                mockSlangRecordsService.Object,
                mockConfiguration.Object,
                mockCorpusTypeService.Object);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_AddSlangRecord_Is_Null()
        {
            //Act
            var result = await slangRecordsController.AddSlangRecord(null);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Should_Return_Ok_If_AddSlangRecord_Is_True()
        {
            //Arrange
            mockSlangRecordsService
                .Setup(m => m.AddSlangRecordAsync(It.IsAny<SlangRecordModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
                
            //Act
            var result = await slangRecordsController.AddSlangRecord(Mock.Of<SlangRecordViewModel>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
