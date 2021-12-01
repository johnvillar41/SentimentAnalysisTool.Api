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
    public class AbbreviationsControllerTests
    {
        private readonly Mock<IAbbreviationsService> mockAbbreviationsService;
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly AbbreviationsController abbreviationsController;
        public AbbreviationsControllerTests()
        {
            mockAbbreviationsService = new Mock<IAbbreviationsService>();
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            abbreviationsController = new AbbreviationsController(
                mockAbbreviationsService.Object,
                mockCorpusTypeService.Object,
                mockConfiguration.Object);
        }
        [Fact]
        public async Task Should_Return_Ok_When_AddAbbreviation_Returns_True()
        {
            //Arrange
            var mockAbbreviations = new List<AbbreviationsViewModel>()
            {
                Mock.Of<AbbreviationsViewModel>(),
                Mock.Of<AbbreviationsViewModel>()
            };
            mockCorpusTypeService
                .Setup(m => m.FindCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(Mock.Of<CorpusTypeModel>()));
            mockAbbreviationsService
                .Setup(m => m.AddAbbreviationAsync(It.IsAny<int>(), It.IsAny<IEnumerable<AbbreviationModel>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = await abbreviationsController.AddAbbreviations(It.IsAny<int>(), mockAbbreviations);
            //Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Should_Return_BadRequest_When_AddAbbreviation_Returns_False()
        {
            //Arrange
            var mockAbbreviations = new List<AbbreviationsViewModel>()
            {
                Mock.Of<AbbreviationsViewModel>(),
                Mock.Of<AbbreviationsViewModel>()
            };
            mockCorpusTypeService
                .Setup(m => m.FindCorpusTypeAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(Mock.Of<CorpusTypeModel>()));
            mockAbbreviationsService
                .Setup(m => m.AddAbbreviationAsync(It.IsAny<int>(), It.IsAny<IEnumerable<AbbreviationModel>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = await abbreviationsController.AddAbbreviations(It.IsAny<int>(), mockAbbreviations);
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
