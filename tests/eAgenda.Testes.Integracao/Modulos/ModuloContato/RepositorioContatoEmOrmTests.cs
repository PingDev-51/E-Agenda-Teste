using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgenda.Testes.Integracao.Modulos.ModuloContato;

[TestClass]
public sealed class RepositorioContatoEmOrmTests() : RepositorioBaseEmOrmTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRegistro()
    {
        // Arranjo
        Contato contato = Builder<Contato>
            .CreateNew()
            .With(c => c.Nome = "Nome1")
            .With(c => c.Telefone = "(00) 00000-0000")
            .With(c => c.Email = "test@gmail.com")
            .With(c => c.Cargo = "testador")
            .With(c => c.Empresa = "Testes")

            .Build();

        // Ação
        repositorioContato.Cadastrar(contato);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual(contato.Id, contatoSelecionado.Id);
        Assert.AreEqual("Nome1", contatoSelecionado.Nome);
        Assert.AreEqual("(00) 00000-0000", contatoSelecionado.Telefone);
        Assert.AreEqual("test@gmail.com", contatoSelecionado.Email);
        Assert.AreEqual("testador", contatoSelecionado.Cargo);
        Assert.AreEqual("Testes", contatoSelecionado.Empresa);
    }


    [TestMethod]
    public void CadastrarESelecionarPorId_SemCamposObrigatorios()
    {
        // Arranjo
        Contato contato = Builder<Contato>
            .CreateNew()
            .With(c => c.Nome = "Nome1")
            .With(c => c.Telefone = "(00) 00000-0000")
            .With(c => c.Email = "test@gmail.com")
            .With(c => c.Cargo = null)
            .With(c => c.Empresa = null)
            .Build();

        // Ação
        repositorioContato.Cadastrar(contato);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual(contato.Id, contatoSelecionado.Id);
        Assert.AreEqual("Nome1", contatoSelecionado.Nome);
        Assert.AreEqual("(00) 00000-0000", contatoSelecionado.Telefone);
        Assert.AreEqual("test@gmail.com", contatoSelecionado.Email);
        Assert.IsNull(contatoSelecionado.Cargo);
        Assert.IsNull(contatoSelecionado.Empresa);
    }

    [TestMethod]
    public void Editar_AtualizaRegistroExistente()
    {
        // Arranjo
        Contato contato = Builder<Contato>
            .CreateNew()
            .Persist();

        Contato contatoAtualizado = Builder<Contato>
            .CreateNew()
            .With(d => d.Nome = "NomeAtualizado")
            .With(c => c.Telefone = "(00) 00000-0000")
            .With(c => c.Email = "test@gmail.com")
            .With(c => c.Cargo = null)
            .With(c => c.Empresa = null)
            .Build();

        // Ação
        bool conseguiuEditar = repositorioContato.Editar(contato.Id, contatoAtualizado);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("NomeAtualizado", contatoSelecionado.Nome);
    }


}


