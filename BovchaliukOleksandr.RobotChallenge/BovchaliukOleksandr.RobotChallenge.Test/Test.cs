using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;

namespace BovchaliukOleksandr.RobotChallenge.Test
{
    [TestClass]
    public class Test
    {
        private BovchaliukOleksandrAlgorithm algorithm;
        private Map map;
        private List<Robot.Common.Robot> robots;

        [TestInitialize]
        public void TestInitialize()
        {
            algorithm = new BovchaliukOleksandrAlgorithm();
            map = new Map();
            robots = new List<Robot.Common.Robot>();
        }
        [TestMethod]
        public void TestNearestFreeStation()
        {
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(10, 10), RecoveryRate = 2 });
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(100, 100), RecoveryRate = 2 });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            var pos = algorithm.GetNearestFreeStationPosition(robots[0], map, robots);
            Assert.AreNotSame(map.Stations[0].Position, pos);
        }
        [TestMethod]
        public void TestDistance()
        {
            var pos1 = new Position(12, 12);
            var pos2 = new Position(13, 15);
            Assert.AreEqual(10, DistanceHelper.FindDistanceEnergy(pos1, pos2));
        }
        [TestMethod]
        public void TestCollectEnergy()
        {
            Variant.Initialize(8);
            var stationPosition = new Position(1,1);
            map.Stations.Add(new EnergyStation() {Energy = 1000, Position = stationPosition, RecoveryRate = 2});
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            var command = algorithm.DoStep(robots, 0, map);
            Assert.IsTrue(command is CollectEnergyCommand);
        }
        [TestMethod]
        public void TestNearestPositionToCollect()
        {
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(5, 1), RecoveryRate = 2 });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });

            Assert.AreEqual(new Position(5, 1), BovchaliukOleksandrAlgorithm.GetPositionToCollectEnergy(map.Stations[0].Position, robots[0].Position, robots[0], map, robots));
        }
        [TestMethod]
        public void TestNumberOfMyRobots()
        {
            Owner owner = new Owner() { Name = "Oleksandr" };
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1), OwnerName = owner.ToString() });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1), OwnerName = owner.ToString() });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1), OwnerName = new Owner() { Name = "Else" }.ToString() });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1), OwnerName = owner.ToString() });

            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1), OwnerName = owner.ToString() });

            Assert.AreEqual(5, BovchaliukOleksandrAlgorithm.AmountMyRobot(robots, owner.ToString()));
        }
        [TestMethod]
        public void TestIsRobotCollectEnergy()
        {
            Position robot_position = new Position(1, 1);
            Position station_position = new Position(2, 2);

            Assert.IsTrue(Check.IsCollectEnergy(robot_position, station_position));
        }
        [TestMethod]
        public void TestIsStationMy()
        {
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(5, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            IList<Relation> relations = new List<Relation>();
            relations.Add(new Relation(3, 0));

            Assert.IsTrue(Check.IsStationMy(relations, 0));
        }
        [TestMethod]
        public void TestIsRelations()
        {

            var i = 3;
            var relationss = 5;
            var robotToMoveIndex = 3;
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(5, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            IList<Relation> relations = new List<Relation>();
            relations.Add(new Relation(robotToMoveIndex,i));
            Assert.AreEqual(robotToMoveIndex, Check.IsRelation(relations, robotToMoveIndex));
        }

        [TestMethod]
        public void TestIsFreeRelations()
        {
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(5, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            IList<Relation> relations = new List<Relation>();
            relations.Add(new Relation(3, 0));
            Assert.AreEqual(false, Check.IsFreeRelation(relations, 0));
        }

        [TestMethod]
        public void TestIsNotStationMy()
        {
            map.Stations.Add(new EnergyStation() { Energy = 1000, Position = new Position(5, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            robots.Add(new Robot.Common.Robot() { Energy = 200, Position = new Position(1, 1) });
            IList<Relation> relations = new List<Relation>();
            relations.Add(new Relation(3, 0));

            Assert.IsTrue(Check.IsStationMy(relations, 0));
        }
    }
}
