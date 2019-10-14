using NUnit.Framework;
using P2E.Shared.Enum;
using System;

namespace P2E.SSO.Tests.Rotina
{
    [TestFixture]
    public class TesteRotina
    {
        [Test]
        public void CriarSemNome()
        {
            var rotina = new Domain.Entities.Rotina(String.Empty, "Descrição da Rotina 1", eTipoRotina.Cadastrar, 1, "");
            Assert.AreEqual(false, rotina.Valid);
        }

        [Test]
        public void CriarSemDescricao()
        {
            var rotina = new Domain.Entities.Rotina("Rotina 1", String.Empty, eTipoRotina.Cadastrar, 1, "");
            Assert.AreEqual(false, rotina.Valid);
        }

        [Test]
        public void CriarSemTipo()
        {
            var rotina = new Domain.Entities.Rotina("Rotina 1", "Descrição da Rotina 1", 0, 1, "");
            Assert.AreEqual(false, rotina.Valid);
        }
    }
}
