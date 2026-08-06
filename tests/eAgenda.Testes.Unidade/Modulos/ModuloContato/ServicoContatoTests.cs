using eAgenda.Aplicacao.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContato;

[TestClass]
public sealed class ServicoContatoTests
{
    [TestMethod]
    public void Cadastrar_EmailDuplicado_RetornaFalha()
    {
        //Arrange
        Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([new Contato("Kauan", "kauazindelas145@gmail.com", "(49) 98883-1234", "dev", "amvn")]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        //Act
        Result resultado = servicoContato
            .Cadastrar(new CadastrarContatoDto("Kauan", "kauazindelas145@gmail.com", "(49) 98883-1234", "dev", "amvn"));

        //Assert
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Email", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);

        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_TelefoneDuplicado_RetornaFalha()
    {
        //Arrange
        Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([new Contato("Kauan", "kauazindelas145@gmail.com", "(49) 98883-1234", "dev", "amvn")]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        //Act
        Result resultado = servicoContato
            .Cadastrar(new CadastrarContatoDto("neymar", "neymartest321@gmail.com", "(49) 98883-1234", "jogador", "santos"));

        //Assert
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Telefone", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);

        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }
}
