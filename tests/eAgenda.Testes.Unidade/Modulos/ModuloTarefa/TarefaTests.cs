using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefa;

[TestClass]
public sealed class TarefaTests()
{
    [TestMethod]
    public void CadastrarTarefa_ComTodosOsCamposPreenchidos()
    {
        //Arange
        Tarefa tarefa = new Tarefa("Testar", PrioridadeTarefa.Baixa);

        //Act
        List<string> erros = tarefa.Validar();

        //Assert
        Assert.HasCount(0, erros);
    }


    [TestMethod]
    public void CadastrarTarefa_DeveNascerPendenteComZeroPorcento()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Normal
        );

        // Assert
        Assert.IsFalse(tarefa.Concluida);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataCriacao);
        Assert.IsNull(tarefa.DataConclusao);
    }





}