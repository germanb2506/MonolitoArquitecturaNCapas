using App.Dto;
using Test.Helpers;

namespace Test.App.Dto
{
    public class ResultTests
    {
        [Fact]
        public void Success_DeberiaCrearResultadoExitoso()
        {
            // Arrange
            var data = TestDataHelper.CreateSampleClienteDto();
            var message = "Cliente creado exitosamente";

            // Act
            var result = Result<ClienteDto>.Success(data, message);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.ResponseCode.Should().Be(ResponseCode.OK);
            result.Message.Should().Be(message);
            result.Data.Should().Be(data);
            result.Errors.Should().BeNull();
        }

        [Fact]
        public void Success_DeberiaUsarMensajePorDefecto()
        {
            // Arrange
            var data = TestDataHelper.CreateSampleClienteDto();

            // Act
            var result = Result<ClienteDto>.Success(data);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.ResponseCode.Should().Be(ResponseCode.OK);
            result.Message.Should().Be("Operación exitosa");
            result.Data.Should().Be(data);
        }

        [Fact]
        public void Success_DeberiaIncluirTraceId()
        {
            // Arrange
            var data = TestDataHelper.CreateSampleClienteDto();
            var traceId = "test-trace-id";

            // Act
            var result = Result<ClienteDto>.Success(data, traceId: traceId);

            // Assert
            result.TraceId.Should().Be(traceId);
        }

        [Fact]
        public void Error_DeberiaCrearResultadoDeError()
        {
            // Arrange
            var message = "Cliente no encontrado";
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var result = Result<ClienteDto>.Error(ResponseCode.NotFound, message, errors);

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.ResponseCode.Should().Be(ResponseCode.NotFound);
            result.Message.Should().Be(message);
            result.Data.Should().BeNull();
            result.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Error_DeberiaPermitirErroresNulos()
        {
            // Arrange
            var message = "Cliente no encontrado";

            // Act
            var result = Result<ClienteDto>.Error(ResponseCode.NotFound, message);

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.ResponseCode.Should().Be(ResponseCode.NotFound);
            result.Message.Should().Be(message);
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public void Error_DeberiaIncluirTraceId()
        {
            // Arrange
            var message = "Cliente no encontrado";
            var traceId = "error-trace-id";

            // Act
            var result = Result<ClienteDto>.Error(ResponseCode.NotFound, message, traceId: traceId);

            // Assert
            result.TraceId.Should().Be(traceId);
        }

        [Theory]
        [InlineData(ResponseCode.OK)]
        [InlineData(ResponseCode.Created)]
        [InlineData(ResponseCode.NoContent)]
        [InlineData(ResponseCode.BadRequest)]
        [InlineData(ResponseCode.Unauthorized)]
        [InlineData(ResponseCode.Forbidden)]
        [InlineData(ResponseCode.NotFound)]
        [InlineData(ResponseCode.InternalServerError)]
        [InlineData(ResponseCode.BadGateway)]
        [InlineData(ResponseCode.ServiceUnavailable)]
        public void Result_DeberiaAceptarTodosLosCodigosDeRespuesta(ResponseCode responseCode)
        {
            // Arrange
            var data = TestDataHelper.CreateSampleClienteDto();

            // Act
            var result = responseCode switch
            {
                ResponseCode.OK => Result<ClienteDto>.Success(data),
                _ => Result<ClienteDto>.Error(responseCode, "Test message")
            };

            // Assert
            result.ResponseCode.Should().Be(responseCode);
        }

        [Fact]
        public void Result_DeberiaFuncionarConTiposGenericos()
        {
            // Arrange
            var stringData = "test string";
            var intData = 42;
            var listData = new List<string> { "item1", "item2" };

            // Act
            var stringResult = Result<string>.Success(stringData);
            var intResult = Result<int>.Success(intData);
            var listResult = Result<List<string>>.Success(listData);

            // Assert
            stringResult.Data.Should().Be(stringData);
            intResult.Data.Should().Be(intData);
            listResult.Data.Should().BeEquivalentTo(listData);
        }

        [Fact]
        public void Result_DeberiaManejarListasVacias()
        {
            // Arrange
            var emptyList = new List<ClienteDto>();

            // Act
            var result = Result<List<ClienteDto>>.Success(emptyList, "Lista vacía");

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().BeEmpty();
            result.Message.Should().Be("Lista vacía");
        }
    }
} 