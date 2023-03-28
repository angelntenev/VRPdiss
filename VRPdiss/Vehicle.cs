using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPdiss
{
    internal class Vehicle
    {
        private string type;
        private double speed;
        private double fuel = 100;
        private double distanceTraveled = 0;

        public string getType() { return type; }
        public double getSpeed() { return speed; }
        public double getDistanceTraveled() { return distanceTraveled; }
        public double getFuel() { return fuel; }
        public void setType(string type)
        {
            this.type = type;
        }
        public void setSpeed(double speed)
        {
            this.speed = speed;
        }
        public void setDistanceTraveled(double distanceTraveled)
        {
            this.distanceTraveled = distanceTraveled;
        }
        public void setFuel(double fuel)
        {
            this.fuel = fuel;
        }




        public void resetDistance()
        {
            distanceTraveled = 0;
        }
        public void addDistance(double distance)
        {
            distanceTraveled += distance;
        }
        public void resetFuel()
        {
            fuel = 100;
        }
        public void drainFuel(double fuelUsed)
        {
            fuel -= fuelUsed;
        }
    }
}
