using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloItemTarefa;

[TestClass]
public sealed class ItemTarefaTests
{
    [TestMethod]
    public void CadastrarItem_ComTodosOsCamposPreenchidos()
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

    [TestMethod]
    public void CadastrarItem_SemTitulo()
    {
        // Arrange
        ItemTarefa itemTarefa = new ItemTarefa(
            string.Empty
        );

        // Act
        List<string> erros = itemTarefa.Validar();

        // Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
           "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
           erros.First()
       );
    }

    [TestMethod]
    public void CadastrarItem_ComTituloAbaixoDoMinimo()
    {
        // Arrange
        ItemTarefa itemTarefa = new ItemTarefa(
            "T"
        );

        // Act
        List<string> erros = itemTarefa.Validar();

        // Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
           "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
           erros.First()
       );
    }

    [TestMethod]
    public void CadastrarItem_ComTituloAcumaMinimo()
    {
        // Arrange
        string tarefa = new string('t', 101);

        ItemTarefa itemTarefa = new ItemTarefa(
            tarefa
        );

        // Act
        List<string> erros = itemTarefa.Validar();

        // Assert
        Assert.HasCount(1, erros);
        Assert.AreEqual(
           "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
           erros.First()
       );
    }

}