using ToolBox.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Model
{
    public class CompMatrixModel
    {
        [ExtendedDescription("Incentive Plan")]
        public string IncentivePlan { get; set; }

        [ExtendedDescription("Country")]
        public string Country { get; set; }

        [ExtendedDescription("FY17 RBI Target", "FY17 RBI  Target", "RBI Target", "RBI  Target", "RBI")]
        public string RevenueBasedIncentive { get; set; }

        [ExtendedDescription("FY17 CBI Target", "FY17 CBI  Target", "CBI Target", "CBI  Target", "CBI")]
        public string CommisionBasedIncentive { get; set; }

        [ExtendedDescription("Transition Allowance Eligible", "TAE")]
        public string TransitionAllowance { get; set; }

        [ExtendedDescription("Salary Structure**", "Pay Scale Type")]
        public string PayScaleType { get; set; }

        [ExtendedDescription("Level Band")]
        public string LevelBand { get; set; }
    }
}
