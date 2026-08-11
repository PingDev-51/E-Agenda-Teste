using FizzWare.NBuilder;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloCategoria;
using eAgenda.Infra.Modulos.ModuloCompromisso;
using eAgenda.Infra.Modulos.ModuloContato;
using eAgenda.Infra.Modulos.ModuloDespesa;
using eAgenda.Infra.Modulos.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using eAgenda.Testes.Integracao.Modulos.ModuloTarefa;

namespace eAgenda.Testes.Integracao.Compartilhado.Orm;

public abstract class RepositorioBaseEmOrmTests
{
    protected EAgendaDbContext dbContext = null!;
    protected RepositorioCategoriaEmOrm repositorioCategoria = null!;
    protected RepositorioCompromissoEmOrm repositorioCompromisso = null!;
    protected RepositorioContatoEmOrm repositorioContato = null!;
    protected RepositorioDespesaEmOrm repositorioDespesa = null!;
    protected RepositorioTarefaEmOrm repositorioTarefa = null!;

    [TestInitialize]
    public void InicializarContexto()
    {
        var options = new DbContextOptionsBuilder<EAgendaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        dbContext = new EAgendaDbContext(options);

        // Categoria
        repositorioCategoria = new RepositorioCategoriaEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Categoria>(
            repositorioCategoria.Cadastrar);

        BuilderSetup.SetCreatePersistenceMethod<IList<Categoria>>(
            categorias =>
            {
                foreach (Categoria categoria in categorias)
                    repositorioCategoria.Cadastrar(categoria);
            });

        // Compromisso
        repositorioCompromisso = new RepositorioCompromissoEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Compromisso>(
            repositorioCompromisso.Cadastrar);

        BuilderSetup.SetCreatePersistenceMethod<IList<Compromisso>>(
            compromissos =>
            {
                foreach (Compromisso compromisso in compromissos)
                    repositorioCompromisso.Cadastrar(compromisso);
            });

        // Contato
        repositorioContato = new RepositorioContatoEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Contato>(
            repositorioContato.Cadastrar);

        BuilderSetup.SetCreatePersistenceMethod<IList<Contato>>(
            (Action<IList<Contato>>)(contatos =>
            {
                foreach (Contato contato in contatos)
                    this.repositorioContato.Cadastrar(contato);
            }));

        // Despesa
        repositorioDespesa = new RepositorioDespesaEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Despesa>(
            repositorioDespesa.Cadastrar);

        BuilderSetup.SetCreatePersistenceMethod<IList<Despesa>>(
            despesas =>
            {
                foreach (Despesa despesa in despesas)
                    repositorioDespesa.Cadastrar(despesa);
            });

        // Tarefa
        repositorioTarefa = new RepositorioTarefaEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Tarefa>(
            repositorioTarefa.Cadastrar);

        BuilderSetup.SetCreatePersistenceMethod<IList<Tarefa>>(
            (Action<IList<Tarefa>>)(tarefas =>
            {
                foreach (Tarefa tarefa in tarefas)
                    this.repositorioTarefa.Cadastrar(tarefa);
            }));
    }

    [TestCleanup]
    public void DescartarContexto()
    {
        dbContext?.Dispose();
    }
}

