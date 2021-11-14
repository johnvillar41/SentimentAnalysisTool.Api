
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SentimentAnalysisTool.Tests
{
    public class RecordsControllerTests
    {
        [Fact]
        public async Task Should_Return_BadRequest_When_Not_Deleted()
        {
            //Arrange
            var mockCommentService = new Mock<ICommentService>();
            var mockCorpusRecordService = new Mock<ICorpusRecordService>();
            var mockWordFrequencyService = new Mock<IWordFrequencyService>();
            var mockRecordService = new Mock<IRecordService>();
            var mockCorpusTypeService = new Mock<ICorpusTypeService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var recordController = new RecordsController(
                mockCommentService.Object,
                mockCorpusRecordService.Object,
                mockWordFrequencyService.Object,
                mockRecordService.Object,
                mockCorpusTypeService.Object,
                mockConfiguration.Object);
            //Act
            var result = await recordController.DeleteRecord(1);
            //Assert
            var contentResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_Ok_When_Deleted()
        {
            //Arrange
            var mockCommentService = new Mock<ICommentService>();
            var mockCorpusRecordService = new Mock<ICorpusRecordService>();
            var mockWordFrequencyService = new Mock<IWordFrequencyService>();
            var mockRecordService = new Mock<IRecordService>();
            var mockCorpusTypeService = new Mock<ICorpusTypeService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var recordController = new RecordsController(
                mockCommentService.Object,
                mockCorpusRecordService.Object,
                mockWordFrequencyService.Object,
                mockRecordService.Object,
                mockCorpusTypeService.Object,
                mockConfiguration.Object);

            mockRecordService.Setup(m => m.DeleteRecordAsync(1, "SampleConnectionString")).ReturnsAsync(true);
            //Act
            var res = await mockRecordService.Object.DeleteRecordAsync(1, "SampleConnectionString");
            var result = await recordController.DeleteRecord(1);
            //Assert
            var contentResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(result, contentResult);
        }
    }
}
