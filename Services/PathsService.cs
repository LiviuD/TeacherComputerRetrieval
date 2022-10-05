using Common;
using System.Xml.Linq;
using Path = Common.Path;

namespace Services
{
    public class PathsService : IPathsService
    {
        public decimal GetShortestPath(Academy start, Academy end, ref IEnumerable<Academy> shortestPath)
        {
            if (start != end)
            {
                var allPaths = new List<List<Path>>();
                ResetVisited(start, ref allPaths, true);
                var map = CalculateShortestPaths(start, end);
                decimal shortestPathDistance = 0;
                var shortestPathList = shortestPath.ToList();
                BuildShortestPath(end, ref shortestPathList, ref shortestPathDistance);
                shortestPathList.Add(start);
                shortestPathList.Reverse();
                shortestPath = shortestPathList.AsEnumerable();
                return shortestPathDistance;
            }
            else
            {
                shortestPath = new List<Academy>();
                Func<Academy, Academy, List<List<Path>>, List<Path>, bool> predicate = (s, e, allPaths, currentPath) =>
                {
                    if (currentPath is object && currentPath.Any())
                    {
                        var totalDistance = currentPath.Sum(x => x.Distance);
                        if (s == e)
                        {
                            allPaths.Add(new List<Path>(currentPath));
                            return true;
                        }
                    }
                    return false;
                };
                var allPaths = GetAllPathsBetween(start, end, predicate, false);
                return allPaths.Select(x => x.Sum(_ => _.Distance)).OrderBy(x => x).First();
            }
        }

        public IEnumerable<IEnumerable<Path>> GetAllPathsBetween(Academy start, Academy end, Func<Academy, Academy, List<List<Path>>, List<Path>, bool> predicate, bool allowSamePath)
        {
            var allPaths = new List<List<Path>>();
            var currentPaths = new List<Path>();
            CalculateAllPaths(start, end, ref allPaths, ref currentPaths, predicate, allowSamePath);
            return allPaths.Where(x => x.Any());
        }

        public decimal GetDistanceOfRoute(params Academy[] academies)
        {
            decimal totalDistance = 0;
            for (int i = 0; i < academies.Count() - 1; i++)
            {
                var connectedAcademiaPath = academies[i].Paths?.SingleOrDefault(x => x.ConnectedAcademia == academies[i + 1]);
                if (connectedAcademiaPath is object)
                {
                    totalDistance += connectedAcademiaPath.Distance;
                }
                else
                {
                    throw new Exception("No SUCH ROUTE");
                }
            }
            return totalDistance;

        }
        private void CalculateAllPaths(Academy start, Academy end, ref List<List<Path>> allPaths, ref List<Path> currentPath, Func<Academy, Academy, List<List<Path>>, List<Path>, bool> predicate, bool allowSamePath = false)
        {
            if(predicate(start, end, allPaths, currentPath))
            {
                return;
            }
            if (start.Paths is null || !start.Paths.Any())
            {
                if (currentPath is object && allPaths.Contains(currentPath))
                {
                    allPaths.Remove(currentPath);
                }
                return;
            }
            foreach (var cnn in start.Paths)
            {
                var connAcademia = cnn.ConnectedAcademia;
                if (connAcademia is null
                    || (!allowSamePath
                        && currentPath.Contains(cnn)))
                {
                    continue;
                }
                currentPath.Add(cnn);
                var noOfPaths = allPaths.Count;
                CalculateAllPaths(connAcademia, end, ref allPaths, ref currentPath, predicate, allowSamePath);
                currentPath.RemoveAt(currentPath.LastIndexOf(cnn));
            }
        }

        private void ResetVisited(Academy start, ref List<List<Path>> allPaths, bool init)
        {
            var currentPath = allPaths.LastOrDefault();
            start.Visited = false;
            if (!(start.Paths?.Any() ?? false))
            {
                return;
            }
            foreach (var cnn in start.Paths)
            {
                if (init || currentPath is null)
                {
                    currentPath = new List<Path>();
                    allPaths.Add(currentPath);
                }
                else
                {
                    if (start.Paths.IndexOf(cnn) > 0)
                    {
                        currentPath = new List<Path>(allPaths.Last());
                        allPaths.Add(currentPath);
                    }
                }
                var connAcademia = cnn.ConnectedAcademia;
                if (connAcademia is null
                    || currentPath.Contains(cnn))
                {
                    continue;
                }
                currentPath.Add(cnn);
                ResetVisited(connAcademia, ref allPaths, false);
            }
        }

        private void BuildShortestPath(Academy academia, ref List<Academy> list, ref decimal shortestPathDistance)
        {
            if (academia.NearestToStart == null)
                return;
            list.Add(academia);
            shortestPathDistance += academia.NearestToStart.Paths.Single(x => x.ConnectedAcademia == academia).Distance;
            BuildShortestPath(academia.NearestToStart, ref list, ref shortestPathDistance);
        }

        private List<Academy> CalculateShortestPaths(Academy startNode, Academy endNode)
        {
            var result = new List<Academy>();
            startNode.MinDistanceToStart = 0;
            var queue = new List<Academy>();
            queue.Add(startNode);
            result.Add(startNode);
            do
            {
                queue = queue.OrderBy(x => x.MinDistanceToStart.Value).ToList();
                var node = queue.First();
                queue.Remove(node);
                foreach (var cnn in node.Paths.OrderBy(x => x.Distance))
                {
                    var connAcademia = cnn.ConnectedAcademia;
                    if (connAcademia.Visited)
                        continue;
                    if (!result.Contains(connAcademia))
                        result.Add(connAcademia);

                    if (connAcademia.MinDistanceToStart == null ||
                        node.MinDistanceToStart + cnn.Distance < connAcademia.MinDistanceToStart)
                    {
                        connAcademia.MinDistanceToStart = node.MinDistanceToStart + cnn.Distance;
                        connAcademia.NearestToStart = node;
                        if (!queue.Contains(connAcademia))
                            queue.Add(connAcademia);
                    }
                }
                node.Visited = true;
                if (node == endNode)
                    return result;
            } while (queue.Any());
            return result;
        }
    }
}