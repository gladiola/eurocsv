using eurocsv.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace eurocsv.Tests.Services
{
    public class SessionPurgeServiceTests
    {
        // ── Constants ────────────────────────────────────────────────────────────

        [Fact]
        public void SessionMaxAge_IsTwoHours()
        {
            Assert.Equal(TimeSpan.FromHours(2), SessionPurgeService.SessionMaxAge);
        }

        [Fact]
        public void PurgeInterval_IsThirtyMinutes()
        {
            Assert.Equal(TimeSpan.FromMinutes(30), SessionPurgeService.PurgeInterval);
        }

        // ── Immediate sweep on start ─────────────────────────────────────────────

        [Fact]
        public async Task ExecuteAsync_RunsInitialPurgeImmediately()
        {
            var fake = new FakeTempFileService();
            var svc = new SessionPurgeService(fake, NullLogger<SessionPurgeService>.Instance);

            using var cts = new CancellationTokenSource();
            var task = svc.StartAsync(cts.Token);

            // Give the background loop a moment to run the initial sweep.
            await Task.Delay(100);
            cts.Cancel();
            await task;

            Assert.True(fake.PurgeCallCount >= 1,
                "Expected at least one purge call immediately after start.");
        }

        [Fact]
        public async Task ExecuteAsync_PassesCorrectMaxAge()
        {
            var fake = new FakeTempFileService();
            var svc = new SessionPurgeService(fake, NullLogger<SessionPurgeService>.Instance);

            using var cts = new CancellationTokenSource();
            var task = svc.StartAsync(cts.Token);
            await Task.Delay(100);
            cts.Cancel();
            await task;

            foreach (var age in fake.RecordedMaxAges)
                Assert.Equal(SessionPurgeService.SessionMaxAge, age);
        }

        // ── Exception resilience ─────────────────────────────────────────────────

        [Fact]
        public async Task ExecuteAsync_DoesNotFaultWhenPurgeThrows()
        {
            var fake = new FakeTempFileService { ThrowOnPurge = true };
            var svc = new SessionPurgeService(fake, NullLogger<SessionPurgeService>.Instance);

            using var cts = new CancellationTokenSource();
            var task = svc.StartAsync(cts.Token);
            await Task.Delay(100);
            cts.Cancel();

            // Must not throw even though PurgeExpiredSessions threw.
            var ex = await Record.ExceptionAsync(() => task);
            Assert.Null(ex);
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        private sealed class FakeTempFileService : ITempFileService
        {
            public int PurgeCallCount { get; private set; }
            public List<TimeSpan> RecordedMaxAges { get; } = new();
            public bool ThrowOnPurge { get; set; }

            public void PurgeExpiredSessions(TimeSpan maxAge)
            {
                PurgeCallCount++;
                RecordedMaxAges.Add(maxAge);
                if (ThrowOnPurge)
                    throw new InvalidOperationException("Simulated purge failure.");
            }

            // Remaining interface members — not exercised by these tests.
            public Task<(string SessionId, string FilePath)> SaveUploadAsync(IFormFile file, CancellationToken ct = default)
                => throw new NotImplementedException();
            public Task<string> SaveTransformedAsync(string sessionId, Stream content, string outputFileName, CancellationToken ct = default)
                => throw new NotImplementedException();
            public string? GetOriginalPath(string sessionId) => throw new NotImplementedException();
            public string? GetTransformedPath(string sessionId) => throw new NotImplementedException();
            public void CleanupSession(string sessionId) => throw new NotImplementedException();
        }
    }
}
