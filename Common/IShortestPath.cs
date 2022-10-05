namespace Common
{
    public interface IPathsService
    {
        decimal GetShortestPath(Academy start, Academy end, ref IEnumerable<Academy> shortestPath);
        IEnumerable<IEnumerable<Path>> GetAllPathsBetween(Academy start, Academy end, Func<Academy, Academy, List<List<Path>>, List<Path>, bool> predicate, bool allowSamePath = false);
        decimal GetDistanceOfRoute(params Academy[] academies);

    }
}