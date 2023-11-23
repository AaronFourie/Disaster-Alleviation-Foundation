using ST10101067_APPR6312_POE_PART_2.Controllers;
using ST10101067_APPR6312_POE_PART_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ST10101067_APPR6312_POE_PART_2.Models;
using FluentAssertions;

namespace APPR6329_POE_Tests.Controller
{
    public class DisastersControllerTests
    {
        private ApplicationDbContext _context;
        private DisastersController _disastersController;

        public DisastersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;

            _context = A.Fake<ApplicationDbContext>(x => x.WithArgumentsForConstructor(new object[] { options }));

            var existingUser = new IdentityUser
            {
                Id = "userId",
                UserName = "ExistingUser",
                Email = "existinguser@example.com"
                // Add other necessary properties
            };


            var userManager = A.Fake<UserManager<IdentityUser>>();

            A.CallTo(() => userManager.FindByIdAsync(A<string>._))
                .WithAnyArguments()
                .Returns(existingUser);

            _disastersController = new DisastersController(_context, userManager);
        }
        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithDisaster()
        {
            // Arrange
            var invalidDisaster = new Disaster
            {
                // Populate fields with invalid data
                DISTATER_ID = 1, // An ID that might be considered invalid
                USERNAME = null,
                LOCATION = null,
                STARTDATE = DateTime.Now.Date.AddDays(-3), // Start date in the past
                ENDDATE = DateTime.Now.Date.AddDays(-4), // End date a day before the start date
                AID_TYPE = "Invalid Type",
                IsActive = 1 // Validating IsActive as true (1) or false (0)
            };

            // Act
            var result = await _disastersController.Create(invalidDisaster);

            // Assert
            result.Should().BeOfType<ViewResult>(); // Expect a ViewResult
            var viewResult = result as ViewResult;
            viewResult.Model.Should().Be(invalidDisaster);
            viewResult.ViewData.ModelState.Keys.Should().Contain("STARTDATE", "ENDDATE"); // Ensure "STARTDATE" key in ModelState
        }

        [Fact]
        public async Task Create_RedirectsToIndexWithValidModel()
        {
            // Arrange
            var validDisaster = new Disaster
            {
                // Populate fields with valid data
                DISTATER_ID = 0, // For instance, assuming a new entry with ID as 0
                USERNAME = "ValidUsername", // Valid value for USERNAME
                STARTDATE = DateTime.Now.Date, // Start date in the past
                ENDDATE = DateTime.Now.Date.AddDays(1), // End date one day after the start date
                LOCATION = "Valid Location", // Valid value for LOCATION
                AID_TYPE = "Valid Type", // Valid value for AID_TYPE field
                IsActive = 1 // Validating IsActive as true (1) or false (0)
                             // Add other fields included in the Bind attribute with valid data
            };

            // Act
            var result = await _disastersController.Create(validDisaster);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>(); // Expect a RedirectToActionResult
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
        }
    }
}
