
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Reservas.Api.Controllers;
using Reservas.Api.Interfaces;
using Reservas.Api.Models;
using Reservas.Testes.MockData;

namespace Reservas.Testes.Controllers
{
    public class ReservasControllerTeste
    {
        [Fact]
        public void GetTodasReservas_DeveRetornar200Status()
        {

            //Arrange - Organizar
            var reservaService = new Mock<IReservaRepository>();
            reservaService.Setup(i => i.Reservas).Returns(ReservasMockData.GetReservas());
            var sut = new ReservasController(reservaService.Object);
            //act - AGIR
            var result = (OkObjectResult)sut.Get();

            //Assert - afirmar
            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public void GEtReservaPorId_DeveRetornar200_QuandoEncrontrar()
        {
            //Arrange - Organizar
            var reservaMock = ReservasMockData.GetReservas()[0];
            var reservaServices = new Mock<IReservaRepository>();
            reservaServices.Setup(i => i[reservaMock.ReservaId]).Returns(reservaMock);
            var sut = new ReservasController(reservaServices.Object);
            //act - AGIR
            var result = sut.Get(reservaMock.ReservaId);
            //Assert - afirmar
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }
        [Fact]
        public void GetReservaPorId_DeveRetornar404_QuandoNaoEncontrar()
        {
            //Arrange - Organizar
            var reservaService = new Mock<IReservaRepository>();
            reservaService.Setup(i => i[999]).Returns((Reserva)null);
            var sut = new ReservasController(reservaService.Object);
            //act - AGIR
            var result = sut.Get(999);
            //Assert - afirmar
            result.Result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void PostReserva_DeveRetornar201Statuts()
        {
            //Arrange - Organizar
            var novaReserva = new Reserva { Nome = "teste" };
            var reservaService = new Mock<IReservaRepository>();
            reservaService.Setup(i => i.AddReserva(It.IsAny<Reserva>())).Returns(novaReserva);
            var sut = new ReservasController (reservaService.Object);
            //act - AGIR
            var result = sut.Post(novaReserva);
            //Assert - afirmar
            var createResult = result.Result as CreatedAtActionResult;
            createResult.Should().NotBeNull();
            createResult.StatusCode.Should().Be(201);
        }
        [Fact]
        public void PutReserva_DeveRetornar200_QuandoAtualizado()
        {
            //Arrange - Organizar
            var reservaAtualizado = new Reserva { ReservaId = 1, Nome = "Atualizado" };
            var reservaService = new Mock<IReservaRepository> ();
            reservaService.Setup(i => i.UpdateReserva(It.IsAny<Reserva>())).Returns(reservaAtualizado);
            var sut = new ReservasController(reservaService.Object);

            //act - AGIR
            var result = sut.Put(reservaAtualizado);
            //Assert - afirmar

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }
        [Fact]
        public void PatchReserva_DeveRetornar200_QuandoEncontrar()
        {
            //Arrange - Organizar
            var reservaOriginal = new Reserva { ReservaId = 1, Nome = "original" };
            var Patch = new JsonPatchDocument<Reserva>();
            Patch.Replace(r => r.Nome, "Atualizado");

            var reservaService = new Mock<IReservaRepository> ();
            reservaService.Setup(i => i[1]).Returns (reservaOriginal);
            var sut = new ReservasController (reservaService.Object);
            //act - AGIR
            var result =sut.Patch(1, Patch);

            //Assert - afirmar
            result.Should().BeOfType<OkResult>();    
        }


        [Fact]
        public void PatchReserva_DeveRetornar404_QuandoNaoEncontrar()
        {
            //Arrange - Organizar
            var patch = new JsonPatchDocument<Reserva>();
            var reservaService = new Mock<IReservaRepository> ();
            reservaService.Setup(i => i[999]).Returns((Reserva)null);
            var sut = new ReservasController(reservaService.Object);

            //act - AGIR
            var result = sut .Patch(999, patch);
            //Assert - afirmar
            result.Should().BeOfType<NotFoundResult>();
        }





        private void Returns(Reserva reserva)
        {
            throw new NotImplementedException();
        }
    }
}
