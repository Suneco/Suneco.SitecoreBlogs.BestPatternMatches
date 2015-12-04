namespace Suneco.SitecoreBlogs.BestPatternMatches.ContentSearch
{
    using System;
    using System.Linq;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Analytics.Models;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data;
    using Suneco.SitecoreBlogs.BestPatternMatches.Services;

    public class BestPatternMatchesComputedField : IComputedIndexField
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        public string ReturnType { get; set; }

        /// <summary>
        /// Computes the field value.
        /// </summary>
        /// <param name="indexable">The indexable.</param>
        /// <returns></returns>
        public object ComputeFieldValue(IIndexable indexable)
        {
            var contactIndexable = indexable as ContactIndexable;

            if (contactIndexable != null)
            {
                var contactId = (Guid)contactIndexable.Id.Value;

                if (contactId != null && contactId != Guid.Empty)
                {
                    var service = new ContactIntelligenceService();

                    var bestPatternMatches = service.GetBestPatternMatches(contactId, 1, 10);

                    if (bestPatternMatches != null && bestPatternMatches.Any())
                    {
                        return bestPatternMatches.Select(x => new ID(x.BestMatchedPatternId).ToShortID().ToString().ToLowerInvariant());
                    }
                }

            }

            return null;
        }
    }
}