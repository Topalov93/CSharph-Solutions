using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables.Contracts;
using Bakery.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Models.Tables
{
    public abstract class Table : ITable
    {
        private IList<IBakedFood> FoodOrders;
        private IList<IDrink> DrinkOrders;

        private int capacity;
        private int numberOfPeople;

        public int TableNumber { get; }
        public int Capacity
        {
            get
            {
                return this.capacity;
            }
            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidTableCapacity);
                }
                this.capacity = value;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return this.numberOfPeople;
            }
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidNumberOfPeople);
                }
                this.numberOfPeople = value;
            }
        }

        public decimal PricePerPerson { get; }

        public bool IsReserved { get; set; }

        public decimal Price => this.NumberOfPeople * this.PricePerPerson;

        public Table(int tableNumber, int capacity, decimal pricePerPerson)
        {
            this.TableNumber = tableNumber;
            this.Capacity = capacity;
            this.PricePerPerson = pricePerPerson;

            this.FoodOrders = new List<IBakedFood>();
            this.DrinkOrders = new List<IDrink>();
        }

        public void Clear()
        {
            this.IsReserved = false;
            this.NumberOfPeople = 0;
            this.FoodOrders.Clear();
            this.DrinkOrders.Clear();
            
        }

        public decimal GetBill()
        {
            decimal bill = 0;

            foreach (var food in FoodOrders)
            {
                bill += food.Price;
            }

            foreach (var drink in DrinkOrders)
            {
                bill += drink.Price;
            }
            return bill+this.Price;
        }

        public string GetFreeTableInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Table: {this.TableNumber}");
            sb.AppendLine($"Type: {this.GetType().Name}");
            sb.AppendLine($"Capacity: {this.Capacity}");
            sb.AppendLine($"Price per Person: {this.PricePerPerson}");

            return sb.ToString().TrimEnd();
        }

        public void OrderDrink(IDrink drink)
        {
            if (drink.GetType().Name == "Tea" || drink.GetType().Name == "Water")
            {
                DrinkOrders.Add(drink);
            }
        }

        public void OrderFood(IBakedFood food)
        {
            if (food.GetType().Name == "Bread" || food.GetType().Name == "Cake")
            {
                FoodOrders.Add(food);
            }
        }

        public void Reserve(int numberOfPeople)
        {
            if (this.IsReserved == false)
            {
                if (this.Capacity >= numberOfPeople)
                {
                    this.IsReserved = true;
                    this.NumberOfPeople = numberOfPeople;
                }
            }
        }
    }
}
