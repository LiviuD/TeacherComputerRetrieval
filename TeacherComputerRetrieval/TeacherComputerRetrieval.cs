using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path = Common.Path;

namespace TeacherComputerRetrievalUI
{
    public class TeacherComputerRetrieval
    {
        private readonly IPathsService pathsService;
        public TeacherComputerRetrieval(IPathsService pathService)
        {
            pathsService = pathService;
        }
        public List<Academy> BuildMapFromString(string routes, out List<string> errors)
        {
            var academies = new List<Academy>();
            errors = new List<string>();
            if(routes is null || !routes.Split(',', ';').Any())
            {
                errors.Add("Please add some routes");
                return academies;
            }
            
            foreach (var r in routes.Split(',', ';'))
            {
                if (r.Length < 3)
                {
                    errors.Add($"The route {r} is not correct format.");
                    continue;
                }
                var r1 = r.Substring(0, 1);
                var r2 = r.Substring(1, 1);
                if (!decimal.TryParse(r.Substring(2), out decimal distance))
                {
                    errors.Add($"The distance for the route {r} must be a number");
                    continue;
                }
                if (string.Compare(r1, r2, true) == 0)
                {
                    errors.Add($"The starting and ending academy must not be the same academy for {r}");
                    continue;
                }
                var academyStart = academies.SingleOrDefault(x => String.Compare(x.Name, r1, true) == 0);
                if (academyStart is null)
                {
                    academyStart = new Academy() { Name = r1, MinDistanceToStart = null, NearestToStart = null, Paths = new List<Path>(), Visited = false };
                    academies.Add(academyStart);
                }
                var academyDest = academies.SingleOrDefault(x => String.Compare(x.Name, r2, true) == 0);
                if (academyDest is null)
                {
                    academyDest = new Academy() { Name = r2, MinDistanceToStart = null, NearestToStart = null, Paths = new List<Path>(), Visited = false };
                    academies.Add(academyDest);
                }
                var path = new Path() { ConnectedAcademia = academyDest, Distance = distance };
                if (academyStart.Paths.Any(x => x.ConnectedAcademia == academyDest))
                {
                    errors.Add($"The there is already a route defined for {r}");
                    continue;
                }
                academyStart.Paths.Add(path);
            }
            return academies;
        }
        public decimal CalculateShortestDistance(Academy start, Academy end)
        {
            var shortestPath = new List<Academy>().AsEnumerable();
            var result = pathsService.GetShortestPath(start, end, ref shortestPath);
            return result;
        }
        public decimal CalculateDistanceOfRoute(params Academy[] academies)
        {
            return pathsService.GetDistanceOfRoute(academies);
        }

        public IEnumerable<IEnumerable<Path>> GetAllRoutesBetween(Academy academyStart, Academy academyEnd)
        {
            Func<Academy, Academy, List<List<Path>>, List<Path>, bool> p = (start, end, allPaths, currentPath) =>
            {
                if (currentPath is object && currentPath.Any())
                {
                    var totalDistance = currentPath.Sum(x => x.Distance);
                    if (start == end)
                    {
                        allPaths.Add(new List<Path>(currentPath));
                        return true;
                    }
                }
                return false;
            };

            return pathsService.GetAllPathsBetween(academyStart, academyEnd, p, allowSamePath: false);
        }
        public IEnumerable<IEnumerable<Path>> GetAllRoutesBetweenWithAMaximumDistance(Academy academyStart, Academy academyEnd, decimal maximumDistance)
        {
            Func<Academy, Academy, List<List<Path>>, List<Path>, bool> p = (start, end, allPaths, currentPath) =>
            {
                if (currentPath is object && currentPath.Any())
                {
                    var totalDistance = currentPath.Sum(x => x.Distance);
                    if(maximumDistance > 0 && totalDistance > maximumDistance)
                    {
                        return true;
                    }
                    if (start == end)
                    {
                        if (maximumDistance <= 0 || totalDistance <= maximumDistance)
                        {
                            allPaths.Add(new List<Path>(currentPath));
                        }
                        if (maximumDistance <= 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            };

            return pathsService.GetAllPathsBetween(academyStart, academyEnd, p, true);  
        }

        public IEnumerable<IEnumerable<Path>> GetAllRoutesBetweenWithFixedDistance(Academy academyStart, Academy academyEnd, decimal noOfStops)
        {
            Func<Academy, Academy, List<List<Path>>, List<Path>, bool> p = (start, end, allPaths, currentPath) =>
            {
                if (currentPath is object && currentPath.Any())
                {
                    if (noOfStops <= 0)
                        return true;
                    var numberOfNodes = currentPath.Count();
                    if (numberOfNodes > noOfStops)
                    {
                        return true;
                    }
                    if (start == end)
                    {
                        if (numberOfNodes == noOfStops)
                        {
                            allPaths.Add(new List<Path>(currentPath));
                        }
                    }
                }
                return false;
            };
            return pathsService.GetAllPathsBetween(academyStart, academyEnd, p, true);
        }
    }
}
