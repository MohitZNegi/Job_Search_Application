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
            var jobAnalytics = _context.Job_Analytics.Where(a => jobIds.Contains(a.JobId)).ToList();

            var jobApplies = new Dictionary<string, int>();

            foreach (var analytics in jobAnalytics)
            {
                jobApplies[analytics.JobId] = analytics.Applies;
            }

            return jobApplies;
        }

        public Dictionary<string, int> GetReviewedCandidates(List<Jobs_Model> jobs)
        {
            var jobIds = jobs.Select(j => j.Jobs_Id).ToList();
            var jobAnalytics = _context.Job_Analytics.Where(a => jobIds.Contains(a.JobId)).ToList();

            var reviewedCandidates = new Dictionary<string, int>();

            foreach (var analytics in jobAnalytics)
            {
                reviewedCandidates[analytics.JobId] = analytics.ReviewedCandidates;
            }

            return reviewedCandidates;
        }

        public Dictionary<string, int> GetSelectedCandidates(List<Jobs_Model> jobs)
        {
            var jobIds = jobs.Select(j => j.Jobs_Id).ToList();
            var jobAnalytics = _context.Job_Analytics.Where(a => jobIds.Contains(a.JobId)).ToList();

            var selectedCandidates = new Dictionary<string, int>();

            foreach (var analytics in jobAnalytics)
            {
                selectedCandidates[analytics.JobId] = analytics.SelectedCandidates;
            }

            return selectedCandidates;
        }


    }

}
