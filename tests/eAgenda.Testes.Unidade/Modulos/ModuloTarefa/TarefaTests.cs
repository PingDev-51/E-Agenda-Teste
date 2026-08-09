using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefa;

[TestClass]
public sealed class TarefaTests()
{


    [TestMethod]
    public void CadastrarTarefa_ComTodosOsCamposPreenchidos()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        // Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.HasCount(0, erros);
        Assert.AreEqual("Testar", tarefa.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefa.Prioridade);
        Assert.AreEqual(DateTime.Today, tarefa.DataCriacao);
        Assert.IsNull(tarefa.DataConclusao);
        Assert.IsFalse(tarefa.Concluida);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
    }


    [TestMethod]
    public void CadastrarTarefa_DeveNascerPendenteComZeroPorcento()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Test",
            PrioridadeTarefa.Normal
        );

        // Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.IsFalse(tarefa.Concluida);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataCriacao);
        Assert.IsNull(tarefa.DataConclusao);
        Assert.HasCount(0, erros);
    }


    [TestMethod]
    public void CadastrarTarefa_SemItens()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Normal
        );

        //Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.HasCount(0, erros);
    }

}