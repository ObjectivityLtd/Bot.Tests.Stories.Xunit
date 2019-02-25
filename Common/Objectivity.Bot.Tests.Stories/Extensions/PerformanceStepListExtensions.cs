namespace Objectivity.Bot.Tests.Stories.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StoryModel;

    public static class PerformanceStepListExtensions
    {
        public static string[] TryGetOptions<T>(this List<PerformanceStep<T>> performanceSteps, int takeCount = 0)
        {
            takeCount = Math.Min(takeCount <= 0 ? performanceSteps.Count : takeCount, performanceSteps.Count);

            var takenSteps = performanceSteps.Take(takeCount).ToList();
            var performanceStep = takenSteps.LastOrDefault(step => step.Options != null);

            return performanceStep?.Options;
        }

        public static void AddNotNullStep<T>(this List<PerformanceStep<T>> performanceSteps, PerformanceStep<T> performanceStep)
        {
            if (performanceStep != null)
            {
                performanceStep.StepIndex = performanceSteps.Count;

                performanceSteps.Add(performanceStep);
            }
        }
    }
}
