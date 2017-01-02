using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Tower_Of_Hanoi
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            System.Drawing.Graphics graphics = this.CreateGraphics();


            InitializeComponent();

        }

        public int milliseconds = 550;              //control the speed of the simulation
        bool done = false;                          //checke wheither to simulate or to stop
        int simulation_key = 0;                     //helps to checke wheither to simulate or to stop
        int stage = 0;                              //store which state the simulation was sttoped at to resue from it and it set to 0 every new simulation with new rod numbers start
        bool flag = false;                          //indecate if a new simualtion with new rod numbers starts
        int rod_num = 0;
        public List<Rectangle> list_rectangle = new List<Rectangle>();//list of drawn blocks

        //draw the intial design of the 3 towers
        private void IntialDesign()
        {
            System.Drawing.Graphics graphics = this.CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.CadetBlue);
            graphics.Clear(Color.Beige);
            //make a rectangle object
            System.Drawing.Rectangle rectangle1 = new System.Drawing.Rectangle(250, 100, 10, 300);
            //draw the outline of the rectangle
            graphics.DrawRectangle(System.Drawing.Pens.CornflowerBlue, rectangle1);
            //fill that outline
            graphics.FillRectangle(brush, rectangle1);

            System.Drawing.Rectangle rectangle2 = new System.Drawing.Rectangle(450, 100, 10, 300);
            graphics.FillRectangle(brush, rectangle2);
            graphics.DrawRectangle(System.Drawing.Pens.CornflowerBlue, rectangle2);

            System.Drawing.Rectangle rectangle3 = new System.Drawing.Rectangle(650, 100, 10, 300);
            graphics.FillRectangle(brush, rectangle3);
            graphics.DrawRectangle(System.Drawing.Pens.CornflowerBlue, rectangle3);
        }

        //draw the n rectangles and fill a list with their position and dimntions
        private void Adddiscs()
        {
            List<Rectangle> tmp_list_rectangle = new List<Rectangle>();
            tmp_list_rectangle.Clear();
            list_rectangle.Clear();
            int y = 380;
            int width = 150;
            int x = 185;

            for (int i = 0; i < (int.Parse(textBox1.Text)); i++)
            {

                System.Drawing.Graphics graphics = this.CreateGraphics();

                System.Drawing.Rectangle rectangle1 = new System.Drawing.Rectangle(x, y, width, 10);
                graphics.DrawRectangle(System.Drawing.Pens.MediumSlateBlue, rectangle1);
                SolidBrush brush = new SolidBrush(Color.MediumOrchid);
                graphics.FillRectangle(brush, rectangle1);
                tmp_list_rectangle.Add(rectangle1);
                y -= 10;
                width -= 20;
                x += 10;
            }

            //invers the temp list to simulate correctly
            for (int i =  tmp_list_rectangle.Count(); i >0; i--)
            {
                list_rectangle.Add(tmp_list_rectangle[i-1]);
            }
        }

        //simulate the tower of hani animation
        private void simulate(List<Rectangle> n_rectangle, List<Step> steps)
        {
            int[] coloumns = { n_rectangle.Count(), 0, 0 };
            System.Drawing.Rectangle rect_tmp = new System.Drawing.Rectangle(150, 100, 20, 10);
            for (int i = stage; i < steps.Count(); i++)
            {
                if (done == false)
                {
                    coloumns[steps[i].src - 1]--;
                    rect_tmp = n_rectangle[steps[i].rod_num - 1];
                    rect_tmp.Y = 380 - coloumns[steps[i].dest - 1] * 10;
                    rect_tmp.X = 185 + (steps[i].dest - 1) * 200 + (n_rectangle.Count() - steps[i].rod_num - 1) * 10;
                    coloumns[steps[i].dest - 1]++;
                    n_rectangle[steps[i].rod_num - 1] = rect_tmp;
                    list_drawing(n_rectangle);
                    label2.Text = "Step number: " + (i + 1);
                    Thread.Sleep(milliseconds);
                    stage = i;
                }
            }
        }

        //draw a given list of rectangles
        private void list_drawing(List<Rectangle> n_rectangle)
        {

            System.Drawing.Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.Beige);
            IntialDesign();
            SolidBrush brush = new SolidBrush(Color.MediumOrchid);
            System.Drawing.Rectangle rectangle1 = new System.Drawing.Rectangle(400, 100, 10, 300);
            for (int i = 0; i < n_rectangle.Count(); i++)
            {
                rectangle1 = n_rectangle[i];
                graphics.DrawRectangle(System.Drawing.Pens.MediumSlateBlue, rectangle1);
                graphics.FillRectangle(brush, rectangle1);
            }

        }

        //Button 1 action
        private void button1_Click(object sender, EventArgs e)
        {
            if (flag == false)
            {
                IntialDesign();         //draw the intial design
                rod_num = int.Parse(textBox1.Text);
                Adddiscs();            //draw the n rectangle and adding them in the list
                flag = true;
            }
            if(rod_num!=int.Parse(textBox1.Text))
            {
                IntialDesign(); 
                 rod_num = int.Parse(textBox1.Text);
                Adddiscs();
                stage = 0;
            }
                SolveTowerOfHanoi solution = new SolveTowerOfHanoi(int.Parse(textBox1.Text));
                int src = 1;
                int dest = 3;
                int aux = 2;
                List<Step> steps = new List<Step>();
                solution.Tower(int.Parse(textBox1.Text), src, dest, aux, steps);
            Thread simulation = new Thread(() =>
            {
                simulate(list_rectangle, steps);
            });

            simulation_key++;

            if (simulation_key > 0)
            {
                simulation_key = -1;
                done = false;
                button1.Text = "Stop";
                simulation.Start();
            }
            else
            {
                done = true;
                button1.Text = "Simulate";
                //simulation.Suspend();
            }
            
        }

        //form intialization
        private void Form1_Load(object sender, EventArgs e)
        {
            IntialDesign();
            milliseconds = 550;
            milliseconds = milliseconds - (trackBar1.Value * (milliseconds/10));
        }

        //exit the program
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //trackbar value 
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            milliseconds = 550;
            milliseconds = milliseconds - (trackBar1.Value * (milliseconds / 10));
        }
    }
}
