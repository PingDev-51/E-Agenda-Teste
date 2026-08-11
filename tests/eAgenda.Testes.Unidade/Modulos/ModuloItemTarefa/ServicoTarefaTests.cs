using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloItemTarefa;

[TestClass]
public sealed class ServicoTarefaTests
{
    [TestMethod]
    public void AdicionarItem_DadosValidos_PersisteTarefa()
    {
        // Arrange
        Tarefa tarefa = new(
            "Tarefa teste",
            PrioridadeTarefa.Normal
        );

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new(
            repositorioTarefa.Object
        );

        // Act
        Result resultado = servicoTarefa.AdicionarItem(
            new AdicionarItemTarefaDto(
                tarefa.Id,
                "Item teste"
            )
        );

        // Assert
        Assert.IsTrue(resultado.IsSuccess);

        Assert.HasCount(1, tarefa.Itens);

        Assert.AreEqual(
            "Item teste",
            tarefa.Itens.First().Titulo
        );

        repositorioTarefa.Verify(
            r => r.Editar(tarefa.Id, tarefa),
            Times.Once
        );
    }

    [TestMethod]
    public void AdicionarItem_SemTitulo_RetornaErro()
    {
        // Arrange
        Tarefa tarefa = new(
            "Tarefa teste",
            PrioridadeTarefa.Normal
        );

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new(
            repositorioTarefa.Object
        );

        // Act
        Result resultado = servicoTarefa.AdicionarItem(
            new AdicionarItemTarefaDto(
                tarefa.Id,
                string.Empty
            )
        );

        // Assert
        Assert.IsTrue(resultado.IsFailed);

        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            resultado.Errors.First().Message
        );

        Assert.HasCount(0, tarefa.Itens);

        repositorioTarefa.Verify(
            r => r.Editar(tarefa.Id, tarefa),
            Times.Never
        );
    }
}