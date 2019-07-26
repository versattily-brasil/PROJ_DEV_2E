using NUnit.Framework;
using P2E.SSO.Infra.Data.DataContext;
using P2E.SSO.Infra.Data.Repositories;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using P2E.Shared.ValuesObject;
using P2E.Shared.Enum;

namespace P2E.SSO.Tests.ParceiroNegocio
{
    [TestFixture]
    public class ParceiroNegocioTest
    {
        [Test]
        public void TestarCriarSemRazaoSocial()
        {
            //var parceiro = new P2E.SSO.Domain.Entities.ParceiroNegocio("", new Document("02569562000186", eTIPODOC.CNPJ));
            //Assert.AreEqual(false, parceiro.Valid);
        }
    }
}
