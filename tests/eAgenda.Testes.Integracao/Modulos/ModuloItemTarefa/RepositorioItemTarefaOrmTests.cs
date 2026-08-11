using eAgenda.Dominio.Modulos.ModuloTarefa;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;
using Microsoft.Testing.Platform.Requests;

namespace eAgenda.Testes.Integracao.Modulos.ModuloItemTarefa;

[TestClass]
public sealed class RepositorioItemTarefaOrmTests : RepositorioBaseEmOrmTests
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

        ItemTarefa itemTarefa = new("Lata de testes");

        tarefa.AdicionarItem(itemTarefa);

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
    public void CadastrarESelecionarPorId_ComCincoItensPendentes_CarregaRegistro()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(c => c.Titulo = "Titulo1")
            .With(c => c.Prioridade = PrioridadeTarefa.Baixa)

            .Build();

        ItemTarefa itemTarefa = new("Lata de testes");

        ItemTarefa itemTarefa2 = new("Lata de testes2");

        ItemTarefa itemTarefa3 = new("Lata de testes3");

        ItemTarefa itemTarefa4 = new("Lata de testes4");

        ItemTarefa itemTarefa5 = new("Lata de testes5");

        tarefa.AdicionarItem(itemTarefa);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AdicionarItem(itemTarefa4);
        tarefa.AdicionarItem(itemTarefa5);

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
    public void CadastrarESelecionarPorId_ComTresItensConcluidos_CarregaRegistro()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(c => c.Titulo = "Titulo1")
            .With(c => c.Prioridade = PrioridadeTarefa.Baixa)

            .Build();

        ItemTarefa itemTarefa = new("Lata de testes");

        ItemTarefa itemTarefa2 = new("Lata de testes2");

        ItemTarefa itemTarefa3 = new("Lata de testes3");

        ItemTarefa itemTarefa4 = new("Lata de testes4");

        ItemTarefa itemTarefa5 = new("Lata de testes5");

        tarefa.AdicionarItem(itemTarefa);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AdicionarItem(itemTarefa4);
        tarefa.AdicionarItem(itemTarefa5);

        itemTarefa.Concluido = true;
        itemTarefa2.Concluido = true;
        itemTarefa3.Concluido = true;


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
    public void CadastrarESelecionarPorId_ComQuatroItensConcluidos_CarregaRegistro()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(c => c.Titulo = "Titulo1")
            .With(c => c.Prioridade = PrioridadeTarefa.Baixa)

            .Build();

        ItemTarefa itemTarefa = new("Lata de testes");

        ItemTarefa itemTarefa2 = new("Lata de testes2");

        ItemTarefa itemTarefa3 = new("Lata de testes3");

        ItemTarefa itemTarefa4 = new("Lata de testes4");

        ItemTarefa itemTarefa5 = new("Lata de testes5");

        tarefa.AdicionarItem(itemTarefa);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AdicionarItem(itemTarefa4);
        tarefa.AdicionarItem(itemTarefa5);

        itemTarefa.Concluido = true;
        itemTarefa2.Concluido = true;
        itemTarefa3.Concluido = true;
        itemTarefa4.Concluido = true;

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

}
