namespace Draft.Server.Services.Impl;

public class RandomProvider : IRandomProvider {
    private readonly Random _random = new();

    public int GetRandomNumber(int min, int max) => _random.Next(min, max);
}
