using Couchbase;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecks.Couchbase
{
    public class CouchbaseHealthCheck : IHealthCheck
    {
        private readonly CouchbaseOptions options;
        public CouchbaseHealthCheck(CouchbaseOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                ClusterHelper.Initialize(options.Configuration);

                if (!ClusterHelper.Initialized)
                {
                    throw new CouchbaseResponseException("Cannot initialize a cluster with set configuration.");
                }

                var cluster = ClusterHelper.Get();

                var clusterReport = cluster.Diagnostics();
                if (clusterReport == null || clusterReport.Services.Count == 0 || string.IsNullOrEmpty(clusterReport.Id) )
                {
                    return Task.FromResult(
                        new HealthCheckResult(context.Registration.FailureStatus, description: $"Couchbase cluster failed."));
                }

                foreach (var bucketName in options.Buckets)
                {
                    var bucket = ClusterHelper.GetBucket(bucketName);

                    var report = bucket.Ping();

                    if (report == null || string.IsNullOrEmpty(report.Id))
                    {
                        return Task.FromResult(
                            new HealthCheckResult(context.Registration.FailureStatus, description: $"Couchbase bucket: {bucket.Name},  ping failed."));
                    }

                }

                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception ex)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
            }
        }
    }
}
