using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;
using BibliotecaGrupoExito.Application.Services;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Enums;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Domain.Extensions;

namespace PrestamosBiblioteca.Tests.Unit.Application.Services
{
    [TestClass]
    public class PrestamoServiceTests
    {
        private IMaterialRepository _materialRepository = null!;
        private IUsuarioRepository _usuarioRepository = null!;
        private IPrestamoRepository _prestamoRepository = null!;
        private IPrestamoService _prestamoService = null!;

        [TestInitialize]
        public void Setup()
        {
            // Inicializo los mocks usando NSubstitute
            _materialRepository = Substitute.For<IMaterialRepository>();
            _usuarioRepository = Substitute.For<IUsuarioRepository>();
            _prestamoRepository = Substitute.For<IPrestamoRepository>();

            // Instanci el servicio inyectando los mocks
            _prestamoService = new PrestamoService(_materialRepository, _usuarioRepository, _prestamoRepository);
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoMaterialNoExiste_DebeRetornarError()
        {
            // Arrange

            var request = new PrestamoRequest { ISBN = "12345", IdentificacionUsuario = "user001" };
            _materialRepository.GetByIsbnAsync(request.ISBN).Returns(Task.FromResult<Material?>(null)); // Mock: Material no encontrado

            // Act
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Assert
            Assert.IsFalse(response.Exito);
            Assert.AreEqual("Material no encontrado.", response.Mensaje);
            Assert.AreEqual(request.ISBN, response.ISBN);
            // Verificar que no se intentó guardar ningún préstamo
            await _prestamoRepository.DidNotReceive().AddAsync(Arg.Any<Prestamo>());
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoUsuarioNoExiste_DebeRetornarError()
        {
            // Arrange
            var request = new PrestamoRequest { ISBN = "12345", IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = "12345", Nombre = "Libro Test", TipoMaterial = TipoMaterial.Libro };

            _materialRepository.GetByIsbnAsync(request.ISBN).Returns(Task.FromResult<Material?>(material));// Mock: Material encontrado
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(null)); // Mock: Usuario no encontrado

            // Act
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Assert
            Assert.IsFalse(response.Exito);
            Assert.AreEqual("Usuario no encontrado.", response.Mensaje);
            Assert.AreEqual(request.ISBN, response.ISBN);
            await _prestamoRepository.DidNotReceive().AddAsync(Arg.Any<Prestamo>());
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoISBNEsPalindromo_DebeRetornarError()
        {
            // Arrange
            var isbnPalindromo = "12321L"; 
            var request = new PrestamoRequest { ISBN = isbnPalindromo, IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbnPalindromo, Nombre = "Libro Palíndromo", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user001", Nombre = "Test User" };

            _materialRepository.GetByIsbnAsync(isbnPalindromo).Returns(Task.FromResult<Material?>(material));
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(usuario));
            _materialRepository.IsMaterialAvailableAsync(isbnPalindromo).Returns(Task.FromResult(false));

            // Act
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Assert
            Assert.IsFalse(response.Exito);
            Assert.AreEqual("El material con ISBN en palíndromo solo es para uso en la biblioteca", response.Mensaje);
            Assert.AreEqual(request.ISBN, response.ISBN);
            await _prestamoRepository.DidNotReceive().AddAsync(Arg.Any<Prestamo>());
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoMaterialYaEstaPrestado_DebeRetornarError()
        {
            // Arrange
            var isbn = "12345L";
            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Libro Test", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user001", Nombre = "Test User" };

            _materialRepository.GetByIsbnAsync(isbn).Returns(Task.FromResult<Material?>(material));
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(usuario));
            _materialRepository.IsMaterialAvailableAsync(isbn).Returns(Task.FromResult(true));

            // Act
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Assert
            Assert.IsFalse(response.Exito);
            Assert.AreEqual("El material no puede ser prestado: Ese ISBN ya está prestado", response.Mensaje);
            Assert.AreEqual(request.ISBN, response.ISBN);
            await _prestamoRepository.DidNotReceive().AddAsync(Arg.Any<Prestamo>());
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoRevistaSePrestaFinDeSemana_DebeRetornarError()
        {
            // Arrange
            var isbn = "98765L";
            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Revista Test", TipoMaterial = TipoMaterial.Revista };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user001", Nombre = "Test User" };

            _materialRepository.GetByIsbnAsync(isbn).Returns(Task.FromResult<Material?>(material));
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(usuario));
            _materialRepository.IsMaterialAvailableAsync(isbn).Returns(Task.FromResult(false));

     
            var sabado = new DateTime(2025, 7, 26); 
      
            Assert.Inconclusive("La prueba de revista en fin de semana requiere inyección de tiempo o un test de integración más robusto.");
        }


        [TestMethod]
        public async Task RealizarPrestamo_Libro_SumaDigitosMayorA30_PrestaPor15DiasHabiles()
        {
            // Arrange
            var isbn = "1234567890123L"; // Suma de dígitos: 1+2+3+4+5+6+7+8+9+0+1+2+3 = 51 (mayor a 30)
            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Libro Grande", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user001", Nombre = "Test User" };

