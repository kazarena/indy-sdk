﻿using Hyperledger.Indy.PoolApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Hyperledger.Indy.Test.PoolTests
{
    [TestClass]
    public class CreatePoolTest : IndyIntegrationTestBase
    {
        [TestMethod]
        public async Task TestCreatePoolWorksForNullConfig()
        {
            var file = File.Create("testCreatePoolWorks.txn");
            PoolUtils.WriteTransactions(file, 1);

            await Pool.CreatePoolLedgerConfigAsync("testCreatePoolWorks", null);
        }

        [TestMethod]
        public async Task TestCreatePoolWorksForConfigJSON()
        {
            var genesisTxnFile = PoolUtils.CreateGenesisTxnFile("genesis.txn");
            var path = Path.GetFullPath(genesisTxnFile.Name).Replace('\\', '/');

            var configJson = string.Format("{{\"genesis_txn\":\"{0}\"}}", path);

            await Pool.CreatePoolLedgerConfigAsync("testCreatePoolWorks", configJson);
        }

        [TestMethod]
        public async Task TestCreatePoolWorksForTwice()
        {

            var genesisTxnFile = PoolUtils.CreateGenesisTxnFile("genesis.txn");
            var path = Path.GetFullPath(genesisTxnFile.Name).Replace('\\', '/');

            var configJson = string.Format("{{\"genesis_txn\":\"{0}\"}}", path);

            await Pool.CreatePoolLedgerConfigAsync("pool1", configJson);

            var ex = await Assert.ThrowsExceptionAsync<PoolLedgerConfigExistsException>(() =>
                Pool.CreatePoolLedgerConfigAsync("pool1", configJson)
            );;
        }
    }
}
