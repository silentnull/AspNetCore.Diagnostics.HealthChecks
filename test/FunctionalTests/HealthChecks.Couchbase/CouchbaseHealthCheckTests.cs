using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using FluentAssertions;
using FunctionalTests.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.HealthChecks.Couchbase
{
    [Collection("execution")]
    public class couchbase_healthcheck_should
    {
        private readonly ExecutionFixture _fixture;

        public couchbase_healthcheck_should(ExecutionFixture fixture)
        { 
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [SkipOnAppVeyor]
        public async Task be_healthy_if_couchbase_is_available()
        {
            var configuration = new ClientConfiguration
            {
                Servers = new List<Uri> {
                    new Uri("http://localhost:8091")
                },
                BucketConfigs = new Dictionary<string, BucketConfiguration> 
                {
                    {
                        "testbucket", new BucketConfiguration
                        {
                            BucketName = "testbucket"
                        }
                    }
                }
            };

            //create the cluster and pass in the RBAC user
            var authenticator = new PasswordAuthenticator("Admin", "testpasswd");
            configuration.SetAuthenticator(authenticator);

            var webHostBuilder = new WebHostBuilder()
            .UseStartup<DefaultStartup>()
            .ConfigureServices(services =>
            {
                services.AddHealthChecks()
                    .AddCouchbase(configuration, tags: new string[] { "couchbase" });
            })
            .Configure(app =>
            {
                app.UseHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = r => r.Tags.Contains("couchbase")
                });
            });

            var server = new TestServer(webHostBuilder);

            var response = await server.CreateRequest($"/health")
                .GetAsync();

            response.StatusCode
                .Should().Be(HttpStatusCode.OK);
        }

        [SkipOnAppVeyor]
        public async Task be_unhealthy_if_couchbase_is_not_available()
        {
            var configuration = new ClientConfiguration
            {
                Servers = new List<Uri> {
                    new Uri("http://localhost:80")
                },
                BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    {
                        "noexitstestbucket", new BucketConfiguration
                        {
                            BucketName = "noexiststestbucket"
                        }
                    }
                }
            };

            //create the cluster and pass in the RBAC user
            var authenticator = new PasswordAuthenticator("Admin", "testpasswd");
            configuration.SetAuthenticator(authenticator);

            var webHostBuilder = new WebHostBuilder()
           .UseStartup<DefaultStartup>()
           .ConfigureServices(services =>
           {
               services.AddHealthChecks()
                .AddCouchbase(configuration, tags: new string[] { "couchbase" });
           })
           .Configure(app =>
           {
               app.UseHealthChecks("/health", new HealthCheckOptions()
               {
                   Predicate = r => r.Tags.Contains("couchbase")
               });
           });

            var server = new TestServer(webHostBuilder);

            var response = await server.CreateRequest($"/health")
                .GetAsync();

            response.StatusCode
                .Should().Be(HttpStatusCode.ServiceUnavailable);
        }
    }
}
