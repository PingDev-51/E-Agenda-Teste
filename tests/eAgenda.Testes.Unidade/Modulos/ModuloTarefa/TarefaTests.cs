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

    // cadastro de outros tipos de tarefas


}