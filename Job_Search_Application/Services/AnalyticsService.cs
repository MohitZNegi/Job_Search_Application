using Job_Search_Application.Data;
using Job_Search_Application.Models;

namespace Job_Search_Application.Services
{
    public class AnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, int> GetJobViews(List<Jobs_Model> jobs)
        {
            var jobIds = jobs.Select(j => j.Jobs_Id).ToList();
            var jobAnalytics = _context.Job_Analytics.Where(a => jobIds.Contains(a.JobId)).ToList();

            var jobViews = new Dictionary<string, int>();

            foreach (var analytics in jobAnalytics)
            {
                jobViews[analytics.JobId] = analytics.Views;
            }

            return jobViews;
        }

        public Dictionary<string, int> GetJobApplies(List<Jobs_Model> jobs)
        {
            var jobIds = jobs.Select(j => j.Jobs_Id).ToList();
            var jobApplies = _context.Job_Request
                .Where(r => jobIds.Contains(r.JobId))
                .GroupBy(r => r.JobId)
                .ToDictionary(g => g.Key, g => g.Count());

            return jobApplies;
        }
    }

}
