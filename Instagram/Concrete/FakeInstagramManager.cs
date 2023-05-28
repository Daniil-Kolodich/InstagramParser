namespace Instagram.Concrete;

public class FakeInstagramManager : IInstagramManager
{
    public async Task<IEnumerable<string[]>> GetFollowers(string instagramId, CancellationToken cancellationToken)
    {
        var result = CreateData(instagramId, 42);
        return result.Select(x => x.Select(y => $"{nameof(GetFollowers)}_{y}").ToArray());
    }

    public async Task<IEnumerable<string[]>> GetFollowings(string instagramId, CancellationToken cancellationToken)
    {
        var result = CreateData(instagramId, 69);
        return result.Select(x => x.Select(y => $"{nameof(GetFollowings)}_{y}").ToArray());
    }

    private IEnumerable<string[]> CreateData(string id, int randomSeed)
    {
        var random = new Random(randomSeed);
        int batchesCount = random.Next(2, 5);
        
        for (int i = 0; i < batchesCount; i++)
        {
            yield return new[]
            {
                $"{id}_{i}_0",
                $"{id}_{i}_1",
                $"{id}_{i}_2",
                $"{id}_{i}_3",
                $"{id}_{i}_4",
                $"{id}_{i}_5",
            };
        }
    }
}