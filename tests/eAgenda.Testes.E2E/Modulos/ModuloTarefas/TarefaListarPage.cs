using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefas;

public sealed class TarefaListarPage(
    IPage page,
    string urlBase
)
{
    public string Url => $"{urlBase}/Tarefa/Listar";

    public ILocator Titulo => page.GetByRole(
        AriaRole.Heading,
        new() { Name = "Listagem de Tarefas" }
    );

    public ILocator CadastrarNova => page.GetByRole(
        AriaRole.Link,
        new() { Name = "Cadastrar Nova" }
    );

    public ILocator EstadoVazio => page.GetByText(
        "Nenhuma tarefa cadastrada.",
        new() { Exact = true }
    );

    public ILocator NomeDaTarefa(string titulo) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = titulo, Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorTitulo(string titulo)
    {
        ILocator tituloTarefa = NomeDaTarefa(titulo);

        return page.Locator(".card").Filter(new() { Has = tituloTarefa });
    }

    public ILocator StatusDaTarefa(string titulo)
    {
        return CardPorTitulo(titulo)
            .GetByText("Status", new() { Exact = true })
            .Locator("xpath=following-sibling::dd[1]");
    }

    public ILocator PercentualDaTarefa(string titulo)
    {
        return CardPorTitulo(titulo)
            .Locator(".progress-bar");
    }
}