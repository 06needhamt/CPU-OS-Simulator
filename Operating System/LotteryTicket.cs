using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This class represents a Lottery ticket used for the Lottery Scheduling Mode
    /// </summary>
    [Serializable]
    public class LotteryTicket
    {
        private int id;
        private SimulatorProcess owner;
        private int currencyValue;
        private bool selected;

        /// <summary>
        /// Default Constructor used when deserialising lottery tickets
        /// NOTE: DO NOT USE IN CODE
        /// </summary>
        public LotteryTicket()
        {
            
        }
        /// <summary>
        /// Constructor for lottery ticket
        /// </summary>
        /// <param name="id"> the unique identifier of the ticket</param>
        /// <param name="owner"> the process that owns this ticket</param>
        /// <param name="currencyValue"> the currency value of this ticket (how likely it is to get chosen) </param>
        public LotteryTicket(int id, SimulatorProcess owner, int currencyValue)
        {
            this.id = id;
            this.owner = owner;
            this.currencyValue = currencyValue;
            this.selected = false;
        }
        /// <summary>
        /// Property for the ID of this ticked
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Property for the process that owns this ticket
        /// </summary>
        public SimulatorProcess Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        /// <summary>
        /// Property for the currency value of this ticket (how likely it is to get chosen)
        /// </summary>
        public int CurrencyValue
        {
            get { return currencyValue; }
            set { currencyValue = value; }
        }
        /// <summary>
        /// Property value for whether this ticked has been chosen before
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

    }
}
