namespace Suneco.SitecoreBlogs.BestPatternMatches.ListManager.Conditions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Sitecore.Analytics.Rules.SegmentBuilder;
    using Sitecore.ContentSearch.Analytics.Models;
    using Sitecore.ContentSearch.Rules;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Rules.Conditions;

    /// <summary>
    /// Segmenation rule for best pattern match
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BestPatternMatchCondition<T> : WhenCondition<T>, IQueryableRule<IndexedContact> 
        where T : VisitorRuleContext<IndexedContact>
    {
        /// <summary>
        /// Gets or sets the initialize predicate.
        /// </summary>
        /// <value>
        /// The initialize predicate.
        /// </value>
        public Expression<Func<IndexedContact, bool>> InitPredicate { protected get; set; }

        /// <summary>
        /// Gets or sets the result predicate.
        /// </summary>
        /// <value>
        /// The result predicate.
        /// </value>
        public Expression<Func<IndexedContact, bool>> ResultPredicate { get; protected set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        public string PatternId { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        /// <value>
        /// The profile identifier.
        /// </value>
        public string ProfileId { get; set; }

        /// <summary>
        /// Executes the specified rule context.
        /// </summary>
        /// <param name="ruleContext">The rule context.</param>
        /// <returns></returns>
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            if (string.IsNullOrWhiteSpace(this.PatternId) || !ID.IsID(this.PatternId))
            {
                this.ApplyPredicate(ruleContext, contact => false);
                return false;
            }

            var patternIdString = new ID(this.PatternId).ToShortID().ToString().ToLower();

            this.ApplyPredicate(ruleContext, c => ((List<string>)c[(ObjectIndexerKey)"suneco.bestpatternmatches"]).Contains(patternIdString));

            return true;
        }

        /// <summary>
        /// Applies the predicate.
        /// </summary>
        /// <param name="ruleContext">The rule context.</param>
        /// <param name="expression">The expression.</param>
        private void ApplyPredicate(T ruleContext, Expression<Func<IndexedContact, bool>> expression)
        {
            this.ResultPredicate = (this.InitPredicate ?? ruleContext.Where).And(expression);

            if (this.InitPredicate == null)
            {
                ruleContext.Where = this.ResultPredicate;
            }
        }
    }
}
