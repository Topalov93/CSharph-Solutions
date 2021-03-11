namespace Bakery.Models.Drinks.Contracts
{
    public interface IDrink
    {
        string Name { get; }

        decimal Portion { get; }

        decimal Price { get; }

        string Brand { get; }
    }
}
