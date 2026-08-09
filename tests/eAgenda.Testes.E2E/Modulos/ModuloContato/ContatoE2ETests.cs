namespace eAgenda.Testes.E2E.Modulos.ModuloContato;

using eAgenda.Testes.E2E.Compartilhado;
using Microsoft.Playwright;

[TestClass]
public sealed class ContatoE2ETests : E2ETestsBase
{
    [TestMethod] //Para Testar o E2E
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemContatos()
    {

        // Act
        await Page.GotoAsync($"{UrlBase}/Contato/Listar");

        // Assert
        Assert.AreEqual(
            "/Contato/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        // Heading = h1, h2, h3, h4, h5, h6
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Listagem de Contatos" }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Novo" }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("Nenhum contato cadastrado.", new() { Exact = true }))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Contato_ComTodosOsDadosValidos()
    {
        // Arrange

        await Page.GotoAsync($"{UrlBase}/Contato/Listar");

        // Act
        await Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Novo" })
            .ClickAsync();

        await Page.GetByLabel("Nome").FillAsync("Test1");
        await Page.GetByLabel("E-mail").FillAsync("test@gmail.com");
        await Page.GetByLabel("Telefone").FillAsync("(49) 00000-0000");
        await Page.GetByLabel("Cargo").FillAsync("DEV");
        await Page.GetByLabel("Empresa").FillAsync("Empresa fantasma");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" })
            .ClickAsync();

        // Assert
        Assert.AreEqual(
            "/Contato/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        await Expect(Page.GetByText("Test1", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("Nenhum contato cadastrado.", new() { Exact = true }))
            .Not.ToBeVisibleAsync();
    }


    [TestMethod]
    public async Task ImpedirCadastrar_Contato_ComEmailJaCadastrado()
    {
        // Arrange

        await Page.GotoAsync($"{UrlBase}/Contato/Listar");

        // Act
        await Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Novo" })
            .ClickAsync();

        await Page.GetByLabel("Nome").FillAsync("Test1");
        await Page.GetByLabel("E-mail").FillAsync("test@gmail.com");
        await Page.GetByLabel("Telefone").FillAsync("(49) 00000-0000");
        await Page.GetByLabel("Cargo").FillAsync("DEV");
        await Page.GetByLabel("Empresa").FillAsync("Empresa fantasma");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" })
            .ClickAsync();


        await Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Novo" })
          .ClickAsync();

        await Page.GetByLabel("Nome").FillAsync("Test2");
        await Page.GetByLabel("E-mail").FillAsync("test@gmail.com");
        await Page.GetByLabel("Telefone").FillAsync("(00) 00000-0000");
        await Page.GetByLabel("Cargo").FillAsync("DEV");
        await Page.GetByLabel("Empresa").FillAsync("Empresa fantasma");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" })
            .ClickAsync();


        // Assert
        Assert.AreEqual(
            "/Contato/Cadastrar",
            new Uri(Page.Url).AbsolutePath
        );

        await Expect(Page.GetByText("Já existe um contato com este email.", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("Nenhum contato cadastrado.", new() { Exact = true }))
            .Not.ToBeVisibleAsync();
    }


    [TestMethod]
    public async Task ImpedirCadastrar_Contato_ComTelefoneInvalido()
    {
        // Arrange

        await Page.GotoAsync($"{UrlBase}/Contato/Listar");

        // Act
        await Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Novo" })
            .ClickAsync();

        await Page.GetByLabel("Nome").FillAsync("Test1");
        await Page.GetByLabel("E-mail").FillAsync("test@gmail.com");
        await Page.GetByLabel("Telefone").FillAsync("49000000000");
        await Page.GetByLabel("Cargo").FillAsync("DEV");
        await Page.GetByLabel("Empresa").FillAsync("Empresa fantasma");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" })
            .ClickAsync();


        // Assert
        Assert.AreEqual(
            "/Contato/Cadastrar",
            new Uri(Page.Url).AbsolutePath
        );

        await Expect(Page.GetByText("O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX.", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("Nenhum contato cadastrado.", new() { Exact = true }))
            .Not.ToBeVisibleAsync();
    }


    [TestMethod]
    public async Task DeveEditar_Contato_ComDadosValidos()
    {
        // Arrange

        await CadastarContatoAsync("teste", "test@gmail.com", "(00) 00000-0000", "dev", "EmpresaFantasma");

        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        await listarPage.EditarAsync("teste");

        // Act
        await formPage.PreencherAsync("TesteEditado", "test@gmail.com", "(00) 00000-0000", "dev", "EmpresaFantasma");
        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("TesteEditado")).ToBeVisibleAsync();
        await Expect(listarPage.NomeDoContato("teste")).Not.ToBeVisibleAsync();
    }


    private async Task CadastarContatoAsync(string nome, string email, string telefone, string cargo, string empresa)
    {
        ContatoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(nome, email, telefone, cargo, empresa);

        await formPage.ConfirmarAsync();

        ContatoListarPage listarPage = new(Page, UrlBase);

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
    }

}