using System.Collections.Generic;
using Xunit;

namespace TestFiapTechChallenge.Testes
{
    public class CadastroContatosTestes
    {
        [Fact]
        public void AdicionarContato_DeveAdicionarNovoContato()
        {
            // Arrange
            var cadastro = new CadastroContatos();
            var contato = new Contato("Fulano", "123456789", "fulano@example.com", "DDD1");

            // Act
            cadastro.AdicionarContato(contato);

            // Assert
            Assert.Contains(contato, cadastro.Contatos);
        }

        // Outros testes para validação de entrada, etc.
    }

    public class ConsultaContatosTestes
    {
        [Fact]
        public void ConsultarContatosPorDDD_DeveRetornarContatosComOMesmoDDD()
        {
            // Arrange
            var consulta = new ConsultaContatos();
            var cadastro = new CadastroContatos();
            var contato1 = new Contato("Fulano", "123456789", "fulano@example.com", "DDD1");
            var contato2 = new Contato("Ciclano", "987654321", "ciclano@example.com", "DDD2");
            cadastro.AdicionarContato(contato1);
            cadastro.AdicionarContato(contato2);

            // Act
            var contatosFiltrados = consulta.ConsultarContatosPorDDD("DDD1");

            // Assert
            Assert.Contains(contato1, contatosFiltrados);
            Assert.DoesNotContain(contato2, contatosFiltrados);
        }

        // Outros testes para casos de consulta vazia, etc.
    }

    public class AtualizacaoExclusaoContatosTestes
    {
        [Fact]
        public void AtualizarContato_DeveAtualizarContatoExistente()
        {
            // Arrange
            var cadastro = new CadastroContatos();
            var contato = new Contato("Fulano", "123456789", "fulano@example.com", "DDD1");
            cadastro.AdicionarContato(contato);
            var novoContato = new Contato("Beltrano", "987654321", "beltrano@example.com", "DDD1");

            // Act
            cadastro.AtualizarContato(contato, novoContato);

            // Assert
            Assert.Contains(novoContato, cadastro.Contatos);
            Assert.DoesNotContain(contato, cadastro.Contatos);
        }

        [Fact]
        public void ExcluirContato_DeveExcluirContatoExistente()
        {
            // Arrange
            var cadastro = new CadastroContatos();
            var contato = new Contato("Fulano", "123456789", "fulano@example.com", "DDD1");
            cadastro.AdicionarContato(contato);

            // Act
            cadastro.ExcluirContato(contato);

            // Assert
            Assert.DoesNotContain(contato, cadastro.Contatos);
        }

        // Outros testes para casos de atualização/exclusão de contatos inexistentes, etc.
    }
}