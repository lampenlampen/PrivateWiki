using Parser.Enums;

namespace Parser
{
    public class InlineTripChar
    {
        /// <summary>
        /// A helper class for the trip chars. This is an optimization. If we ask each class to go
        /// through the rage and look for itself we end up looping through the range n times, once
        /// for each inline. This class represent a character that an inline needs to have a
        /// possible match. We will go through the range once and look for everyone's trip chars,
        /// and if they can make a match from the trip char then we will commit to them.
        /// </summary>
        public char FirstChar { get; set; }
        
        public InlineParseMethod Method { get; set; }
    }
}