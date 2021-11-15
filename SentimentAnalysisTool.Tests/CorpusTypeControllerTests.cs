using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SentimentAnalysisTool.Api.Controllers;
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
    public class CorpusTypeControllerTests
    {
        private readonly Mock<ICorpusTypeService> mockCorpusTypeService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly CorpusTypeController corpusTypeController;
        public CorpusTypeControllerTests()
        {
            mockCorpusTypeService = new Mock<ICorpusTypeService>();
            mockConfiguration = new Mock<IConfiguration>();
            corpusTypeController = new CorpusTypeController(
                mockCorpusTypeService.Object,
                mockConfiguration.Object);
        }
        [Fact]
        public async Task Should_Return_NotFound_When_CorpusTypeViewModel_Is_Null()
        {
            //Arrange
            var result = await corpusTypeController.AddCorpusType(null);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
