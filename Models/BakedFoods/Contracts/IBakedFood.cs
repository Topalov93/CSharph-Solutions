namespace Bakery.Models.BakedFoods.Contracts
{
    public interface IBakedFood
    {
        string Name { get; }

        decimal Portion { get; }

        decimal Price { get; }
    }
}
