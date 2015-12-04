namespace Suneco.SitecoreBlogs.BestPatternMatches.Services
{
    using System;
    using System.Collections.Generic;
    using Suneco.SitecoreBlogs.BestPatternMatches.Models;

    public interface IContactIntelligenceService
   {
        IEnumerable<BestPatternMatch> GetBestPatternMatches(Guid contactId, int pageNumber = 1, int pageSize = 3);
    }
}
