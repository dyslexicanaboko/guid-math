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

        //Since I found an alternative that works for subtraction, then addition can do the same thing.
        //No need to painfully increment segments one at a time.
        private void TryAdd(Segment segment, BigInteger number)
        {
            if (segment == null) throw new InvalidAdditionException();

            var sum = segment.Value + number;

            //If this Segment is not larger than max then it can be incremented.
            if (segment.Max > sum)
            {
                segment.Value = sum;

                return;
            }

            //If the segment is larger than or equal to max, then it is has to be reset to zero
            //after reducing the number by the current value and we attempt to increment the 
            //next segment.
            var remainder = sum - segment.Max; //Calculate the remainder

            //Console.WriteLine(remainder);

            segment.Value = 0; //Reset to zero

            TryAdd(segment.Left, remainder);
        }
    }
}
