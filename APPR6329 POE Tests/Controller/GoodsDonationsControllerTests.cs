using ST10101067_APPR6312_POE_PART_2.Controllers;
using ST10101067_APPR6312_POE_PART_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2.Models;
using FluentAssertions;

namespace APPR6329_POE_Tests.Controller
{
    public class GoodsDonationsControllerTests
    {
        private ApplicationDbContext _context;
        private GoodsDonationsController _goodsDonationsController;

        public GoodsDonationsControllerTests()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: "TestDatabase")
              .Options;

            _context = A.Fake<ApplicationDbContext>(x => x
             .WithArgumentsForConstructor(new object[] { options })
             .CallsBaseMethods());

            // Mock the GoodsInventory DbSet in the ApplicationDbContext
            var goodsInventoryData = new List<GoodsInventory>
            {
                new GoodsInventory { GOODS_INVENTORY_ID = 1, CATGEORY = "Category1", ITEM_COUNT = 10 },
                new GoodsInventory { GOODS_INVENTORY_ID = 2, CATGEORY = "Category2", ITEM_COUNT = 20 },
                // Add more sample items as needed
            };

            var goodsInventoryDbSet = A.Fake<DbSet<GoodsInventory>>(builder => builder
                .Implements(typeof(IQueryable<GoodsInventory>))
                .Implements(typeof(IEnumerable<GoodsInventory>)));

            A.CallTo(() => ((IQueryable<GoodsInventory>)goodsInventoryDbSet).Provider)
                .Returns(goodsInventoryData.AsQueryable().Provider);
            A.CallTo(() => ((IQueryable<GoodsInventory>)goodsInventoryDbSet).Expression)
                .Returns(goodsInventoryData.AsQueryable().Expression);
            A.CallTo(() => ((IEnumerable<GoodsInventory>)goodsInventoryDbSet).GetEnumerator())
                .Returns(goodsInventoryData.GetEnumerator());

            A.CallTo(() => _context.Set<GoodsInventory>()).Returns(goodsInventoryDbSet);

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

            _goodsDonationsController = new GoodsDonationsController(_context, userManager);
        }
        public void Dispose()
        {
            // Clean up resources after each test
            _context.Dispose();
        }
        [Fact]
        public async Task Create_Post_ReturnsViewWithInvalid_GoodsDonationModel()
        {
            // Arrange
            var invalidGoodsDonation = new GoodsDonation
            {
                // Populate fields with invalid data
                GOODS_DONATION_ID = 1, // An ID that might be considered invalid
                USERNAME = "ExistingUser",
                DATE = DateTime.Now.Date.AddDays(-3), // Start date in the past
                ITEM_COUNT = 4, // End date a day before the start date
                CATEGORY = "Medkit",
                DESCRIPTION = "Fast applying Medical kit containing health emergency essentials to aid immediate low-level wounds/pains",
                DONOR = "Anonymous"
            };

            // Act
            var result = await _goodsDonationsController.Create(invalidGoodsDonation);

            // Assert
            result.Should().BeOfType<ViewResult>(); // Expect a ViewResult
            var viewResult = result as ViewResult;
            viewResult.Model.Should().Be(invalidGoodsDonation);
            viewResult.ViewData.ModelState.Keys.Should().Contain("DATE"); // Ensure "DATE" and "DONOR" keys in ModelState
        }
        [Fact]
        public async Task Create_RedirectsToIndexWithValid_GoodsDonationModel()
        {
            // Arrange
            var validGoodsDonation = new GoodsDonation
            {
                // Populate fields with valid data
                GOODS_DONATION_ID = 0,
                USERNAME = "ExistingUser",
                DATE = DateTime.Now.Date,
                ITEM_COUNT = 4,
                CATEGORY = "Medkit",
                DESCRIPTION = "Fast applying Medical kit containing health emergency essentials to aid immediate low-level wounds/pains",
                DONOR = "Anonymous"
            };

            // Act
            var result = await _goodsDonationsController.Create(validGoodsDonation);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>(); // Expect a RedirectToActionResult
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
        }
    }
}
