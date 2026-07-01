namespace eurocsv.Services
{
    /// <summary>
    /// A background service that periodically removes expired session directories,
    /// ensuring the privacy-policy promise of "files deleted within 2 hours" is kept
    /// even when the application runs continuously without a restart.
    /// </summary>
    public class SessionPurgeService : BackgroundService
    {
        /// <summary>How long a session directory may exist before it is eligible for purging.</summary>
        public static readonly TimeSpan SessionMaxAge = TimeSpan.FromHours(2);

        /// <summary>How often the purge sweep runs.</summary>
        public static readonly TimeSpan PurgeInterval = TimeSpan.FromMinutes(30);

        private readonly ITempFileService _tempFileService;
        private readonly ILogger<SessionPurgeService> _logger;

        public SessionPurgeService(ITempFileService tempFileService, ILogger<SessionPurgeService> logger)
        {
            _tempFileService = tempFileService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "SessionPurgeService started. Purge interval: {Interval}, max session age: {MaxAge}.",
                PurgeInterval, SessionMaxAge);

            // Run an initial sweep immediately so stale files left from a previous
            // run are cleaned up without waiting for the first full interval.
            RunPurge();

            using var timer = new PeriodicTimer(PurgeInterval);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                RunPurge();
            }
        }

        private void RunPurge()
        {
            try
            {
                _logger.LogInformation(
                    "PurgeSweepStarted | MaxAge={MaxAge} | CutoffUtc={CutoffUtc}",
                    SessionMaxAge, DateTime.UtcNow - SessionMaxAge);
                _tempFileService.PurgeExpiredSessions(SessionMaxAge);
                _logger.LogInformation("PurgeSweepCompleted");
            }
            catch (Exception ex)
            {
                // Log but do not let an exception bring down the background service.
                _logger.LogError(ex, "PurgeSweepError | UnhandledExceptionDuringPurgeSweep");
            }
        }
    }
}
