using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace SimpleFactoryLegacy
{
    public enum StoreType
    {
        /// <summary>
        /// 7-11
        /// </summary>
        Seven = 0,

        /// <summary>
        /// 全家
        /// </summary>
        Family = 1
    }

    public interface IStoreService
    {
        void Ship(Order order);
    }

    public class FamilyService : IStoreService
    {
        public void Ship(Order order)
        {
            // family web service
            var client = new HttpClient();
            client.PostAsync("http://api.family.com/Order", order, new JsonMediaTypeFormatter());
        }
    }

    public class Order
    {
        public int Amount { get; set; }

        public int Id { get; set; }

        public StoreType StoreType { get; set; }
    }

    public class SevenService : IStoreService
    {
        public void Ship(Order order)
        {
            // seven web service
            var client = new HttpClient();
            client.PostAsync("http://api.seven.com/Order", order, new JsonMediaTypeFormatter());
        }
    }

    public class ShipService
    {
        public void ShippingByStore(List<Order> orders)
        {
            foreach (var order in orders)
            {
                //todo, directly depend on simple factory static function, how to test it?
                IStoreService storeService = SimpleFactory.GetStoreService(order);
                storeService.Ship(order);
            }
        }
    }

    public class SimpleFactory
    {
        //private static IStoreService sevenService = new SevenService();
        private static IStoreService sevenService;

        //private static IStoreService familyService = new FamilyService();
        private static IStoreService familyService;

        //add a internal SevenService setter for test project to inject stub/mock object
        internal static void SetSevenService(IStoreService stub)
        {
            sevenService = stub;
        }

        //add a internal FamilyService setter for test project to inject stub/mock object
        internal static void SetFamilyService(IStoreService stub)
        {
            familyService = stub;
        }

        public static IStoreService GetStoreService(Order order)
        {
            if (order.StoreType == StoreType.Family)
            {
                return sevenService ?? new SevenService();
            }
            else
            {
                return familyService ?? new FamilyService();
            }
        }
    }
}