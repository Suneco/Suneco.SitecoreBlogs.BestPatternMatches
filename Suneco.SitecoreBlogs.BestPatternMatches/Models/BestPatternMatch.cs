namespace Suneco.SitecoreBlogs.BestPatternMatches.Models
{
    using System;

    public class BestPatternMatch
    {
        public Guid ContactId { get; set; }

        public Guid LatestVisitId { get; set; }

        public Guid ProfileId { get; set; }

        public string ProfileDisplayName { get; set; }

        public int ProfileCount { get; set; }

        public string ProfileCalculationType { get; set; }

        public int LatestVisitIndex { get; set; }

        public DateTime LatestVisitStartDateTime { get; set; }

        public int LatestVisitEndDateTime { get; set; }

        public bool PatternWasAppliedToVisit { get; set; }

        public Guid BestMatchedPatternId { get; set; }

        public string BestMatchedPatternDisplayName { get; set; }

        public int BestMatchedPatternGravityShare { get; set; }

    }
}