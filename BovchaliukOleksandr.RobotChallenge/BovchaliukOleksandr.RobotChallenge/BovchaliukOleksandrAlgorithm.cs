using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace BovchaliukOleksandr.RobotChallenge
{
    public class DistanceHelper
    {
        public static int FindDistanceEnergy(Position a, Position b)
        {
            return (int) (Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }

    public class Relation
    {
        private int robot_index = 0;
        private int station_index = 0;

        public Relation(int robot_index, int station_index)
        {
            this.robot_index = robot_index;
            this.station_index = station_index;
        }

        public int getRobotIndex()
        {
            return robot_index;
        }

        public int getStationIndex()
        {
            return station_index;
        }
    }
    public class BovchaliukOleksandrAlgorithm: IRobotAlgorithm
    {
        public string Author
        {
            get { return "Bovchaliuk Oleksandr"; }
        }
        public string Description
        {
            get { return "It's Oleksandr Bovchaliuk algorithm"; }
        }
        public BovchaliukOleksandrAlgorithm()
        {
            Logger.OnLogRound += Logger_OnLogRound;
        }
        private void Logger_OnLogRound(object sender, LogRoundEventArgs e)
        {
            RoundCount++;
        }
        public int RoundCount { get; set; }

        static IList<Relation> relations = new List<Relation>();

        public static int AmountMyRobot(IList<Robot.Common.Robot> robots, string OwnerName)
        {
            int n = 0;
            for (int i = 0; i < robots.Count; i++)
            {
                if (robots[i].OwnerName.ToString() == OwnerName.ToString())
                {
                    n++;
                }
            }

            return n;
        }
        public IList<EnergyStation> GetListOfBusyStations(Robot.Common.Robot movingRobot, Map map, IList<Robot.Common.Robot> robots)
        {
            IList<EnergyStation> list = new List<EnergyStation>();
            foreach (EnergyStation station in map.Stations)
            {
                if (Check.IsCollectEnergy(movingRobot.Position, station.Position))
                    list.Add(station);
            }
            return list;
        }

        public Position GetNearestFreeStationPosition(Robot.Common.Robot movingRobot, Map map,
            IList<Robot.Common.Robot> robots)
        {
            Position nearest = null;
            int minDistance = int.MaxValue;
            int i = 0;

            foreach (var station in map.Stations)
            {
                if (!Check.IsCollectEnergy(movingRobot.Position, station.Position) && !Check.IsStationMy(relations, i) &&
                    (Math.Abs(station.Position.X - movingRobot.Position.X) > 2 || Math.Abs(station.Position.Y - movingRobot.Position.Y) > 2))
                {
                    int d = DistanceHelper.FindDistanceEnergy(station.Position, movingRobot.Position);

                    if (d < minDistance && station.Energy > 0)
                    {
                        minDistance = d;
                        nearest = station.Position;
                    }

                }
                i++;
            }

            return nearest == null ? null : nearest;
        }
        public static Position GetNearestFreeStationPosition2(Robot.Common.Robot movingRobot, Map map,
            IList<Robot.Common.Robot> robots)
        {
            Position nearest = null;
            int minDistance = int.MaxValue;
            int i = 0;

            foreach (var station in map.Stations)
            {
                if (!Check.IsCollectEnergy(movingRobot.Position, station.Position) && !Check.IsStationMy(relations, i) &&
                    (Math.Abs(station.Position.X - movingRobot.Position.X) > 2 || Math.Abs(station.Position.Y - movingRobot.Position.Y) > 2))
                {
                    int d = DistanceHelper.FindDistanceEnergy(station.Position, movingRobot.Position);

                    if (d < minDistance && station.Energy > 0)
                    {
                        minDistance = d;
                        nearest = station.Position;
                    }

                }
                i++;
            }

            return nearest == null ? null : nearest;
        }
        public static Position GetPositionToCollectEnergy(Position station_position, Position robot_position, Robot.Common.Robot movingRobot, Map map,
            IList<Robot.Common.Robot> robots)
        {
            Position newPosition = new Position();
            var energy = 0;
            int variant = 0;
            if (robot_position.X == station_position.X)
            {
                variant = 1;
                energy = (int)Math.Pow((station_position.Y - robot_position.Y), 2);
            }
            if (robot_position.Y == station_position.Y)
            {
                variant = 2;
                energy = (int)Math.Pow((station_position.X - robot_position.X), 2);
            }

            if (!(robot_position.X == station_position.X) && !(robot_position.Y == station_position.Y))
            {
                variant = 3;
                energy = (int)Math.Pow((station_position.X - robot_position.X), 2) +
                         (int)Math.Pow((station_position.Y - robot_position.Y), 2);
            }

            var directionX = Math.Sign(station_position.X - robot_position.X);
            var directionY = Math.Sign(station_position.Y - robot_position.Y);

            if (variant == 1 && energy <= 100)
            {
                newPosition.X = station_position.X;
                newPosition.Y = station_position.Y;

            }
            if (variant == 2 && energy <= 100)
            {
                newPosition.Y = station_position.Y;
                newPosition.X = station_position.X;
            }
            if (variant == 3 && energy <= 100)
            {
                newPosition.X = station_position.X;
                newPosition.Y = station_position.Y;
            }
            if (variant == 3 && energy >= 101 && movingRobot.Energy <= energy)
            {
                newPosition.X = station_position.X;
                newPosition.Y = station_position.Y;
            }

            if (variant == 0)
            {
                Position pos = GetNearestFreeStationPosition2(movingRobot, map, robots);
                if (pos.X - robot_position.X > 0)
                {
                    newPosition.X = robot_position.X + 1;
                }
                else
                {
                    newPosition.X = robot_position.X - 1;
                }

                if (pos.Y - robot_position.Y > 0)
                {
                    newPosition.Y = robot_position.Y + 1;
                }
                else
                {
                    newPosition.Y = robot_position.Y - 1;
                }
            }

            return newPosition;
        }
        public Position GetPositionToCollectEnergy2(Position station_position, Position robot_position, Robot.Common.Robot movingRobot, Map map,
            IList<Robot.Common.Robot> robots)
        {
            Position newPosition = new Position();
            var energy = 0;
            int variant = 0;
            if (robot_position.X == station_position.X)
            {
                variant = 1;
                energy = (int)Math.Pow((station_position.Y - robot_position.Y), 2);
            }
            if (robot_position.Y == station_position.Y)
            {
                variant = 2;
                energy = (int)Math.Pow((station_position.X - robot_position.X), 2);
            }

            if (!(robot_position.X == station_position.X) && !(robot_position.Y == station_position.Y))
            {
                variant = 3;
                energy = (int)Math.Pow((station_position.X - robot_position.X), 2) +
                         (int)Math.Pow((station_position.Y - robot_position.Y), 2);
            }

            var directionX = Math.Sign(station_position.X - robot_position.X);
            var directionY = Math.Sign(station_position.Y - robot_position.Y);

            if (variant == 1 && energy <= 100)
            {
                newPosition.X = station_position.X;
                newPosition.Y = station_position.Y;

            }
            if (variant == 2 && energy <= 100)
            {
                newPosition.Y = station_position.Y;
                newPosition.X = station_position.X;
            }
            if (variant == 3 && energy <= 100)
            {
                newPosition.X = station_position.X;
                newPosition.Y = station_position.Y;
            }
            if (variant == 3 && energy >= 101 && movingRobot.Energy <= energy)
            {
                newPosition.X = station_position.X;
                newPosition.Y = station_position.Y;
            }

            if (variant == 0)
            {
                Position pos = GetNearestFreeStationPosition2(movingRobot, map, robots);
                if (pos.X - robot_position.X > 0)
                {
                    newPosition.X = robot_position.X + 1;
                }
                else
                {
                    newPosition.X = robot_position.X - 1;
                }

                if (pos.Y - robot_position.Y > 0)
                {
                    newPosition.Y = robot_position.Y + 1;
                }
                else
                {
                    newPosition.Y = robot_position.Y - 1;
                }
            }

            return newPosition;
        }

        public static bool IsMyRobotWithMyRobot(Position station_position, Position robot_position,
            Robot.Common.Robot movingRobot, Map map, IList<Robot.Common.Robot> robots)
        {
            var directionRobotsAndStationX = station_position.X - robot_position.X;
            var directionRoborsAndStationY = station_position.Y - robot_position.Y;

            if (directionRobotsAndStationX <= 3 && directionRoborsAndStationY <= 3)
            {
                return true;
            }

            return false;
        }
        
        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot movingRobot = robots[robotToMoveIndex];
            Position pos = GetNearestFreeStationPosition(movingRobot, map, robots);
            var energys = 0;
            if (pos != null)
            {
                energys = (int)Math.Pow((pos.X - movingRobot.Position.X), 2) +
                          (int)Math.Pow((pos.Y - movingRobot.Position.Y), 2);
            }

            if (movingRobot.Energy > 420 && energys > 1 && RoundCount < 41 && AmountMyRobot(robots, movingRobot.OwnerName) < 45)
            {
                return new CreateNewRobotCommand();
            }
            
            int j = 0;
            
            foreach (var v in map.Stations)
            {
                if (pos != null && (v.Position.X == pos.X && v.Position.Y == pos.Y)) break;
                j++;
            }

            int numb_station = Check.IsRelation(relations, robotToMoveIndex);

            IList<EnergyStation> list = GetListOfBusyStations(robots[robotToMoveIndex], map, robots);
            int i = 0;
            foreach (var energyStation in list)
            {
                if (energyStation.Energy > 0)
                {
                    if (Check.IsFreeRelation(relations, i))
                        relations.Add(new Relation(robotToMoveIndex, i));
                    return new CollectEnergyCommand();
                }
                i++;
            }

            Position newPos = new Position();
            newPos = GetPositionToCollectEnergy(pos, movingRobot.Position, movingRobot, map, robots);

            return new MoveCommand() { NewPosition = newPos };

        }
    }
}