            _materialRepository.GetByIsbnAsync(isbn).Returns(Task.FromResult<Material?>(material));
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(usuario));
            _materialRepository.IsMaterialAvailableAsync(isbn).Returns(Task.FromResult(false));

        
            Prestamo capturedPrestamo = null;
            await _prestamoRepository.AddAsync(Arg.Do<Prestamo>(p => capturedPrestamo = p));

            // Act
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Assert
            Assert.IsTrue(response.Exito);
            Assert.AreEqual("Préstamo realizado exitosamente.", response.Mensaje);
            Assert.IsNotNull(response.FechaDevolucionEsperada);
            Assert.AreEqual(request.ISBN, response.ISBN);

            // Verificar que se llamó a AddAsync
            await _prestamoRepository.Received(1).AddAsync(Arg.Any<Prestamo>());
            Assert.IsNotNull(capturedPrestamo);

            // Calcular la fecha esperada manualmente para la aserción
            var fechaPrestamo = DateTime.Today;
            var expectedDevolucion = DateCalculator.CalculaterReturnDate(fechaPrestamo, 15);
            expectedDevolucion = DateCalculator.AdjustIfSunday(expectedDevolucion);

            Assert.AreEqual(expectedDevolucion.Date, response.FechaDevolucionEsperada.Value.Date);
            Assert.AreEqual(expectedDevolucion.Date, capturedPrestamo.FechaDevolucionEsperada.Date);
        }

        [TestMethod]
        public async Task RealizarPrestamo_Libro_SumaDigitosMenorOIgualA30_PrestaPor10DiasHabiles()
        {
            // Arrange
            var isbn = "12345L"; // Suma de dígitos: 1+2+3+4+5 = 15 (menor a 30)
            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Libro Pequeño", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user001", Nombre = "Test User" };

            _materialRepository.GetByIsbnAsync(isbn).Returns(Task.FromResult<Material?>(material));
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(usuario));
            _materialRepository.IsMaterialAvailableAsync(isbn).Returns(Task.FromResult(false));

            Prestamo capturedPrestamo = null;
            await _prestamoRepository.AddAsync(Arg.Do<Prestamo>(p => capturedPrestamo = p));

            // Act
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Assert
            Assert.IsTrue(response.Exito);
            Assert.AreEqual("Préstamo realizado exitosamente.", response.Mensaje);
            Assert.IsNotNull(response.FechaDevolucionEsperada);
            Assert.AreEqual(request.ISBN, response.ISBN);

            await _prestamoRepository.Received(1).AddAsync(Arg.Any<Prestamo>());
            Assert.IsNotNull(capturedPrestamo);

            // Calcular la fecha esperada manualmente
            var fechaPrestamo = DateTime.Today;
            var expectedDevolucion = DateCalculator.CalculaterReturnDate(fechaPrestamo, 10);
            expectedDevolucion = DateCalculator.AdjustIfSunday(expectedDevolucion);

            Assert.AreEqual(expectedDevolucion.Date, response.FechaDevolucionEsperada.Value.Date);
            Assert.AreEqual(expectedDevolucion.Date, capturedPrestamo.FechaDevolucionEsperada.Date);
        }

        [TestMethod]
        public async Task RealizarPrestamo_Revista_PrestaPor2DiasHabilesYFechaCaeEnDomingoSeAjusta()
        {
            // Arrange
            var isbn = "98765L";
            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user001" };
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Revista Semanal", TipoMaterial = TipoMaterial.Revista };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user001", Nombre = "Test User" };

            _materialRepository.GetByIsbnAsync(isbn).Returns(Task.FromResult<Material?>(material));
            _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario).Returns(Task.FromResult<Usuario?>(usuario));
            _materialRepository.IsMaterialAvailableAsync(isbn).Returns(Task.FromResult(false));

            Prestamo capturedPrestamo = null;
            await _prestamoRepository.AddAsync(Arg.Do<Prestamo>(p => capturedPrestamo = p));

         
            // ACT
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // ASSERT
            Assert.IsTrue(response.Exito);
            Assert.AreEqual("Préstamo realizado exitosamente.", response.Mensaje);
            Assert.IsNotNull(response.FechaDevolucionEsperada);
            Assert.AreEqual(request.ISBN, response.ISBN);

            await _prestamoRepository.Received(1).AddAsync(Arg.Any<Prestamo>());
            Assert.IsNotNull(capturedPrestamo);

        
            var fechaPrestamo = DateTime.Today;
            var expectedDevolucion = DateCalculator.CalculaterReturnDate(fechaPrestamo, 2);
            expectedDevolucion = DateCalculator.AdjustIfSunday(expectedDevolucion); 
            Assert.AreEqual(expectedDevolucion.Date, response.FechaDevolucionEsperada.Value.Date);

            Assert.AreEqual(expectedDevolucion.Date, capturedPrestamo.FechaDevolucionEsperada.Date);
            Assert.AreNotEqual(DayOfWeek.Sunday, response.FechaDevolucionEsperada.Value.DayOfWeek);
        }
    }
}