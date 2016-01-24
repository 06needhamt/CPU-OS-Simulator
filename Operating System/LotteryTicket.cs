using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Operating_System
{
    [Serializable]
    public class LotteryTicket
    {
        private int id;
        private SimulatorProcess owner;
        private int currencyValue;
        private bool selected;

        public LotteryTicket()
        {
            
        }

        public LotteryTicket(int id, SimulatorProcess owner, int currencyValue)
        {
            this.id = id;
            this.owner = owner;
            this.currencyValue = currencyValue;
            this.selected = false;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public SimulatorProcess Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public int CurrencyValue
        {
            get { return currencyValue; }
            set { currencyValue = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

    }
}
