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
        private Mock<ICommentService> mockCommentService;
        private Mock<ICorpusRecordService> mockCorpusRecordService;
        private Mock<IWordFrequencyService> mockWordFrequencyService;
        private Mock<IRecordService> mockRecordService;
        private Mock<ICorpusTypeService> mockCorpusTypeService;
        private Mock<IConfiguration> mockConfiguration;
        private RecordsController recordController;
        public RecordsControllerTests()
        {
            mockCommentService = new Mock<ICommentService>();
            mockCorpusRecordService = new Mock<ICorpusRecordService>();
            mockWordFrequencyService = new Mock<IWordFrequencyService>();
            mockRecordService = new Mock<IRecordService>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            recordController = new RecordsController(
                mockCommentService.Object,
                mockCorpusRecordService.Object,
                mockWordFrequencyService.Object,
                mockRecordService.Object,
                mockCorpusTypeService.Object,
                mockConfiguration.Object
            );
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Not_Deleted()
        {
            //Act
            var result = await recordController.DeleteRecord(It.IsAny<int>());
            //Assert
            var contentResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_Ok_When_Deleted()
        {
            //Arrange          
            mockRecordService.Setup(m => m.DeleteRecordAsync(It.IsAny<int>(), "SampleConnectionString")).ReturnsAsync(true);
            //Act            
            var result = await recordController.DeleteRecord(It.IsAny<int>());
            //Assert
            var contentResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_RecordViewModel_Is_Null()
        {
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
            mockRecordService.Setup(m => m.AddRecordAsync(new Mock<RecordModel>().Object, It.IsAny<string>()))
                .Returns(Task.FromResult(-1));
            //Act            
            var result = await recordController.AddRecord(new Mock<RecordViewModel>().Object);
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding Record!", contentResultMessage);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_SaveCommentsResult_Is_False()
        {
            //Arrange          
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(1));
            mockCommentService
                .Setup(m => m.SaveCommentsAsync(new Mock<IEnumerable<CommentModel>>().Object, It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            //Act
            var recordViewModel = new RecordViewModel()
            {
                CommentViewModels = new Mock<IEnumerable<CommentViewModel>>().Object
            };
            var result = await recordController.AddRecord(recordViewModel);
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding Comments!", contentResultMessage);
        }
    }
}
