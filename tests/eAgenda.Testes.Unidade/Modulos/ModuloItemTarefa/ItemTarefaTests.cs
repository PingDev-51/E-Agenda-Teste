using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloItemTarefa;

[TestClass]
public sealed class ItemTarefaTests
{
    [TestMethod]
    public void CadastrarTarefa_ComTodosOsCamposPreenchidos()
    {
        // Arrange
        ItemTarefa itemTarefa = new ItemTarefa(
            "Testar"
        );

        // Act
        List<string> erros = itemTarefa.Validar();

        // Assert
        Assert.HasCount(0, erros);
    }
}