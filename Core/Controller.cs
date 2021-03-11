using Bakery.Models.BakedFoods;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables;
using Bakery.Models.Tables.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Core.Contracts
{
    public class Controller : IController
    {
        private ICollection<IBakedFood> bakedFoods;
        private ICollection<IDrink> drinks;
        private ICollection<ITable> tables;
        private decimal totalIncome;

        public Controller()
        {
            this.bakedFoods = new List<IBakedFood>();
            this.drinks = new List<IDrink>();
            this.tables = new List<ITable>();
        }

        public string AddDrink(string type, string name, int portion, string brand)
        {

            if (type == "Tea")
            {
                drinks.Add(new Tea(name, portion, brand));
                return $"added {name} ({brand}) to the drink menu";
            }
            else if (type == "Water")
            {
                drinks.Add(new Water(name, portion, brand));
                return $"Added {name} ({brand}) to the drink menu";
            }

            throw new ArgumentException();
        }

        public string AddFood(string type, string name, decimal price)
        {
            if (type == "Bread")
            {
                bakedFoods.Add(new Bread(name, price));
                return $"Added {name} ({type}) to the menu";
            }
            else if (type == "Cake")
            {
                bakedFoods.Add(new Cake(name, price));
                return $"Added {name} ({type}) to the menu";
            }

            throw new ArgumentException();
        }

        public string AddTable(string type, int tableNumber, int capacity)
        {
            if (type == "InsideTable")
            {
                tables.Add(new InsideTable(tableNumber, capacity));
                return $"Added table number {tableNumber} in the bakery";
            }
            else if (type == "OutsideTable")
            {
                tables.Add(new OutsideTable(tableNumber, capacity));
                return $"Added table number {tableNumber} in the bakery";
            }

            throw new ArgumentException();
        }

        public string GetFreeTablesInfo()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var table in tables)
            {
                if (table.IsReserved == false)
                {

                    sb.AppendLine(table.GetFreeTableInfo());
                }

            }
            return sb.ToString().TrimEnd();

        }

        public string GetTotalIncome()
        {

            return $"Total income: {totalIncome:f2}lv";
        }

        public string LeaveTable(int tableNumber)
        {
            var table = tables.FirstOrDefault(t => t.TableNumber == tableNumber);


            var check = table.GetBill();

            this.totalIncome += check;
            table.Clear();

            return $"Table: {tableNumber}{Environment.NewLine}Bill: {check:f2}";

        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)
        {

            ITable wantedTable = tables.FirstOrDefault(t => t.TableNumber == tableNumber);

            if (wantedTable == null)
            {
                return $"Could not find table with {tableNumber}";
            }

            IDrink wantedDrink = drinks.FirstOrDefault(d => d.Name == drinkName && d.Brand == drinkBrand);

            if (wantedDrink == null)
            {
                return $"There is no { drinkName } { drinkBrand} available";

            }
            else
            {
                wantedTable.OrderDrink(wantedDrink);
                return $"Table {tableNumber} ordered {drinkName} {drinkBrand}";

            }

        }

        public string OrderFood(int tableNumber, string foodName)
        {
            ITable wantedTable = tables.FirstOrDefault(t => t.TableNumber == tableNumber);

            if (wantedTable == null)
            {
                return $"Could not find table with {tableNumber}";
            }

            IBakedFood wantedFood = bakedFoods.FirstOrDefault(f => f.Name == foodName);

            if (wantedFood == null)
            {
                return $"No {foodName} in the menu";

            }
            else
            {
                wantedTable.OrderFood(wantedFood);
                return $"Table {tableNumber} ordered {foodName}";

            }

        }

        public string ReserveTable(int numberOfPeople)
        {
            ITable wantedTable = tables.FirstOrDefault(t => t.IsReserved == false && t.Capacity >= numberOfPeople);

            if (wantedTable == null)
            {
                return $"No available table for {numberOfPeople} people";
            }
            else
            {
                wantedTable.Reserve(numberOfPeople);
                return $"Table {wantedTable.TableNumber} has been reserved for {numberOfPeople} people";
            }
        }
    }
}
