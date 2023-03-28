using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml;

namespace DisplayNodes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Load the XML file into an XmlDocument object
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Li_24.xml");

            XmlNodeList nodes = xmlDoc.SelectNodes("//network/nodes/node");

            // Calculate the coordinates of the root node circle
            int centerX = pictureBox1.ClientRectangle.Width / 2;
            int centerY = pictureBox1.ClientRectangle.Height / 2;

            List<Point> points = new List<Point>();
            List<Point> pointsToWrite = new List<Point>();

            foreach (XmlNode node in nodes)
            {
                double cx = double.Parse(node.SelectSingleNode("cx").InnerText);
                double cy = double.Parse(node.SelectSingleNode("cy").InnerText);


                drawPoint(centerX + (int)cx, centerY + (int)cy);

                points.Add(new Point(centerX + (int)cx, centerY + (int)cy));

            }


            //generate set number of routes with random points************************************************
            for (int i = 0; i < 300; i++)
            {
                Random rand = new Random();
                int maxPoints = rand.Next(10, 40);

                for (int j = 0; j < maxPoints; j++)
                {
                    int index = rand.Next(0, points.Count);
                    pointsToWrite.Add(points[index]);
                }

                using (StreamWriter sw = new StreamWriter("C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\GeneratedRoute1.txt", true))
                {
                    for (int k = 0; k < pointsToWrite.Count; k++)
                    {
                        sw.WriteLine(pointsToWrite[k].X + "," + pointsToWrite[k].Y);
                    }
                    sw.WriteLine("NEXT");
                }
                pointsToWrite.Clear();

            }
            ////721

        }

        public void drawPoint(int x, int y)
        {
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            SolidBrush brush = new SolidBrush(Color.Black);
            Point dPoint = new Point(x, (pictureBox1.Height - y));
            dPoint.X = dPoint.X - 2;
            dPoint.Y = dPoint.Y - 2;
            Rectangle rect = new Rectangle(dPoint, new Size(4, 4));
            g.FillRectangle(brush, rect);
            g.Dispose();
        }

        public void drawPointWithIdentifier(int x, int y, int index)
        {
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            SolidBrush brush = new SolidBrush(Color.Black);
            Point dPoint = new Point(x, (pictureBox1.Height - y));
            dPoint.X = dPoint.X - 2;
            dPoint.Y = dPoint.Y - 2;
            Rectangle rect = new Rectangle(dPoint, new Size(15, 15));
            g.FillRectangle(brush, rect);

            // Draw the number above the point
            Font font = new Font("Arial", 8);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            g.DrawString(index.ToString() + ".", font, textBrush, dPoint.X - 10, dPoint.Y - 20, stringFormat);

            g.Dispose();
        }

        public void drawLine(Point startPoint, Point endPoint, int number)
        {
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            Pen pen = new Pen(Color.Black);
            g.DrawLine(pen, startPoint, endPoint);

            // Calculate the midpoint of the line
            Point midpoint = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);

            // Draw the number next to the midpoint
            Font font = new Font("Arial", 10);
            SolidBrush brush = new SolidBrush(Color.Black);
            g.DrawString(number.ToString(), font, brush, midpoint);

            g.Dispose();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\GeneratedRoute1.txt";

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int currentIndex = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals("NEXT"))
                    {
                        break;
                    }
                    else
                    {
                        string[] coordinates = line.Split(',');
                        int x = int.Parse(coordinates[0]);
                        int y = int.Parse(coordinates[1]);
                        drawPointWithIdentifier(x, y, currentIndex);
                    }
                    currentIndex++;
                }
            }

            filePath = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Solution to Routes\\Route1Solution.txt";

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int currentIndex = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    Point startPoint, endPoint;
                    string[] coordinates = line.Split(',');
                    int x = int.Parse(coordinates[0]);
                    int y = int.Parse(coordinates[1]);
                    startPoint = new Point(x,y);
                    if ((line = sr.ReadLine()) != null)
                    {
                        coordinates = line.Split(',');
                        x = int.Parse(coordinates[0]);
                        y = int.Parse(coordinates[1]);
                        endPoint = new Point(x, y);
                        drawLine(startPoint, endPoint, currentIndex);
                    }
                    currentIndex++;   
                }
                
            }
        }
    }
}