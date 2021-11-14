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
            var result = await recordController.DeleteRecord(1);
            //Assert
            var contentResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_RecordViewModel_Is_Null()
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
            var result = await recordController.AddRecord(null);
            //Assert
            var contentResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_PrimaryKey_Is_Less_Than_Zero()
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

            mockRecordService.Setup(m => m.AddRecordAsync(new RecordModel(), "SampleConnectionString"))
                .ReturnsAsync(-1);
            //Act            
            var result = await recordController.AddRecord(new RecordViewModel());
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding Record!", contentResultMessage);
        }
        [Fact]//TODO FIX TEST
        public async Task Should_Return_BadRequest_When_SaveCommentsResult_Is_False()
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

            mockRecordService.Setup(m => m.AddRecordAsync(new RecordModel(), "SampleConnectionString"))
                .ReturnsAsync(1);
            mockCommentService.Setup(m => m.SaveCommentsAsync(new List<CommentModel>(), "SampleConnectionString"))
                .ReturnsAsync(false);
            //Act            
            var result = await recordController.AddRecord(new RecordViewModel());
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding Comments!", contentResultMessage);
        }
    }
}
