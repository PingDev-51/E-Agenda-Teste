using eAgenda.Testes.E2E.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefas;

[TestClass]
public sealed class TarefaE2ETests : E2ETestsBase
{
    [TestMethod] //Para Testar o E2E
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemTarefas()
    {

        // Act
        await Page.GotoAsync($"{UrlBase}/Tarefa/Listar");

        // Assert
        Assert.AreEqual(
            "/Tarefa/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        // Heading = h1, h2, h3, h4, h5, h6
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Listagem de Tarefas" }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Nova" }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("Nenhuma tarefa cadastrada.", new() { Exact = true }))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Tarefa_ComTodosOsDadosValidos()
    {
        // Arrange

        await Page.GotoAsync($"{UrlBase}/Tarefa/Listar");

        // Act
        await Page.GetByRole(AriaRole.Link, new() { Name = "Cadastrar Nova" })
            .ClickAsync();

        await Page.GetByLabel("Título").FillAsync("Test1");
        await Page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" })
            .ClickAsync();

        // Assert
        Assert.AreEqual(
            "/Tarefa/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        await Expect(Page.GetByText("Test1", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("Nenhuma tarefa cadastrada.", new() { Exact = true }))
            .Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveConcluir_Tarefa()
    {
        // Arrange

        await Page.GotoAsync($"{UrlBase}/Tarefa/Listar");

        await CadastarTarefaAsync("test", "Normal");

        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name = "Concluir" })
            .ClickAsync();


        // Assert
        Assert.AreEqual(
            "/Tarefa/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        await Expect(Page.GetByText("Concluída", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText("100%", new() { Exact = true }))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveReabrir_Tarefa()
    {
        // Arrange
        await Page.GotoAsync($"{UrlBase}/Tarefa/Listar");

        await CadastarTarefaAsync("test", "Normal");

        // Act
        await Page.GetByRole(
            AriaRole.Button,
            new() { Name = "Concluir" }
        ).ClickAsync();

        await Page.GetByRole(
            AriaRole.Button,
            new() { Name = "Reabrir" }
        ).ClickAsync();

        // Assert
        Assert.AreEqual(
            "/Tarefa/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        TarefaListarPage listarPage = new(Page, UrlBase);

        await Expect(listarPage.StatusDaTarefa("test"))
            .ToHaveTextAsync("Pendente");

        await Expect(listarPage.PercentualDaTarefa("test"))
            .ToHaveTextAsync("0%");
    }

    [TestMethod]
    public async Task DeveExibir_TodasAsTarefas_IndependenteDoStatus()
    {
        // Arrange
        await Page.GotoAsync($"{UrlBase}/Tarefa/Listar");

        await CadastarTarefaAsync("Tarefa Pendente", "Normal");
        await CadastarTarefaAsync("Tarefa Concluida", "Alta");

        // Act
        await Page.GetByRole(
            AriaRole.Button,
            new() { Name = "Concluir" }
        ).Last.ClickAsync();

        await Page.GetByRole(
            AriaRole.Link,
            new() { Name = "Todas", Exact = true }
        ).ClickAsync();

        // Assert
        TarefaListarPage listarPage = new(Page, UrlBase);

        await Expect(listarPage.NomeDaTarefa("Tarefa Pendente"))
            .ToBeVisibleAsync();

        await Expect(listarPage.NomeDaTarefa("Tarefa Concluida"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_TarefaESeusItensVinculados()
    {
        // Arrange
        await Page.GotoAsync($"{UrlBase}/Tarefa/Listar");

        await CadastarTarefaAsync("Tarefa Para Excluir", "Normal");

        TarefaListarPage listarPage = new(Page, UrlBase);

        await Expect(listarPage.NomeDaTarefa("Tarefa Para Excluir"))
            .ToBeVisibleAsync();

        // Act
        await listarPage.ExcluirAsync("Tarefa Para Excluir");

        await Page.GetByRole(
            AriaRole.Button,
            new() { Name = "Confirmar", Exact = true }
        ).ClickAsync();

        // Assert
        await Expect(listarPage.NomeDaTarefa("Tarefa Para Excluir"))
            .Not.ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .ToBeVisibleAsync();
    }


    private async Task CadastarTarefaAsync(string nome, string prioridade)
    {
        TarefaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(nome, prioridade);

        await formPage.ConfirmarAsync();

        TarefaListarPage listarPage = new(Page, UrlBase);

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
    }
}

