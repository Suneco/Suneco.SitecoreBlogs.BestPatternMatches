namespace Suneco.SitecoreBlogs.BestPatternMatches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Cintel.Commons;
    using Sitecore.Cintel.Configuration;
    using Sitecore.Cintel.Reporting;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Suneco.SitecoreBlogs.BestPatternMatches.Models;

    public class ContactIntelligenceService : IContactIntelligenceService
    {
        public IEnumerable<BestPatternMatch> GetBestPatternMatches(Guid contactId, int pageNumber = 1, int pageSize = 3)
        {
            try
            {
                var result = new List<BestPatternMatch>();

                // The pipeline must run in the context of a content database (Not in the Core database)
                using (var db = new DatabaseSwitcher(Sitecore.Context.ContentDatabase))
                {
                    var parameters = ViewParameters.Default;

                    parameters.ContactId = contactId;
                    parameters.ViewName = "best-pattern-matches";
                    parameters.ViewEntityId = null;
                    parameters.PageNumber = pageNumber;
                    parameters.PageSize = pageSize;
                    parameters.SortFields = new List<SortCriterion>();
                    parameters.Filters = new List<Sitecore.Cintel.Commons.Filter>();
                    parameters.AdditionalParameters = new Dictionary<string, object>();

                    var pipeline = ViewFactory.GetPipeline("best-pattern-matches", WellknownIdentifiers.ContactViewPipelineGroupName);

                    if (pipeline == null)
                    {
                        Log.Error("Cannot resolve 'best-pattern-matches' pipeline.", this);
                        return null;
                    }

                    var args = new ReportProcessorArgs(parameters);
                    pipeline.Run(args);

                    if (!args.ResultSet.Data.Dataset.Any())
                    {
                        Log.Warn("Cannot get resultset from 'best-pattern-matches' pipeline.", this);
                        return null;
                    }

                    var dataset = args.ResultSet.Data.Dataset.FirstOrDefault().Value;

                    for (int i = 0; i < dataset.Rows.Count; i++)
                    {
                        var row = dataset.Rows[i];

                        var model = new BestPatternMatch();

                        model.ContactId = new Guid(System.Convert.ToString(row["ContactId"]));
                        model.LatestVisitId = new Guid(System.Convert.ToString(row["LatestVisitId"]));
                        model.ProfileId = new Guid(System.Convert.ToString(row["ProfileId"]));
                        model.ProfileDisplayName = System.Convert.ToString(row["ProfileDisplayName"]);
                        model.ProfileCount = System.Convert.ToInt32(row["ProfileCount"]);
                        model.ProfileCalculationType = System.Convert.ToString(row["ProfileCalculationType"]);
                        model.LatestVisitIndex = System.Convert.ToInt32(row["LatestVisitIndex"]);
                        model.PatternWasAppliedToVisit = System.Convert.ToBoolean(row["PatternWasAppliedToVisit"]);
                        model.BestMatchedPatternId = new Guid(System.Convert.ToString(row["BestMatchedPatternId"]));
                        model.BestMatchedPatternDisplayName = System.Convert.ToString(row["BestMatchedPatternDisplayName"]);
                        model.BestMatchedPatternGravityShare = System.Convert.ToInt32(row["BestMatchedPatternGravityShare"]);

                        result.Add(model);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Cannot resolve best pattern matches for contact: " + contactId.ToString(), ex, this);
            }

            return null;
        }
    }
}