using Xunit;
using ContaBancaria;

namespace ContaBancaria.Tests;

/// <summary>
/// Testes unitários para a classe Conta.
/// 
/// PARTE 1 — Testes de exemplo (Construtor) já estão prontos.
///           Observe o padrão AAA e o uso de [Fact] e [Theory].
///
/// PARTE 2 — Você deve escrever os testes para os demais métodos
///           seguindo rigorosamente o ciclo TDD: Red → Green → Refactor.
///
/// Para cada método da classe Conta, crie testes que cubram:
///   ✅ O cenário de sucesso (caminho feliz)
///   ❌ Cada regra de validação (cenários de exceção)
///   🔄 Casos de borda (valores limites)
/// </summary>
public class ContaTests
{
    // =======================================================
    //  PARTE 1 — EXEMPLO GUIADO: Testes do Construtor
    //  Observe o padrão Arrange-Act-Assert (AAA)
    // =======================================================

    [Fact]
    public void Construtor_DadosValidos_CriaContaCorretamente()
    {
        // Arrange & Act
        var conta = new Conta("Maria", 100);

        // Assert
        Assert.Equal("Maria", conta.Titular);
        Assert.Equal(100, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    [Fact]
    public void Construtor_SemSaldoInicial_CriaContaComSaldoZero()
    {
        // Arrange & Act
        var conta = new Conta("João");

        // Assert
        Assert.Equal("João", conta.Titular);
        Assert.Equal(0, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    [Fact]
    public void Construtor_TitularNulo_LancaArgumentException()
    {
        // Assert — verifica que a exceção é lançada
        Assert.Throws<ArgumentException>(() => new Conta(null!));
    }

    [Fact]
    public void Construtor_TitularVazio_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta(""));
    }

    [Fact]
    public void Construtor_SaldoNegativo_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta("Maria", -50));
    }

    [Theory]
    [InlineData("Ana", 0)]
    [InlineData("Carlos", 1000)]
    [InlineData("Beatriz", 0.01)]
    public void Construtor_VariosValoresValidos_CriaContaCorretamente(string titular, decimal saldo)
    {
        // Act
        var conta = new Conta(titular, saldo);

        // Assert
        Assert.Equal(titular, conta.Titular);
        Assert.Equal(saldo, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    // =======================================================
    //  PARTE 2 — ESCREVA OS TESTES ABAIXO (TDD)
    //  Lembre-se: escreva o teste PRIMEIRO, veja FALHAR (Red),
    //  depois implemente o código para PASSAR (Green),
    //  e por fim faça Refactor se necessário.
    // =======================================================

//Testes: Depositar
[Fact]
public void Depositar_ValorValido_AtualizaSaldo()
{
    // Arrange
    var conta = new Conta("Maria", 100);
    var valorDeposito = 50m;
    var saldoEsperado = 150m;

    // Act
    conta.Depositar(valorDeposito);

    // Assert
    Assert.Equal(saldoEsperado, conta.Saldo);
}

[Theory]
[InlineData(0)]
[InlineData(-1)]
[InlineData(-100)]
public void Depositar_ValorInvalido_LancaArgumentException(decimal valorInvalido)
{
    // Arrange
    var conta = new Conta("Maria", 100);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => conta.Depositar(valorInvalido));
}

[Fact]
public void Depositar_ContaInativa_LancaInvalidOperationException()
{
    // Arrange
    var conta = new Conta("Maria", 0);
    conta.Encerrar(); 

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => conta.Depositar(50));
}

[Fact]
public void Depositar_ValorValido_AtualizaSaldoCorretamente()
{
    // Arrange
    var conta = new Conta("Maria", 100);
    var valorDeposito = 50m;
    var saldoEsperado = 150m;

    // Act
    conta.Depositar(valorDeposito);

    // Assert
    Assert.Equal(saldoEsperado, conta.Saldo);
}

//Testes: Sacar
[Fact]
public void Sacar_ValorValido_AtualizaSaldoCorretamente()
{
    // Arrange
    var conta = new Conta("Maria", 100);
    var valorSaque = 40m;
    var saldoEsperado = 60m;

    // Act
    conta.Sacar(valorSaque);

    // Assert
    Assert.Equal(saldoEsperado, conta.Saldo);
}

[Fact]
public void Sacar_ValorMaiorQueSaldo_LancaInvalidOperationException()
{
    // Arrange
    var conta = new Conta("Maria", 100);
    var valorSaque = 150m;

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => conta.Sacar(valorSaque));
}

[Theory]
[InlineData(0)]
[InlineData(-10)]
public void Sacar_ValorInvalido_LancaArgumentException(decimal valorInvalido)
{
    // Arrange
    var conta = new Conta("Maria", 100);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => conta.Sacar(valorInvalido));
}

[Fact]
public void Sacar_ContaInativa_LancaInvalidOperationException()
{
    // Arrange
    var conta = new Conta("Maria", 0);
    conta.Encerrar(); 

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => conta.Sacar(50));
}

//Testes: Transferir
[Fact]
public void Transferir_ValorValido_AtualizaSaldoDeAmbasAsContas()
{
    // Arrange
    var contaOrigem = new Conta("Maria", 500);
    var contaDestino = new Conta("João", 100);
    var valorTransferencia = 200m;
    var saldoEsperadoOrigem = 300m;
    var saldoEsperadoDestino = 300m;

    // Act
    contaOrigem.Transferir(contaDestino, valorTransferencia);

    // Assert
    Assert.Equal(saldoEsperadoOrigem, contaOrigem.Saldo);
    Assert.Equal(saldoEsperadoDestino, contaDestino.Saldo);
}

[Fact]
public void Transferir_SaldoInsuficiente_LancaInvalidOperationException()
{
    // Arrange
    var contaOrigem = new Conta("Maria", 100);
    var contaDestino = new Conta("João", 100);
    var valorTransferencia = 150m;

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => contaOrigem.Transferir(contaDestino, valorTransferencia));
}

[Theory]
[InlineData(0)]
[InlineData(-50)]
public void Transferir_ValorInvalido_LancaArgumentException(decimal valorInvalido)
{
    // Arrange
    var contaOrigem = new Conta("Maria", 500);
    var contaDestino = new Conta("João", 100);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => contaOrigem.Transferir(contaDestino, valorInvalido));
}

[Fact]
public void Transferir_ContaOrigemInativa_LancaInvalidOperationException()
{
    // Arrange
    var contaOrigem = new Conta("Maria", 0);
    var contaDestino = new Conta("João", 100);
    contaOrigem.Encerrar();

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => contaOrigem.Transferir(contaDestino, 100));
}

[Fact]
public void Transferir_ContaDestinoInativa_LancaInvalidOperationException()
{
    // Arrange
    var contaOrigem = new Conta("Maria", 500);
    var contaDestino = new Conta("João", 0);
    contaDestino.Encerrar(); 

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => contaOrigem.Transferir(contaDestino, 100));
}

//Testes: Encerrar
[Fact]
public void Encerrar_ContaComSaldoZero_AlteraAtivaParaFalso()
{
    // Arrange
    var conta = new Conta("Maria", 0);

    // Act
    conta.Encerrar();

    // Assert
    Assert.False(conta.Ativa);
}

[Fact]
public void Encerrar_ContaComSaldoPositivo_LancaInvalidOperationException()
{
    // Arrange
    var conta = new Conta("Maria", 100);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => conta.Encerrar());
}

[Fact]
public void Encerrar_ContaJaInativa_LancaInvalidOperationException()
{
    // Arrange
    var conta = new Conta("Maria", 0);
    conta.Encerrar();

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => conta.Encerrar());
}
} 