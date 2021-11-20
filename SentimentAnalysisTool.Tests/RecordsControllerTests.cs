using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
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
        private Mock<IServiceWrapper> mockServiceWrapper;
        private RecordsController recordController;

        public RecordsControllerTests()
        {
            mockCommentService = new Mock<ICommentService>();
            mockCorpusRecordService = new Mock<ICorpusRecordService>();
            mockWordFrequencyService = new Mock<IWordFrequencyService>();
            mockRecordService = new Mock<IRecordService>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockServiceWrapper = new Mock<IServiceWrapper>();
            recordController = new RecordsController(
                mockCommentService.Object,
                mockCorpusRecordService.Object,
                mockWordFrequencyService.Object,
                mockRecordService.Object,
                mockCorpusTypeService.Object,
                mockServiceWrapper.Object,
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
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockDbTransaction = It.IsAny<DbTransaction>();
            mockServiceWrapper
                .Setup(m => m.OpenConnection(It.IsAny<string>()))
                .Returns(mockSqlConnection);
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
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
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockDbTransaction = It.IsAny<DbTransaction>();
            mockServiceWrapper
                .Setup(m => m.OpenConnection(It.IsAny<string>()))
                .Returns(mockSqlConnection);
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(1));
            mockCommentService
                 .Setup(m => m.SaveCommentsAsync(It.IsAny<IEnumerable<CommentModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                 .Returns(Task.FromResult(false));

            //Act
            var recordViewModel = Mock.Of<RecordViewModel>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel>>();
            var result = await recordController.AddRecord(recordViewModel);
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding Comments!", contentResultMessage);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddCorpusRecord_Is_False()
        {
            //Arrange          
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockDbTransaction = It.IsAny<DbTransaction>();
            mockServiceWrapper
                .Setup(m => m.OpenConnection(It.IsAny<string>()))
                .Returns(mockSqlConnection);
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(1));
            mockCommentService
                 .Setup(m => m.SaveCommentsAsync(It.IsAny<IEnumerable<CommentModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                 .Returns(Task.FromResult(true));
            mockCorpusRecordService
                .Setup(m => m.AddCorpusRecordAsync(It.IsAny<IEnumerable<CorpusRecordModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(false));

            //Act
            var recordViewModel = Mock.Of<RecordViewModel>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel>>();
            recordViewModel.CorpusRecordViewModels = Mock.Of<IEnumerable<CorpusRecordViewModel>>();
            var result = await recordController.AddRecord(recordViewModel);
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding Corpus!", contentResultMessage);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddFrequency_Is_False()
        {
            //Arrange          
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockDbTransaction = It.IsAny<DbTransaction>();
            mockServiceWrapper
                .Setup(m => m.OpenConnection(It.IsAny<string>()))
                .Returns(mockSqlConnection);
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(1));
            mockCommentService
                 .Setup(m => m.SaveCommentsAsync(It.IsAny<IEnumerable<CommentModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                 .Returns(Task.FromResult(true));
            mockCorpusRecordService
                .Setup(m => m.AddCorpusRecordAsync(It.IsAny<IEnumerable<CorpusRecordModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(true));
            mockWordFrequencyService
               .Setup(m => m.AddWordFrequenciesAsync(It.IsAny<IEnumerable<WordFrequencyModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
               .Returns(Task.FromResult(false));

            //Act
            var recordViewModel = Mock.Of<RecordViewModel>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel>>();
            recordViewModel.CorpusRecordViewModels = Mock.Of<IEnumerable<CorpusRecordViewModel>>();
            recordViewModel.WordFrequencyViewModels = Mock.Of<IEnumerable<WordFrequencyViewModel>>();
            var result = await recordController.AddRecord(recordViewModel);
            //Assert
            var contentResult = Assert.IsType<BadRequestObjectResult>(result);
            var contentResultMessage = contentResult.Value;
            Assert.Equal(result, contentResult);
            Assert.Equal("Error Adding WordFrequencies!", contentResultMessage);
        }
        [Fact]
        public async Task Should_Return_Ok_When_All_Is_True()
        {            
            //Arrange          
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockDbTransaction = It.IsAny<DbTransaction>();
            mockServiceWrapper
                .Setup(m => m.OpenConnection(It.IsAny<string>()))
                .Returns(mockSqlConnection);
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<DbTransaction>(),It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(1));
            mockCommentService
                 .Setup(m => m.SaveCommentsAsync(It.IsAny<IEnumerable<CommentModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                 .Returns(Task.FromResult(true));
            mockCorpusRecordService
                .Setup(m => m.AddCorpusRecordAsync(It.IsAny<IEnumerable<CorpusRecordModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(true));
            mockWordFrequencyService
               .Setup(m => m.AddWordFrequenciesAsync(It.IsAny<IEnumerable<WordFrequencyModel>>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
               .Returns(Task.FromResult(true));

            //Act
            var recordViewModel = Mock.Of<RecordViewModel>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel>>();
            recordViewModel.CorpusRecordViewModels = Mock.Of<IEnumerable<CorpusRecordViewModel>>();
            recordViewModel.WordFrequencyViewModels = Mock.Of<IEnumerable<WordFrequencyViewModel>>();
            var result = await recordController.AddRecord(recordViewModel);
            //Assert
            var contentResult = Assert.IsType<OkResult>(result);
            Assert.Equal(result, contentResult);
        }
    }
}
