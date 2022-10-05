using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeacherComputerRetrievalUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace TeacherComputerRetrievalUI.Tests
{
    [TestClass()]
    public class TeacherComputerRetrievalTests
    {
        [TestMethod()]
        public void BuildMapFromStringTestCorrect()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap =  teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            Assert.IsFalse(errors.Any(), "There should be no error");
            Assert.IsTrue(academiesMap.Count == 5);
        }
        [TestMethod()]
        public void BuildMapFromStringTestSameRouteTwice()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DC6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            Assert.IsTrue(errors.Any(), "There should be no error");
        }
        [TestMethod()]
        public void BuildMapFromStringTestStartNodeSameAsEndNode()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,EE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            Assert.IsTrue(errors.Any(), "There should be no error");
        }
        [TestMethod]
        public void CalculateCorrectDistanceAToC()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            Assert.IsFalse(errors.Any(), "There should be no error");
            Assert.IsTrue(academiesMap.Count == 5);
            Assert.IsTrue(academiesMap.Single(x => x.Name == "B").Paths.Count == 1);
            Assert.IsTrue(academiesMap.Single(x => x.Name == "A").Paths.Count == 3);
            var minimumdistance = teacherComputerRetrieval.CalculateShortestDistance(
                academiesMap.Single(x => x.Name == "A"),
                academiesMap.Single(x => x.Name == "C"));
            Assert.IsTrue(minimumdistance == 9, "Minimum distance not as expected");
        }
        [TestMethod]
        public void CalculateCorrectDistanceAToD()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            Assert.IsFalse(errors.Any(), "There should be no error");
            Assert.IsTrue(academiesMap.Count == 5);
            Assert.IsTrue(academiesMap.Single(x => x.Name == "B").Paths.Count == 1);
            Assert.IsTrue(academiesMap.Single(x => x.Name == "A").Paths.Count == 3);
            var minimumdistance = teacherComputerRetrieval.CalculateShortestDistance(academiesMap.Single(x => x.Name == "A"),
                                                                             academiesMap.Single(x => x.Name == "D"));
            Assert.IsTrue(minimumdistance == 5, "Minimum distance not as expected");
        }
        [TestMethod]
        public void CalculateCorrectDistanceOfRouteAToBToC()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var minimumdistance = teacherComputerRetrieval.CalculateDistanceOfRoute(academiesMap.Single(x => x.Name == "A"),
                academiesMap.Single(x => x.Name == "B"), 
                academiesMap.Single(x => x.Name == "C"));
            Assert.IsTrue(minimumdistance == 9, "Calcuated distance not as expected");
        }
        [TestMethod]
        public void CalculateCorrectDistanceOfRouteAToEToBToCToD()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var minimumdistance = teacherComputerRetrieval.CalculateDistanceOfRoute(
                academiesMap.Single(x => x.Name == "A"),
                academiesMap.Single(x => x.Name == "E"),
                academiesMap.Single(x => x.Name == "B"),
                academiesMap.Single(x => x.Name == "C"),
                academiesMap.Single(x => x.Name == "D"));
            Assert.IsTrue(minimumdistance == 22, "Calcuated distance not as expected");
        }
        [TestMethod]
        public void CalculateCorrectDistanceOfRouteNonExisted()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            Assert.IsFalse(errors.Any(), "There should be no error");
            Assert.IsTrue(academiesMap.Count == 5);
            Assert.IsTrue(academiesMap.Single(x => x.Name == "B").Paths.Count == 1);
            Assert.IsTrue(academiesMap.Single(x => x.Name == "A").Paths.Count == 3);
            var haveException = false;
            try
            {
                var minimumdistance = teacherComputerRetrieval.CalculateDistanceOfRoute(
                    academiesMap.Single(x => x.Name == "A"),
                    academiesMap.Single(x => x.Name == "E"),
                    academiesMap.Single(x => x.Name == "D"));
            }
            catch
            {
                haveException = true;
            }
            Assert.IsTrue(haveException, "There should have been an exception");
        }
        [TestMethod]
        public void CalculateNumberOfTripsStartingInCAndEndingInCWithMAximum3Stops()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE5,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var alRoutes = teacherComputerRetrieval.GetAllRoutesBetween(
                academiesMap.Single(x => x.Name == "C"),
                academiesMap.Single(x => x.Name == "C"));
            Assert.IsTrue(alRoutes.Count(x => x.Count() <= 3) == 2, "Calculated number of trips as expected: 3");
        }
        [TestMethod]
        public void CalculateNumberOfTripsStartingInAAndEndingInCWithExactly4Stops()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var alRoutes = teacherComputerRetrieval.GetAllRoutesBetweenWithFixedDistance(
                academiesMap.Single(x => x.Name == "A"),
                academiesMap.Single(x => x.Name == "C"),
                4);
            Assert.IsTrue(alRoutes.Count(x => x.Count() == 4) == 3, "Calculated number of trips as expected: 3");
        }
        [TestMethod]
        public void CalculateShortestRouteStartingInAAndEndingInC()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var distance = teacherComputerRetrieval.CalculateShortestDistance(
                academiesMap.Single(x => x.Name == "A"),
                academiesMap.Single(x => x.Name == "C"));
            Assert.IsTrue(distance == 9, "Calculated shortes distance is not as expected: 9");
        }
        [TestMethod]
        public void CalculateShortestRouteStartingInBAndEndingInB()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var distance = teacherComputerRetrieval.CalculateShortestDistance(
                academiesMap.Single(x => x.Name == "B"),
                academiesMap.Single(x => x.Name == "B"));
            Assert.IsTrue(distance == 9, "Calculated shortes distance is not as expected: 9");
        }
        [TestMethod]
        public void CalculateShortestRouteStartingInCAndEndingInCWith30AsMaximumDistance()
        {
            var teacherComputerRetrieval = new TeacherComputerRetrieval(new PathsService());
            var input = "AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7";
            var academiesMap = teacherComputerRetrieval.BuildMapFromString(input, out List<string> errors);
            var routes = teacherComputerRetrieval.GetAllRoutesBetweenWithAMaximumDistance(
                academiesMap.Single(x => x.Name == "C"),
                academiesMap.Single(x => x.Name == "C"),
                30);
            Assert.IsTrue(routes.Count() == 9, "The number of routes are not as expected: 9");
        }
    }
}