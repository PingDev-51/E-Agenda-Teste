using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using FluentAssertions;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefa;

[TestClass]
public sealed class ServicoTarefaTests
{
    [TestMethod]
    public void Cadastrar_DadosValidos_PersisteTarefa()
    {
        //Arange
        Tarefa tarefa = new("testar", PrioridadeTarefa.Baixa);

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([]);

        Tarefa? tarefaCadastrada = null;

        repositorioTarefa.Setup(r => r.Cadastrar(It.IsAny<Tarefa>())).Callback<Tarefa>(materia => tarefaCadastrada = materia);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        //Act
        Result resultado = servicoTarefa
            .Cadastrar(new CadastrarTarefaDto("testar", PrioridadeTarefa.Baixa));


        //Assert
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(servicoTarefa);

        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_SemItens_PersisteTarefa()
    {
        //Arange
        Tarefa tarefa = new("testar", PrioridadeTarefa.Baixa);

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([]);

        Tarefa? tarefaCadastrada = null;

        repositorioTarefa.Setup(r => r.Cadastrar(It.IsAny<Tarefa>())).Callback<Tarefa>(materia => tarefaCadastrada = materia);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        //Act
        Result resultado = servicoTarefa
            .Cadastrar(new CadastrarTarefaDto("testar", PrioridadeTarefa.Baixa));


        //Assert
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(servicoTarefa);

        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
    }


    [TestMethod]
    public void Cadastrar_ComItens_PersisteTarefa()
    {
        //Arange
        Tarefa tarefa = new("testar", PrioridadeTarefa.Baixa);

        tarefa.AdicionarItem(new ItemTarefa("Lata de teste"));

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([]);

        Tarefa? tarefaCadastrada = null;

        repositorioTarefa.Setup(r => r.Cadastrar(It.IsAny<Tarefa>())).Callback<Tarefa>(materia => tarefaCadastrada = materia);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        //Act
        Result resultado = servicoTarefa
            .Cadastrar(new CadastrarTarefaDto("testar", PrioridadeTarefa.Baixa));


        //Assert
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(servicoTarefa);

        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
    }


    [TestMethod]
    public void Cadastrar_ComCamposInvalidos_RetornaErro()
    {
        //Arange
        Tarefa tarefa = new(string.Empty, (PrioridadeTarefa)999);

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([]);

        Tarefa? tarefaCadastrada = null;

        repositorioTarefa.Setup(r => r.Cadastrar(It.IsAny<Tarefa>())).Callback<Tarefa>(materia => tarefaCadastrada = materia);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        //Act
        Result resultado = servicoTarefa
            .Cadastrar(new CadastrarTarefaDto(string.Empty, (PrioridadeTarefa)999));

        List<string> erros = tarefa.Validar();

        //Assert
        Assert.IsTrue(resultado.IsFailed);
        Assert.HasCount(2, erros);
        Assert.AreEqual(
          "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
          erros[0]
        );
        Assert.AreEqual(
          "O campo \"Prioridade\" deve ser preenchido.",
          erros[1]
        );

        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Never);
    }


    [TestMethod]
    public void Cadastrar_ComPrioridadeAcimaDoLimite_RetornaErro()
    {
        //Arange
        Tarefa tarefa = new("Teste", (PrioridadeTarefa)999);

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([]);

        Tarefa? tarefaCadastrada = null;

        repositorioTarefa.Setup(r => r.Cadastrar(It.IsAny<Tarefa>())).Callback<Tarefa>(materia => tarefaCadastrada = materia);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        //Act
        Result resultado = servicoTarefa
            .Cadastrar(new CadastrarTarefaDto("Teste", (PrioridadeTarefa)999));

        List<string> erros = tarefa.Validar();

        //Assert
        Assert.IsTrue(resultado.IsFailed);
        Assert.HasCount(1, erros);
        Assert.AreEqual(
          "O campo \"Prioridade\" deve ser preenchido.",
          erros.First()
        );

        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void ConcluirTarefaPendente_AtualizaConclusao()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Act
        Result resultado = servicoTarefa.AlterarConclusao(
            new AlterarConclusaoTarefaDto(tarefa.Id, true)
        );

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataConclusao);

        repositorioTarefa.Verify(
            r => r.Editar(tarefa.Id, tarefa),
            Times.Once
        );
    }

    [TestMethod]
    public void ConcluirTarefa_AtualizaConclusao()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Act
        Result resultado = servicoTarefa.AlterarConclusao(
            new AlterarConclusaoTarefaDto(tarefa.Id, true)
        );

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataConclusao);

        repositorioTarefa.Verify(
            r => r.Editar(tarefa.Id, tarefa),
            Times.Once
        );
    }


    [TestMethod]
    public void ReabrirTarefaConcluida_AtualizaConclusao()
    {
        // Arrange
        Tarefa tarefa = new Tarefa(
            "Testar",
            PrioridadeTarefa.Alta
        );

        tarefa.AlterarConclusaoManual(true);

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Act
        Result resultado = servicoTarefa.AlterarConclusao(
            new AlterarConclusaoTarefaDto(tarefa.Id, false)
        );

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsFalse(tarefa.Concluida);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.IsNull(tarefa.DataConclusao);

        repositorioTarefa.Verify(
            r => r.Editar(tarefa.Id, tarefa),
            Times.Once
        );
    }


    [TestMethod]
    public void ExcluirTarefa_ComItensVinculados_RemoveTarefa()
    {
        // Arrange
        Tarefa tarefa = new(
            "Testar",
            PrioridadeTarefa.Normal
        );

        tarefa.AdicionarItem(new ItemTarefa("Item 1"));
        tarefa.AdicionarItem(new ItemTarefa("Item 2"));

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new(
            repositorioTarefa.Object
        );

        // Act
        Result resultado = servicoTarefa.Excluir(tarefa.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);

        repositorioTarefa.Verify(
            r => r.Excluir(tarefa.Id),
            Times.Once
        );
    }
}