using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleFactoryLegacy.Test
{
    /// <summary>
    /// Summary description for ShipServiceTest
    /// </summary>
    [TestClass]
    public class ShipServiceTest
    {
        public ShipServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes

        [TestMethod]
        public void TestShippingByStore_Seven_1_Order_Family_2_Orders()
        {
            //arrange
            var target = new ShipService();

            var orders = new List<Order>
            {
                new Order{ StoreType= StoreType.Seven, Id=1},
                new Order{ StoreType= StoreType.Family, Id=2},
                new Order{ StoreType= StoreType.Family, Id=3},
            };

            //set stub by simple factory's internal setter
            var stubSeven = Substitute.For<IStoreService>();
            SimpleFactory.SetSevenService(stubSeven);

            var stubFamily = Substitute.For<IStoreService>();
            SimpleFactory.SetFamilyService(stubFamily);

            //act
            target.ShippingByStore(orders);

            //assert
            //ShipService should invoke SevenService once and FamilyService twice
            stubSeven.Received(1).Ship(Arg.Is<Order>(x => x.StoreType == StoreType.Seven));
            stubFamily.Received(2).Ship(Arg.Is<Order>(x => x.StoreType == StoreType.Family));
        }
    }
}