using eAgenda.Testes.E2E.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefas;

[TestClass]
public sealed class ItemTarefaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveCadastrar_Tarefa_ComTodosOsDadosValidos()
    {
        // Arrange
        TarefaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Act
        await formPage.PreencherAsync(
            "Tarefa Teste",
            "Normal"
        );

        await formPage.ConfirmarAsync();

        // Assert
        Assert.AreEqual(
            "/Tarefa/Listar",
            new Uri(Page.Url).AbsolutePath
        );

        TarefaListarPage listarPage = new(Page, UrlBase);

        await Expect(listarPage.NomeDaTarefa("Tarefa Teste"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveAdicionar_Item_EmTarefaExistente()
    {
        // Arrange
        TarefaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            "Tarefa Teste",
            "Normal"
        );

        await formPage.ConfirmarAsync();

        TarefaListarPage listarPage = new(Page, UrlBase);

        await Expect(listarPage.NomeDaTarefa("Tarefa Teste"))
            .ToBeVisibleAsync();

        await Page.GetByRole(
            AriaRole.Link,
            new() { Name = "Itens", Exact = true }
        ).ClickAsync();

        // Act
        await Page.GetByLabel("Novo Item")
            .FillAsync("Item Teste");

        await Page.GetByRole(
            AriaRole.Button,
            new() { Name = "Adicionar", Exact = true }
        ).ClickAsync();

        // Assert
        await Expect(
            Page.GetByText("Item Teste", new() { Exact = true })
        ).ToBeVisibleAsync();
    }
}