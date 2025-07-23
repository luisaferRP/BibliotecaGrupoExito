using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using BibliotecaGrupoExito.Tests.Integration.Configuration;
using FluentAssertions; 
using System.Net;
using System;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Enums;
using BibliotecaGrupoExito.Infrastructure.Data;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BibliotecaGrupoExito.Tests.Integration.Api.Controllers
{
    [TestClass]
    public class PrestamosControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> _factory; 
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;

        public PrestamosControllerTests()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();

            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
            _dbContext.Dispose();
        }

        private async Task SeedDataAsync(Material material = null, Usuario usuario = null, Prestamo prestamo = null)
        {
            if (material != null)
            {
                _dbContext.Materiales.Add(material);
            }
            if (usuario != null)
            {
                _dbContext.Usuarios.Add(usuario);
            }
            if (prestamo != null)
            {
                _dbContext.Prestamos.Add(prestamo);
            }
            await _dbContext.SaveChangesAsync();
        }


        [TestMethod]
        public async Task RealizarPrestamo_CuandoISBNEsPalindromo_DebeRetornarBadRequest()
        {
            // Arrange
            var isbnPalindromo = "121L"; 
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbnPalindromo, Nombre = "Libro de Palíndromos", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user123", Nombre = "Juan Perez" };

            await SeedDataAsync(material, usuario);

            var request = new PrestamoRequest { ISBN = isbnPalindromo, IdentificacionUsuario = "user123" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/prestamos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
            var responseString = await response.Content.ReadAsStringAsync();
            var prestamoResponse = JsonConvert.DeserializeObject<PrestamoResponse>(responseString);

            prestamoResponse.Should().NotBeNull();
            prestamoResponse.Exito.Should().BeFalse();
            prestamoResponse.Mensaje.Should().Be("El material con ISBN en palíndromo solo es para uso en la biblioteca");
            prestamoResponse.ISBN.Should().Be(isbnPalindromo);

       
            _dbContext.Prestamos.Should().BeEmpty();
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoMaterialYaEstaPrestado_DebeRetornarBadRequest()
        {
            // Arrange
            var isbn = "12345L";
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Libro Ocupado", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user123", Nombre = "Juan Perez" };
            var prestamoActivo = new Prestamo
            {
                Id = Guid.NewGuid(),
                MaterialId = material.Id,
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Today.AddDays(-5),
                FechaDevolucionEsperada = DateTime.Today.AddDays(10),
                Activo = true
            };

            await SeedDataAsync(material, usuario, prestamoActivo);

            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user123" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/prestamos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            var prestamoResponse = JsonConvert.DeserializeObject<PrestamoResponse>(responseString);

            prestamoResponse.Should().NotBeNull();
            prestamoResponse.Exito.Should().BeFalse();
            prestamoResponse.Mensaje.Should().Be("El material no puede ser prestado: Ese ISBN ya está prestado");
            prestamoResponse.ISBN.Should().Be(isbn);

       
            _dbContext.Prestamos.Count().Should().Be(1);
            _dbContext.Prestamos.First().Activo.Should().BeTrue();
        }

        [TestMethod]
        public async Task RealizarPrestamo_CuandoRevistaSePrestaFinDeSemana_DebeRetornarBadRequest()
        {
            // Arrange
            var isbn = "98765L";
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Revista Vieja", TipoMaterial = TipoMaterial.Revista };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user123", Nombre = "Juan Perez" };

            await SeedDataAsync(material, usuario);

            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user123" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");


            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                // Act
                var response = await _client.PostAsync("/api/prestamos", content);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                var responseString = await response.Content.ReadAsStringAsync();
                var prestamoResponse = JsonConvert.DeserializeObject<PrestamoResponse>(responseString);

                prestamoResponse.Should().NotBeNull();
                prestamoResponse.Exito.Should().BeFalse();
                prestamoResponse.Mensaje.Should().Be("La revista no puede ser prestada para el fin de semana");
                prestamoResponse.ISBN.Should().Be(isbn);

                _dbContext.Prestamos.Should().BeEmpty();
            }
            else
            {
                // Si la prueba se ejecuta en un día de semana, esta condición no se cumplirá
                // y el préstamo se realizaría exitosamente (suponiendo que las otras reglas pasan).
                Assert.Inconclusive("Esta prueba para revista en fin de semana solo es relevante si se ejecuta un sábado o domingo, o si el tiempo es mockeado.");
            }
        }


        [TestMethod]
        public async Task RealizarPrestamo_LibroValido_SumaDigitosMayorA30_DebeRetornarOkCon15DiasHabiles()
        {
            // Arrange
            var isbn = "1234567890123L";
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Libro Epico", TipoMaterial = TipoMaterial.Libro };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user456", Nombre = "Maria Lopez" };

            await SeedDataAsync(material, usuario);

            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user456" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/prestamos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            var prestamoResponse = JsonConvert.DeserializeObject<PrestamoResponse>(responseString);

            prestamoResponse.Should().NotBeNull();
            prestamoResponse.Exito.Should().BeTrue();
            prestamoResponse.Mensaje.Should().Be("Préstamo realizado exitosamente.");
            prestamoResponse.ISBN.Should().Be(isbn);
            prestamoResponse.FechaDevolucionEsperada.Should().HaveValue();

            // Calcular fecha esperada manualmente usando la lógica del servicio para verificar
            var expectedDate = BibliotecaGrupoExito.Domain.Extensions.DateCalculator.CalculaterReturnDate(DateTime.Today, 15);
            expectedDate = BibliotecaGrupoExito.Domain.Extensions.DateCalculator.AdjustIfSunday(expectedDate);

            prestamoResponse.FechaDevolucionEsperada.Value.Date.Should().Be(expectedDate.Date);

            // Verificar que el préstamo se guardó en la base de datos
            var savedPrestamo = _dbContext.Prestamos.Should().ContainSingle().Subject;
            savedPrestamo.MaterialId.Should().Be(material.Id);
            savedPrestamo.UsuarioId.Should().Be(usuario.Id);
            savedPrestamo.FechaPrestamo.Date.Should().Be(DateTime.Today.Date);
            savedPrestamo.FechaDevolucionEsperada.Date.Should().Be(expectedDate.Date);
            savedPrestamo.Activo.Should().BeTrue();
        }

        [TestMethod]
        public async Task RealizarPrestamo_RevistaValida_DebeRetornarOkCon2DiasHabiles()
        {
            // Arrange
            var isbn = "112233L";
            var material = new Material { Id = Guid.NewGuid(), ISBN = isbn, Nombre = "Revista Cientifica", TipoMaterial = TipoMaterial.Revista };
            var usuario = new Usuario { Id = Guid.NewGuid(), Identificacion = "user789", Nombre = "Carlos Garcia" };

            await SeedDataAsync(material, usuario);

            var request = new PrestamoRequest { ISBN = isbn, IdentificacionUsuario = "user789" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/prestamos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            var prestamoResponse = JsonConvert.DeserializeObject<PrestamoResponse>(responseString);

            prestamoResponse.Should().NotBeNull();
            prestamoResponse.Exito.Should().BeTrue();
            prestamoResponse.Mensaje.Should().Be("Préstamo realizado exitosamente.");
            prestamoResponse.ISBN.Should().Be(isbn);
            prestamoResponse.FechaDevolucionEsperada.Should().HaveValue();

            // Calcular fecha esperada manualmente
            var expectedDate = BibliotecaGrupoExito.Domain.Extensions.DateCalculator.CalculaterReturnDate(DateTime.Today, 2);
            expectedDate = BibliotecaGrupoExito.Domain.Extensions.DateCalculator.AdjustIfSunday(expectedDate);

            prestamoResponse.FechaDevolucionEsperada.Value.Date.Should().Be(expectedDate.Date);

            var savedPrestamo = _dbContext.Prestamos.Should().ContainSingle().Subject;
            savedPrestamo.MaterialId.Should().Be(material.Id);
            savedPrestamo.UsuarioId.Should().Be(usuario.Id);
            savedPrestamo.FechaPrestamo.Date.Should().Be(DateTime.Today.Date);
            savedPrestamo.FechaDevolucionEsperada.Date.Should().Be(expectedDate.Date);
            savedPrestamo.Activo.Should().BeTrue();
        }
    }
}