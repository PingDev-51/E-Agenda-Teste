using System.Runtime.CompilerServices;
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

        tarefa.AdicionarItem(new ItemTarefa("Lata de testes"));

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

    [TestMethod]
    public void CadastrarTarefa_ComItens()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Normal
        );

        tarefa.AdicionarItem(new ItemTarefa("Caixa de teste"));

        //Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.HasCount(0, erros);
    }


    [TestMethod]
    public void CadastrarTarefa_ComOsCamposObrigatoriosEmBranco()
    {
        //Arange
        Tarefa tarefa = new Tarefa(
            string.Empty,
            (PrioridadeTarefa)999
        );

        tarefa.DataCriacao = DateTime.MinValue;

        //Act
        List<string> erros = tarefa.Validar();

        //Assert
        Assert.HasCount(3, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros[0]
        );
        Assert.AreEqual(
            "O campo \"Prioridade\" deve ser preenchido.",
            erros[1]
        );
        Assert.AreEqual(
           "O campo \"Data de Criação\" deve ser preenchido.",
            erros[2]
        );
    }


    [TestMethod]
    public void CadastrarTarefa_ComNomeAbaixoDoMinimo()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "T",
            PrioridadeTarefa.Normal
        );

        //Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void CadastrarTarefa_ComNomeNoLimiteMax()
    {
        string nome = new string('K', 100);
        // Arrange
        Tarefa tarefa = new Tarefa(
            nome,
            PrioridadeTarefa.Normal
        );

        //Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void CadastrarTarefa_ComNomeAcimaLimiteMax()
    {
        string nome = new string('K', 101);

        // Arrange
        Tarefa tarefa = new Tarefa(
            nome,
            PrioridadeTarefa.Normal
        );

        //Act
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void CadastrarTarefa_ComPrioridadeForaDaListaPermitida()
    {
        //Arange
        Tarefa tarefa = new Tarefa(
            "teste",
            (PrioridadeTarefa)999
        );

        //Act
        List<string> erros = tarefa.Validar();

        //Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Prioridade\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void AtualizarTarefa_ComDadosValidos()
    {
        //Arange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        Tarefa tarefaAtualizada = new Tarefa("TestarAtualzido", PrioridadeTarefa.Normal);

        // Act
        tarefa.Atualizar(tarefaAtualizada);
        List<string> erros = tarefa.Validar();

        // Assert
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void ConcluirTarefaPendente()
    {
        //Arange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        //Act
        tarefa.AlterarConclusaoManual(true);
        List<string> erros = tarefa.Validar();

        //Assert
        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(DateTime.Today, tarefa.DataCriacao);
        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void TarefaPendente_Concluir()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        // Act
        tarefa.AlterarConclusaoManual(true);

        // Assert
        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataConclusao);
    }
}