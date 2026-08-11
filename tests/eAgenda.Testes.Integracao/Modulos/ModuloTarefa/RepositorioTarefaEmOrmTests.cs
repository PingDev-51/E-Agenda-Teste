using eAgenda.Dominio.Modulos.ModuloTarefa;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgenda.Testes.Integracao.Modulos.ModuloTarefa;

[TestClass]
public sealed class RepositorioTarefaEmOrmTests : RepositorioBaseEmOrmTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRegistro()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(c => c.Titulo = "Titulo1")
            .With(c => c.Prioridade = PrioridadeTarefa.Baixa)

            .Build();

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual(tarefa.Id, tarefaSelecionada.Id);
        Assert.AreEqual(tarefa.Titulo, tarefaSelecionada.Titulo);
        Assert.AreEqual(tarefa.Prioridade, tarefaSelecionada.Prioridade);

    }

    [TestMethod]
    public void CadastrarESelecionarPorId_ComItens_CarregaRegistro()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(c => c.Titulo = "Titulo1")
            .With(c => c.Prioridade = PrioridadeTarefa.Baixa)

            .Build();

        tarefa.AdicionarItem(new ItemTarefa("Lata De Teste"));

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual(tarefa.Id, tarefaSelecionada.Id);
        Assert.AreEqual(tarefa.Titulo, tarefaSelecionada.Titulo);
        Assert.AreEqual(tarefa.Prioridade, tarefaSelecionada.Prioridade);
    }


    [TestMethod]
    public void CadastrarESelecionarPorId_ComPriorioridadeForaDoLimite_RetornaErro()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(c => c.Titulo = "Titulo1")
            .With(c => c.Prioridade = (PrioridadeTarefa)999)

            .Build();


        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Prioridade\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Editar_AtualizaRegistroExistente()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .Persist();

        Tarefa tarefaAtualizada = Builder<Tarefa>
            .CreateNew()
            .With(t => t.Titulo = "TituloAtualizado")
            .With(t => t.Prioridade = PrioridadeTarefa.Baixa)
            .Build();

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("TituloAtualizado", tarefaSelecionada.Titulo);
    }

    [TestMethod]
    public void Excluir_RemoveRegistroExistente()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .Persist();

        // Ação
        bool conseguiuExcluir = repositorioTarefa.Excluir(tarefa.Id);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionado = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(tarefaSelecionado);
    }

    [TestMethod]
    public void SelecionarTodos_CarregaRegistros()
    {
        // Arranjo / Ação
        IList<Tarefa> tarefas = Builder<Tarefa>
            .CreateListOfSize(3)
            .All()
            .Persist();

        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.HasCount(3, repositorioTarefa.SelecionarTodos());
    }
}