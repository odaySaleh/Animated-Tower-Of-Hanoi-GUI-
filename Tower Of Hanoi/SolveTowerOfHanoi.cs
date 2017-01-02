using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tower_Of_Hanoi
{
    class SolveTowerOfHanoi
    {
        int numberOfRods;
        Step step;

        //set the rod numbers
        public SolveTowerOfHanoi(int NumberOfRods)
        {
            numberOfRods = NumberOfRods;
        }

        //solve teh problem and fill the gude list with the steps 
        public void Tower(int numberOfRods, int src, int dest, int aux, List<Step> steps)
        {
            if (numberOfRods == 1)
            {
                step.rod_num = numberOfRods;
                //step.rod_num += 1;
                step.dest = dest;
                step.src = src;
                steps.Add(step);
            }

            else
            {
                Tower(numberOfRods - 1, src, aux, dest, steps);
                step.rod_num = numberOfRods;
                //step.rod_num += 1;
                step.dest = dest;
                step.src = src;
                steps.Add(step);
                Tower(numberOfRods - 1, aux, dest, src, steps);
            }
        }
    }
}