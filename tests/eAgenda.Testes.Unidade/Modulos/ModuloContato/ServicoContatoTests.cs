using System.Runtime.Versioning;
using eAgenda.Aplicacao.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentAssertions;
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

    [TestMethod]
    public void Editar_Contato_ComDadosValidos()
    {
        // Arrange
        Guid contatoId = Guid.CreateVersion7();

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        repositorioContato
            .Setup(r => r.Editar(contatoId, It.IsAny<Contato>()))
            .Returns(true);

        ServicoContato servicoContato = new(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Act
        Result resultado = servicoContato.Editar(
            new EditarContatoDto(
                contatoId,
                "Neymar",
                "neymar@gmail.com",
                "(49) 98883-1234",
                "Jogador",
                "Santos"
            )
        );

        // Assert
        resultado.IsSuccess.Should().BeTrue();

        repositorioContato.Verify(
            r => r.Editar(contatoId, It.IsAny<Contato>()),
            Times.Once
        );
    }

    [TestMethod]
    public void Editar_EmailDuplicado_RetornaFalha()
    {
        //Arrange
        Guid contatoId = Guid.CreateVersion7();
        Guid outroContatoId = Guid.CreateVersion7();

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        Contato contatoExistente = new(
            "João",
            "neymar@gmail.com",
            "(49) 99999-9999",
            "Amigo",
            "Casa"
        );

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contatoExistente]);

        repositorioContato
            .Setup(r => r.Editar(contatoId, It.IsAny<Contato>()))
            .Returns(true);

        ServicoContato servicoContato = new(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        //Act
        Result resultado = servicoContato.Editar(
            new EditarContatoDto(
                contatoId,
                "Neymar",
                "neymar@gmail.com",
                "(49) 98883-1234",
                "Jogador",
                "Santos"
            )
        );

        //Assert
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Email", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);

        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_Contato_MantendoEmail_E_Telefone()
    {
        // Arrange
        Guid contatoId = Guid.CreateVersion7();

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        Contato contatoExistente = new(
            "Neymar",
            "neymar@gmail.com",
            "(49) 98883-1234",
            "Jogador",
            "Santos"
        )
        {
            Id = contatoId
        };

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contatoExistente]);

        repositorioContato
            .Setup(r => r.Editar(contatoId, It.IsAny<Contato>()))
            .Returns(true);

        ServicoContato servicoContato = new(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Act
        Result resultado = servicoContato.Editar(
            new EditarContatoDto(
                contatoId,
                "Vini jr",
                "neymar@gmail.com",
                "(49) 98883-1234",
                "Jogador",
                "Real Madrid"
            )
        );

        // Assert
        resultado.IsSuccess.Should().BeTrue();

        repositorioContato.Verify(
            r => r.Editar(contatoId, It.IsAny<Contato>()),
            Times.Once
        );
    }

    [TestMethod]
    public void SelecionarTodos_DeveRetornarTodosOsContatos()
    {
        // Arrange
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        Contato contato1 = new(
            "Kauan S.",
            "kauans@gmail.com",
            "(49) 99999-9999",
            "Programador",
            "Empresa Fantasma"
        );

        Contato contato2 = new(
            "Pedrinho",
            "pedrinhoteste@gmail.com",
            "(49) 98888-8888",
            "Designer",
            "Empresa Fantasma"
        );

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([contato1, contato2]);

        ServicoContato servicoContato = new(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Act
        List<ListarContatosDto> resultado = servicoContato.SelecionarTodos();

        // Assert
        resultado.Should().HaveCount(2);

        resultado.Should().BeEquivalentTo(
        new ListarContatosDto(
         contato1.Id,
         contato1.Nome,
         contato1.Email,
         contato1.Telefone,
         contato1.Cargo,
         contato1.Empresa
        ),
        new ListarContatosDto(
         contato2.Id,
         contato2.Nome,
         contato2.Email,
         contato2.Telefone,
         contato2.Cargo,
         contato2.Empresa
     )
    );
        repositorioContato.Verify(
            r => r.SelecionarTodos(), Times.Once
        );
    }


    [TestMethod]
    public void Excluir_ContatosSemCompromissosVinculados()
    {
        // Arrange
        Contato contato = new("Kauan S.","kauazindelas145@gmail.com", "(49) 98883-1234", null, string.Empty);

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato
            .Setup(r => r.SelecionarPorId(contato.Id))
            .Returns(contato);

        repositorioCompromisso
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        ServicoContato servicoContato = new(
            repositorioContato.Object,
            repositorioCompromisso.Object);

        // Act
        Result resultado = servicoContato.Excluir(contato.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);

        repositorioContato.Verify(
            r => r.Excluir(It.IsAny<Guid>()),
            Times.Once);
    }
}