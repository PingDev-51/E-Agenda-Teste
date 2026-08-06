using eAgenda.Dominio.Modulos.ModuloContato;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContato;

[TestClass]
public sealed class ContatoTests
{
    [TestMethod]
    public void CadastrarContato_ComTodosOsCamposPreenchidos()
    {
        //Arange
        Contato contato = new Contato("Kauan", "kauazindelas145@gmail.com", "(49) 98883-1234", "dev", "amvn");

        //Act
        List<string> erros = contato.Validar();

        //Assert
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void CadastrarContato_ComOsCamposObrigatoriosPreenchidos()
    {
        //Arange
        Contato contato = new Contato("Kauan", "kauazindelas145@gmail.com", "(49) 98883-1234", null, string.Empty);

        //Act
        List<string> erros = contato.Validar();

        //Assert
        Assert.HasCount(0, erros);
    }


    [TestMethod]
    public void CadastrarContato_ComOsCamposObrigatoriosEmBranco()
    {
        //Arange
        Contato contato = new Contato(string.Empty, string.Empty, string.Empty, null, string.Empty);

        //Act
        List<string> erros = contato.Validar();

        //Assert
        Assert.HasCount(3, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros[0]
        );
        Assert.AreEqual(
            "O campo \"E-mail\" deve conter um endereço de e-mail válido.",
            erros[1]
        );
        Assert.AreEqual(
            "O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX.",
            erros[2]
        );
    }

    [TestMethod]
    public void CadastrarContato_ComONomeAbaixoDoMinimo()
    {
        //Arange
        Contato contato = new Contato("K", "kauazindelas145@gmail.com", "(49) 98883-1234", null, string.Empty);

        //Act
        List<string> erros = contato.Validar();

        //Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void CadastrarContato_ComONomeNoLimiteMinimo()
    {
        //Arange
        Contato contato = new Contato("KA", "kauazindelas145@gmail.com", "(49) 98883-1234", null, string.Empty);

        //Act
        List<string> erros = contato.Validar();

        //Assert
        Assert.HasCount(0, erros);
    }


    [TestMethod]
    public void CadastrarContato_ComONomeNoLimiteMaximo()
    {
        //Arange
        Contato contato = new Contato("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK", "kauazindelas145@gmail.com", "(49) 98883-1234", null, string.Empty);

        //Act
        List<string> erros = contato.Validar();

        //Assert
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void CadastrarContato_ComONomeAcimaDoLimiteMaximo()
    {
        // Arrange
        Contato contato = new Contato(
            "KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK",
            "kauazindelas145@gmail.com",
            "(49) 98883-1234",
            null,
            string.Empty
        );

        // Act
        List<string> erros = contato.Validar();

        // Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
          "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
          erros.First()
      );
    }

    [TestMethod]
    public void CadastrarContato_ComOEmailNoFormatoInvalido()
    {
        // Arrange
        Contato contato = new Contato(
            "Kauan",
            "kauazindelas145gmail.com",
            "(49) 98883-1234",
            null,
            string.Empty
        );

        // Act
        List<string> erros = contato.Validar();

        // Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
          "O campo \"E-mail\" deve conter um endereço de e-mail válido.",
          erros.First()
      );
    }
}