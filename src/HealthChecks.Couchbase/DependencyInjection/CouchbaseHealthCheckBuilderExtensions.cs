using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using HealthChecks.Couchbase;
using Couchbase.Configuration.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CouchbaseHealthCheckBuilderExtensions
    {
        const string NAME = "couchbase";

        /// <summary>
        /// Add a health check for Couchbase databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="hostname">The Couchbase server connection hostname (default: 127.0.0.1). </param>
        /// <param name="port">The Couchbase server connection port (default: 8091). </param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'couchbase' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns></param>
        public static IHealthChecksBuilder AddCouchbase(this IHealthChecksBuilder builder, string hostname, string port, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            var options = new CouchbaseOptions();
            options.UseServer(hostname, port);
            
            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthCheck(options),
                failureStatus,
                tags));
        }

        /// <summary>
        /// Add a health check for Couchbase databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="hostname">The Couchbase server connection hostname (default: 127.0.0.1). </param>
        /// <param name="port">The Couchbase server connection port (default: 8091). </param>
        /// <param name="buckets">List of buckets</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'couchbase' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns></param>
        public static IHealthChecksBuilder AddCouchbase(this IHealthChecksBuilder builder, string hostname, string port, string[] buckets, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            var options = new CouchbaseOptions();
            options.UseServer(hostname, port, buckets);

            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthCheck(options),
                failureStatus,
                tags));
        }

        /// <summary>
        /// Add a health check for Couchbase databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="configuration">Use Couchbase client configuration (read from config JSON file).</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'couchbase' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns></param>
        public static IHealthChecksBuilder AddCouchbase(this IHealthChecksBuilder builder, ClientConfiguration configuration, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            var options = new CouchbaseOptions();
            options.UseClientConfiguration(configuration);

            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthCheck(options),
                failureStatus,
                tags));
        }

        /// <summary>
        /// Add a health check for Couchbase databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="configuration">Use Couchbase client configuration (read from config JSON file).</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'couchbase' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns></param>
        public static IHealthChecksBuilder AddCouchbase(this IHealthChecksBuilder builder, ICouchbaseClientDefinition clientDefinition, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            var options = new CouchbaseOptions();
            options.UseClientDefinition(clientDefinition);

            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthCheck(options),
                failureStatus,
                tags));
        }

        /// <summary>
        /// Add a health check for Couchbase databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="clientDefinition">Use Couchbase client configuration  definition (read from config JSON file).</param>
        /// <param name="certKeyPath">The physical path to X509 certification</param>
        /// <param name="password">The certificate security password</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'couchbase' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns></param>
        public static IHealthChecksBuilder AddCouchbase(this IHealthChecksBuilder builder, ICouchbaseClientDefinition clientDefinition, string certKeyPath, string password, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            var options = new CouchbaseOptions();
            options.UseClientDefinition(clientDefinition);
            options.UseCertificate(certKeyPath, password);

            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthCheck(options),
                failureStatus,
                tags));
        }

        /// <summary>
        /// Add a health check for Couchbase databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="setup">The Couchbase option setup.</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'couchbase' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns></param>
        public static IHealthChecksBuilder AddCouchbase(this IHealthChecksBuilder builder, Action<CouchbaseOptions> setup, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            var options = new CouchbaseOptions();
            setup?.Invoke(options);
            
            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new CouchbaseHealthCheck(options),
                failureStatus,
                tags));
        }
    }
}
