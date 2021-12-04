using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Api.Helpers;
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
        private readonly Mock<ICommentService> mockCommentService;
        private readonly Mock<ICorpusRecordService> mockCorpusRecordService;
        private readonly Mock<IWordFrequencyService> mockWordFrequencyService;
        private readonly Mock<IRecordService> mockRecordService;
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IFileHelper> mockFileHelper;
        private readonly Mock<IServiceWrapper> mockServiceWrapper;
        private readonly RecordsController recordController;

        public RecordsControllerTests()
        {
            mockCommentService = new Mock<ICommentService>();
            mockCorpusRecordService = new Mock<ICorpusRecordService>();
            mockWordFrequencyService = new Mock<IWordFrequencyService>();
            mockRecordService = new Mock<IRecordService>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockFileHelper = new Mock<IFileHelper>();
            mockServiceWrapper = new Mock<IServiceWrapper>();
            recordController = new RecordsController(
                mockCommentService.Object,
                mockCorpusRecordService.Object,
                mockWordFrequencyService.Object,
                mockRecordService.Object,
                mockCorpusTypeService.Object,
                mockServiceWrapper.Object,
                mockFileHelper.Object,
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
            var result = await recordController.AddRecordUsingVader(null);
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
                .Setup(m => m.OpenConnectionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockSqlConnection));
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), It.IsAny<DbTransaction>(), It.IsAny<SqlConnection>()))
                .Returns(Task.FromResult(-1));
            //Act            
            var result = await recordController.AddRecordUsingVader(new Mock<RecordViewModel<VaderModel>>().Object);
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
                .Setup(m => m.OpenConnectionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockSqlConnection));
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
            var recordViewModel = Mock.Of<RecordViewModel<VaderModel>>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel<VaderModel>>>();
            var result = await recordController.AddRecordUsingVader(recordViewModel);
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
                .Setup(m => m.OpenConnectionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockSqlConnection));
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
            var recordViewModel = Mock.Of<RecordViewModel<VaderModel>>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel<VaderModel>>>();
            recordViewModel.CorpusRecordViewModels = Mock.Of<IEnumerable<CorpusRecordViewModel>>();
            var result = await recordController.AddRecordUsingVader(recordViewModel);
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
                .Setup(m => m.OpenConnectionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockSqlConnection));
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
            var recordViewModel = Mock.Of<RecordViewModel<VaderModel>>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel<VaderModel>>>();
            recordViewModel.CorpusRecordViewModels = Mock.Of<IEnumerable<CorpusRecordViewModel>>();
            recordViewModel.WordFrequencyViewModels = Mock.Of<IEnumerable<WordFrequencyViewModel>>();
            var result = await recordController.AddRecordUsingVader(recordViewModel);
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
                .Setup(m => m.OpenConnectionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockSqlConnection));
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockSqlConnection))
                .Returns(Task.FromResult(mockDbTransaction));
            mockRecordService
                .Setup(m => m.AddRecordAsync(It.IsAny<RecordModel>(), mockDbTransaction, mockSqlConnection))
                .Returns(Task.FromResult(1));
            mockCommentService
                 .Setup(m => m.SaveCommentsAsync(It.IsAny<IEnumerable<CommentModel>>(), mockDbTransaction, mockSqlConnection))
                 .Returns(Task.FromResult(true));
            mockCorpusRecordService
                .Setup(m => m.AddCorpusRecordAsync(It.IsAny<IEnumerable<CorpusRecordModel>>(), mockDbTransaction, mockSqlConnection))
                .Returns(Task.FromResult(true));
            mockWordFrequencyService
               .Setup(m => m.AddWordFrequenciesAsync(It.IsAny<IEnumerable<WordFrequencyModel>>(), mockDbTransaction, mockSqlConnection))
               .Returns(Task.FromResult(true));

            //Act
            var recordViewModel = Mock.Of<RecordViewModel<VaderModel>>();
            recordViewModel.CommentViewModels = Mock.Of<IEnumerable<CommentViewModel<VaderModel>>>();
            recordViewModel.CorpusRecordViewModels = Mock.Of<IEnumerable<CorpusRecordViewModel>>();
            recordViewModel.WordFrequencyViewModels = Mock.Of<IEnumerable<WordFrequencyViewModel>>();
            var result = await recordController.AddRecordUsingVader(recordViewModel);
            //Assert
            var contentResult = Assert.IsType<OkResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_Ok_When_FetcthRecord_Is_Successfull()
        {
            //Arrange
            var mockConnection = It.IsAny<SqlConnection>();
            var mockTransaction = It.IsAny<DbTransaction>();
            var mockComments = new List<CommentModel>()
            {
                new Mock<CommentModel>().Object,
                new Mock<CommentModel>().Object
            };
            var mockCorpuses = new List<CorpusRecordModel>()
            {
                new Mock<CorpusRecordModel>().Object,
                new Mock<CorpusRecordModel>().Object
            };
            var mockWordFrequencies = new List<WordFrequencyModel>()
            {
                new Mock<WordFrequencyModel>().Object,
                new Mock<WordFrequencyModel>().Object
            };

            mockServiceWrapper
                .Setup(m => m.OpenConnectionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockConnection));
            mockServiceWrapper
                .Setup(m => m.BeginTransactionAsync(mockConnection))
                .Returns(Task.FromResult(mockTransaction));

            mockRecordService
                .Setup(m => m.FindRecordAsync(It.IsAny<int>(), mockTransaction, mockConnection))
                .Returns(Task.FromResult(Mock.Of<RecordModel>()));
            mockCommentService
                .Setup(m => m.FetchCommentsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), mockTransaction, mockConnection))
                .Returns(Task.FromResult((ICollection<CommentModel>)mockComments));
            mockCorpusRecordService
                .Setup(m => m.FetchCorpusRecordAsync(It.IsAny<int>(), mockTransaction, mockConnection))
                .Returns(Task.FromResult((IEnumerable<CorpusRecordModel>)mockCorpuses));
            mockWordFrequencyService
               .Setup(m => m.FetchWordFrequenciesAsync(It.IsAny<int>(), mockTransaction, mockConnection))
               .Returns(Task.FromResult((IEnumerable<WordFrequencyModel>)mockWordFrequencies));
            //Act
            var result = await recordController.FetchRecord(It.IsAny<int>());
            //Assert
            var contentResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(result, contentResult);
        }
        [Fact]
        public async Task Should_Return_BadRequestObjectResult_When_UploadCsv_Fails()
        {
            //Arrange
            mockFileHelper
                .Setup(m => m.UploadCsv(It.IsAny<IFormFile>()))
                .Returns(Task.FromResult(string.Empty));
            //Arrange 
            var result = await recordController.UploadCsv(It.IsAny<IFormFile>(), It.IsAny<AlgorithmnType>());
            //Assert
            var message = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Uploading file!", message.Value);
        }
        [Fact]
        public async Task Should_Return_OkObjectResult_When_PolarizeCsvFile_SentiWorNet_Returns_Values()
        {
            //Arrange
            mockFileHelper
                .Setup(m => m.UploadCsv(It.IsAny<IFormFile>()))
                .Returns(Task.FromResult("Sample String"));
            //Arrange 
            var result = await recordController.UploadCsv(It.IsAny<IFormFile>(), AlgorithmnType.SentiWordNet);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Should_Return_OkObjectResult_When_PolarizeCsvFile_Vader_Returns_Values()
        {
            //Arrange
            mockFileHelper
                .Setup(m => m.UploadCsv(It.IsAny<IFormFile>()))
                .Returns(Task.FromResult("Sample String"));
            //Arrange 
            var result = await recordController.UploadCsv(It.IsAny<IFormFile>(), AlgorithmnType.Vader);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
