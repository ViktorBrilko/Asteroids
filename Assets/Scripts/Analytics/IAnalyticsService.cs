namespace Analytics
{
    public interface IAnalyticsService
    {
        public void LogEvent(string eventName);
    }
}