using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2PROF
{
    //<sVincent>
    public class Score
    {
        private static Score instance = null;
        private int pills;
        private int superPills;
        private int ghostEaten;

        private Score()
        {
            pills = 0;
            superPills = 0;
        }

        public Score GetInstance()
        {
            if (instance == null)
            {
                instance = new Score();
                return instance;
            }
            else
            {
                return instance;
            }

        }

        public int Pills
        {
            get { return pills; }
            set { pills = value; }
        }

        public int SuperPills
        {
            get {return superPills;}
            set { superPills = value; }

        }

    }
    //</sVincent>
}
