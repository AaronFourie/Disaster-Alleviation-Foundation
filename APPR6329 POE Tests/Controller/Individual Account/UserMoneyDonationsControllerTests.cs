using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2.Controllers;
using ST10101067_APPR6312_POE_PART_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ST10101067_APPR6312_POE_PART_2.Models;
using FluentAssertions;

namespace APPR6329_POE_Tests.Controller.Individual_Account
{
    public class UserMoneyDonationsControllerTests: IDisposable
    {
        private ApplicationDbContext _context;
        private UserMoneyDonationsController _userMoneyDonationsController;
        public UserMoneyDonationsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = A.Fake<ApplicationDbContext>(x => x
            .WithArgumentsForConstructor(new object[] { options })
            .CallsBaseMethods());

            // Mock the GoodsInventory DbSet in the ApplicationDbContext
            var moneyData = new List<Money>
            {
                new Money { MoneyId = 1, RemainingMoney = 0, TotalMoney = 0 },
                // Add more sample items as needed
            };

            var moneyDbSet = A.Fake<DbSet<Money>>(builder => builder
                .Implements(typeof(IQueryable<Money>))
                .Implements(typeof(IEnumerable<Money>)));

            A.CallTo(() => ((IQueryable<Money>)moneyDbSet).Provider)
                .Returns(moneyData.AsQueryable().Provider);
            A.CallTo(() => ((IQueryable<Money>)moneyDbSet).Expression)
                .Returns(moneyData.AsQueryable().Expression);
            A.CallTo(() => ((IEnumerable<Money>)moneyDbSet).GetEnumerator())
                .Returns(moneyData.GetEnumerator());

            A.CallTo(() => _context.Set<Money>()).Returns(moneyDbSet);

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

            _userMoneyDonationsController = new UserMoneyDonationsController(_context, userManager);
        }
        public void Dispose()
        {
            // Clean up resources after each test
            _context.Dispose();
        }
        [Fact]
        public async Task Create_Post_ReturnsViewWithInvalid_MoneyDonationModel()
        {
            // Arrange
            var invalidMoneyDonation = new MoneyDonation
            {
                // Populate fields with invalid data
                MONEY_DONATION_ID = 1, // An ID that might be considered invalid
                USERNAME = "ExistingUser",
                DATE = DateTime.Now.Date.AddDays(-3), // Start date in the past
                AMOUNT = 0, // End date a day before the start date
                DONOR = "Anonymous"
            };

            // Act
            var result = await _userMoneyDonationsController.Create(invalidMoneyDonation);
            // Retrieve the existing Money entity or create a new one
            // Assert
            result.Should().BeOfType<ViewResult>(); // Expect a ViewResult
            var viewResult = result as ViewResult;
            viewResult.Model.Should().Be(invalidMoneyDonation);
            viewResult.ViewData.ModelState.Keys.Should().Contain("DATE", "AMOUNT"); // Ensure "DATE" and "DONOR" keys in ModelState
        }
        [Fact]
        public async Task Create_RedirectsToIndexWithValid_MoneyDonationModel()
        {
            // Arrange
            var validMoneyDonation = new MoneyDonation
            {
                // Populate fields with valid data
                // Populate fields with invalid data
                MONEY_DONATION_ID = 0,
                USERNAME = "ExistingUser",
                DATE = DateTime.Now.Date, // Start date in the past
                AMOUNT = 400, // End date a day before the start date
                DONOR = "Anonymous"
            };

            // Act
            var result = await _userMoneyDonationsController.Create(validMoneyDonation);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>(); // Expect a RedirectToActionResult
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
        }
    }
}
