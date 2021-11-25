using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
        private readonly Mock<IServiceWrapper> mockServiceWrapper;
        private readonly Mock<ICorpusWordsService> mockCorpusWordsService;
        private readonly Mock<ISlangRecordsService> mockSlangRecordsService;
        private readonly CorpusTypeController corpusTypeController;
        public CorpusTypeControllerTests()
        {
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockServiceWrapper = new Mock<IServiceWrapper>();
            mockCorpusWordsService = new Mock<ICorpusWordsService>();
            mockSlangRecordsService = new Mock<ISlangRecordsService>();
            corpusTypeController = new CorpusTypeController(
                mockCorpusTypeService.Object,
                mockConfiguration.Object,
                mockServiceWrapper.Object,
                mockCorpusWordsService.Object,
                mockSlangRecordsService.Object);
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
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockTransaction = It.IsAny<DbTransaction>();
            var mockCorpusTypeModel = new Mock<CorpusTypeModel>().Object;
            mockCorpusTypeService
                .Setup(m => m.AddCorpusTypeAsync(mockCorpusTypeModel, mockSqlConnection, mockTransaction))
                .Returns(Task.FromResult(It.IsAny<int>()));
            //Act
            var result = await corpusTypeController.AddCorpusType(Mock.Of<CorpusTypeViewModel>());
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddCorpusWords_Is_False()
        {
            //Arrange
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockTransaction = It.IsAny<DbTransaction>();
            var mockCorpusTypeModel = new CorpusTypeViewModel()
            {
                CorpusTypeName = It.IsAny<string>(),
                CorpusWordViewModels = new List<CorpusWordViewModel>()
                {
                    new Mock<CorpusWordViewModel>().Object,
                    new Mock<CorpusWordViewModel>().Object
                },
                SlangRecordViewModels = new List<SlangRecordViewModel>()
                {
                    new Mock<SlangRecordViewModel>().Object,
                    new Mock<SlangRecordViewModel>().Object
                }
            };
            mockCorpusTypeService
                 .Setup(m => m.AddCorpusTypeAsync(It.IsAny<CorpusTypeModel>(), mockSqlConnection, mockTransaction))
                 .Returns(Task.FromResult(It.IsAny<int>()));
            mockCorpusWordsService
                .Setup(m => m.AddCorpusWordsAsync(new Mock<CorpusWordModel>().Object, It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await corpusTypeController.AddCorpusType(mockCorpusTypeModel);
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddSlangRecord_Is_False()
        {
            //Arrange
            var mockSqlConnection = It.IsAny<SqlConnection>();
            var mockTransaction = It.IsAny<DbTransaction>();
            var mockCorpusTypeModel = new CorpusTypeViewModel()
            {
                CorpusTypeName = It.IsAny<string>(),
                CorpusWordViewModels = new List<CorpusWordViewModel>()
                {
                    new Mock<CorpusWordViewModel>().Object,
                    new Mock<CorpusWordViewModel>().Object
                },
                SlangRecordViewModels = new List<SlangRecordViewModel>()
                {
                    new Mock<SlangRecordViewModel>().Object,
                    new Mock<SlangRecordViewModel>().Object
                }
            };
            mockCorpusTypeService
                 .Setup(m => m.AddCorpusTypeAsync(It.IsAny<CorpusTypeModel>(), mockSqlConnection, mockTransaction))
                 .Returns(Task.FromResult(It.IsAny<int>()));
            mockCorpusWordsService
                .Setup(m => m.AddCorpusWordsAsync(new Mock<CorpusWordModel>().Object, It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            mockSlangRecordsService
                .Setup(m => m.AddSlangRecordAsync(It.IsAny<SlangRecordModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            //Act
            var result = await corpusTypeController.AddCorpusType(mockCorpusTypeModel);
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
