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
        //private FamilyService _family = new FamilyService();

        //private SevenService _seven = new SevenService();

        public void ShippingByStore(List<Order> orders)
        {
            foreach (var order in orders)
            {
                // simple factory pattern implementation
                IStoreService storeService = SimpleFactory.GetStoreService(order);
                storeService.Ship(order);
            }
        }

        //private IStoreService GetStoreService(Order order)
        //{
        //    if (order.StoreType == StoreType.Family)
        //    {
        //        return this._family;
        //    }
        //    else
        //    {
        //        return this._seven;
        //    }
        //}
    }

    public class SimpleFactory
    {
        private static IStoreService sevenService = new SevenService();
        private static IStoreService familyService = new FamilyService();

        public static IStoreService GetStoreService(Order order)
        {
            if (order.StoreType == StoreType.Family)
            {
                return sevenService;
            }
            else
            {
                return familyService;
            }
        }
    }
}