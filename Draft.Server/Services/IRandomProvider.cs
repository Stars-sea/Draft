namespace Draft.Server.Services;

public interface IRandomProvider {
    public int GetRandomNumber(int min, int max);
}
