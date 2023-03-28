using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBiddingExchange
{
    internal class AgentDetailsForTrade
    {
        private int agentID;
        private double agentDistance;
        private string agentName;
        private Point furthestPoint;


        

        public AgentDetailsForTrade(int agentID, double agentDistance, string agentName, Point furthestPoint)
        {
            this.agentID = agentID;
            this.agentDistance = agentDistance;
            this.agentName = agentName;
            this.furthestPoint = furthestPoint;
        }

        public double getAgentDistance()
        {
            return agentDistance;
        }
        
        public int getAgentID()
        {
            return agentID;
        }
        
        public string getAgentName()
        {
            return agentName;
        }

        public Point getFurthestPoint()
        {
            return furthestPoint;
        }
    }
}
