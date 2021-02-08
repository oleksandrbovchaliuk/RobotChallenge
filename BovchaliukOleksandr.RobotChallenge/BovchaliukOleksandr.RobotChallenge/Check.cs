using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace BovchaliukOleksandr.RobotChallenge
{
    public class Check
    {
        public static bool IsStationMy(IList<Relation> relations, int station_index)
        {
            foreach (var rel in relations)
            {
                if (rel.getStationIndex() == station_index)
                {
                    return true;
                }
            }

            return false;
        }

        public static int IsRelation(IList<Relation> relations, int robot_index)
        {
            foreach (var rel in relations)
            {
                if (rel.getRobotIndex() == robot_index)
                {
                    return rel.getStationIndex();
                }
            }

            return -1;
        }

        public static bool IsFreeRelation(IList<Relation> relations, int station_index)
        {
            foreach (var rel in relations)
            {
                if (rel.getStationIndex() == station_index)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsCollectEnergy(Position robot_pos, Position station_pos)
        {
            if ((Math.Abs(robot_pos.X - station_pos.X) <= 2) && (Math.Abs(robot_pos.Y - station_pos.Y) <= 2))
            {
                return true;
            }

            return false;
        }

    }
}
