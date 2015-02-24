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
        private FamilyService _family = new FamilyService();

        private SevenService _seven = new SevenService();

        public void ShippingByStore(List<Order> orders)
        {
            //// handle seven's orders
            //var ordersBySeven = orders.Where(x => x.StoreType != StoreType.Family);
            //var sevenService = new SevenService();
            //foreach (var order in ordersBySeven)
            //{
            //    sevenService.Ship(order);
            //}

            //// handle family's orders
            //var ordersByFamily = orders.Where(x => x.StoreType == StoreType.Family);
            //var familyService = new FamilyService();
            //foreach (var order in ordersByFamily)
            //{
            //    familyService.Ship(order);
            //}

            foreach (var order in orders)
            {
                // strategy pattern implementation
                IStoreService storeService = GetStoreService(order);
                storeService.Ship(order);
            }
        }

        private IStoreService GetStoreService(Order order)
        {
            if (order.StoreType == StoreType.Family)
            {
                return this._family;
            }
            else
            {
                return this._seven;
            }
        }
    }
}