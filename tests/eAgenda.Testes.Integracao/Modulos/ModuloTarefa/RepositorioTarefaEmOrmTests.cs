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
}