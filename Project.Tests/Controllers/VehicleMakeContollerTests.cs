using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Project.MVC_.Controllers;
using Project.Service_.Interfaces;
using Project.Service_.DTOs;
using Project.MVC_.ViewModels;
using AutoMapper;
using System.Web.Mvc;

namespace Project.Tests.Controllers
{
    [TestClass]
    public class VehicleMakeControllerTests
    {
        [TestMethod]
        public async Task Details_ReturnsBadRequest_WhenIdIsNull()
        {
            // Arrange
            var mockService = new Mock<IVehicleMakeService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new VehicleMakeController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            var status = result as HttpStatusCodeResult;
            Assert.AreEqual(400, status.StatusCode); // BadRequest
        }

        [TestMethod]
        public async Task Details_ReturnsHttpNotFound_WhenMakeIsNull()
        {
            // Arrange
            var mockService = new Mock<IVehicleMakeService>();
            var mockMapper = new Mock<IMapper>();
            mockService.Setup(s => s.GetByIdAsync(42)).ReturnsAsync((VehicleMakeDto)null);

            var controller = new VehicleMakeController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.Details(42);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WhenMakeExists()
        {
            // Arrange
            var dto = new VehicleMakeDto { Id = 1, Name = "BMW", Abrv = "BMW" };
            var viewModel = new VehicleMakeViewModel { Id = 1, Name = "BMW", Abrv = "BMW" };

            var mockService = new Mock<IVehicleMakeService>();
            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<VehicleMakeViewModel>(dto)).Returns(viewModel);

            var controller = new VehicleMakeController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(VehicleMakeViewModel));
            var model = viewResult.Model as VehicleMakeViewModel;
            Assert.AreEqual("BMW", model.Name);
        }
    }
}