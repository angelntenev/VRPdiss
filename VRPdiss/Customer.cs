using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPdiss
{
    internal class Customer
    {
        // Demand of the customer
        private int Demand { get; set; }

        // Location of the customer
        private Point Location { get; set; }

        // Constructor to initialize the Demand and Location properties
        public Customer()
        {
            Location = generateLocation(100,100);
        }

        public Customer(int x, int y)
        {
            Location = new Point(x, y);
        }

        public int getDemand() { return Demand; }
        public void setDemand(int demand) { Demand = demand; }

        public Point getLocation() { return Location; }
        public void setLocation(Point location) { Location = location; }
        private Point generateLocation(int maxX, int maxY)
        {
            Random rnd = new Random();
            return new Point(rnd.Next(-100, maxX), rnd.Next(-100, maxY));
        }
        

    }

}
