using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
using SentimentAnalysisTool.Api.Helpers;
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
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> mockCommentService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IFileHelper> mockFileHelper;
        private readonly CommentController commentController;
        public CommentControllerTests()
        {
            mockCommentService = new Mock<ICommentService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockFileHelper = new Mock<IFileHelper>();
            commentController = new CommentController(
                mockCommentService.Object,
                mockConfiguration.Object,
                mockFileHelper.Object);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_FetchComments_Returns_None()
        {
            //Arrange
            mockCommentService
                .Setup(m => m.FetchCommentsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<CommentModel>());
            //Act
            var result = await commentController.FetchComments(It.IsAny<int>(), It.IsAny<int>());
            //Assert
            var message = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No Comments Found!", message.Value);
        }
        [Fact]
        public async Task Should_Return_Ok_When_FetchComments_Returns_Comments()
        {
            //Arrange
            var mockComments = new List<CommentModel>()
            {
                Mock.Of<CommentModel>(),
                Mock.Of<CommentModel>()
            };
            mockCommentService
                .Setup(m => m.FetchCommentsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(mockComments);
            //Act
            var result = await commentController.FetchComments(It.IsAny<int>(), It.IsAny<int>());
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_UploadCsv_Is_False()
        {
            //Arrange
            var mockFormFile = new Mock<IFormFile>();
            mockFileHelper
                .Setup(m => m.UploadCsv(mockFormFile.Object))
                .Returns(Task.FromResult(false));
            //Act
            var result = await commentController.SaveComments(mockFormFile.Object);
            //Assert
            var message = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Uploading File!", message.Value);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_PolarizeCsvFile_Returns_Zero()
        {
            //Arrange
            var mockFormFile = new Mock<IFormFile>();
            mockFileHelper
               .Setup(m => m.UploadCsv(mockFormFile.Object))
               .Returns(Task.FromResult(true));
            mockFileHelper
                .Setup(m => m.PolarizeCsvFile(mockFormFile.Object))
                .ReturnsAsync(new List<CommentModel>());
            //Act
            var result = await commentController.SaveComments(mockFormFile.Object);
            //Assert
            var message = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error Polarizing Files!", message.Value);
        }
        [Fact]
        public async Task Should_Return_Ok_When_SaveComments_Is_True()
        {
            //Arrange
            var mockFormFile = new Mock<IFormFile>();
            mockFileHelper
               .Setup(m => m.UploadCsv(mockFormFile.Object))
               .Returns(Task.FromResult(true));
            mockFileHelper
                .Setup(m => m.PolarizeCsvFile(mockFormFile.Object))
                .ReturnsAsync(new List<CommentModel>()
                    {
                        Mock.Of<CommentModel>(),
                        Mock.Of<CommentModel>()
                    });
            //Act
            var result = await commentController.SaveComments(mockFormFile.Object);
            //Assert
            Assert.IsType<OkObjectResult>(result);            
        }
    }
}
