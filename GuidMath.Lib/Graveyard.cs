using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This is a parking lot for dead code. I don't want to throw it away just yet, but I also don't want to see it.
//This file is set to Build Action None.
namespace GuidMath.Lib
{
    private class Graveyard
    {
        //This is no longer used, but I am going to keep it around just in case
        //This also didn't work, but trying to make it work would have been inefficient
        private void TrySubtractBySegment(Segment segment, BigInteger number)
        {
            if (segment == null) throw new InvalidSubtractionException();

            var diff = segment.Value + number;

            //If this Segment is not negative (or less than zero) then then it can be decremented.
            if (diff >= 0)
            {
                segment.Value = diff;

                return;
            }

            //If the segment is less than zero, then it is has to be reset to zero
            //after reducing the number by the current value and we attempt to decrement the 
            //next segment.
            segment.Value = 0; //Reset to zero

            TrySubtractBySegment(segment.Left, diff);
        }
    }
}
