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
        private readonly Mock<ISlangRecordsService> mockSlangRecordsService;
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly SlangRecordsController slangRecordsController;
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
        [Fact]
        public async Task Should_Return_BadRequest_If_AddSlangRecord_Is_False()
        {
            //Arrange
            mockSlangRecordsService
                .Setup(m => m.AddSlangRecordAsync(It.IsAny<SlangRecordModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = await slangRecordsController.AddSlangRecord(Mock.Of<SlangRecordViewModel>());
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_AddSlangRecords_Is_Null()
        {
            //Act
            var result = await slangRecordsController.AddSlangRecords(null);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact(Skip = "Can't Test Task.WhenAll")]
        public async Task Should_Return_Ok_If_AddSlangRecords_Is_True()
        {
            //Arrange
            mockCorpusTypeService
               .Setup(m => m.FindCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
               .Returns(Task.FromResult(Mock.Of<CorpusTypeModel>()));
            mockSlangRecordsService
                .Setup(m => m.AddSlangRecordAsync(Mock.Of<IEnumerable<SlangRecordModel>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await slangRecordsController.AddSlangRecords(Mock.Of<IEnumerable<SlangRecordViewModel>>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact(Skip = "Can't Test Task.WhenAll")]        
        public async Task Should_Return_BadRequest_If_AddSlangRecords_Is_False()
        {
            //Arrange
            mockCorpusTypeService
               .Setup(m => m.FindCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
               .ReturnsAsync(Mock.Of<CorpusTypeModel>());           
            //Act
            var result = await slangRecordsController.AddSlangRecords(Mock.Of<IEnumerable<SlangRecordViewModel>>());
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task Should_Return_Ok_When_DeleteSlangRecord_Is_True()
        {
            //Arrange
            mockSlangRecordsService
                .Setup(m => m.DeleteSlangRecordAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await slangRecordsController.DeleteSlangRecord(It.IsAny<int>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_Ok_When_DeleteSlangRecord_Is_False()
        {
            //Arrange
            mockSlangRecordsService
                .Setup(m => m.DeleteSlangRecordAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await slangRecordsController.DeleteSlangRecord(It.IsAny<int>());
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
