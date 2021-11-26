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
        [Fact]
        public async Task Should_Return_Ok_If_AddSlangRecords_Is_True()
        {
            //Arrange
            var mockListSlangRecordsViewModel = new List<SlangRecordViewModel>()
            {
                Mock.Of<SlangRecordViewModel>(),
                Mock.Of<SlangRecordViewModel>()
            };            
            mockCorpusTypeService
               .Setup(m => m.FindCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
               .Returns(Task.FromResult(Mock.Of<CorpusTypeModel>()));
            mockSlangRecordsService
                .Setup(m => m.AddSlangRecordAsync(It.IsAny<IEnumerable<SlangRecordModel>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            //Act
            var result = await slangRecordsController.AddSlangRecords(mockListSlangRecordsViewModel);
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]        
        public async Task Should_Return_BadRequest_If_AddSlangRecords_Is_False()
        {
            //Arrange
            var mockListSlangRecordViewModels = new List<SlangRecordViewModel>()
            {
                Mock.Of<SlangRecordViewModel>(),
                Mock.Of<SlangRecordViewModel>()
            };
            mockCorpusTypeService
               .Setup(m => m.FindCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
               .ReturnsAsync(It.IsAny<CorpusTypeModel>());           
            //Act
            var result = await slangRecordsController.AddSlangRecords(mockListSlangRecordViewModels);
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
        public async Task Should_Return_BadRequest_When_DeleteSlangRecord_Is_False()
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
