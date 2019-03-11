using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase.Configuration.Client;
using FluentAssertions;
using HealthChecks.Couchbase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Xunit;

namespace UnitTests.HealthChecks.DependencyInjection.Couchbase
{
    public class couchbase_registration_should
    {
        [Fact]
        public void add_health_check_when_properly_configured_default_client_connection()
        {
            var host = "localhost";
            var port = "8091";

            var services = new ServiceCollection();
            services.AddHealthChecks().AddCouchbase(host, port);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("couchbase");
            check.GetType().Should().Be(typeof(CouchbaseHealthCheck));
        }
        [Fact]
        public void add_health_check_when_properly_configured_cluster_with_check_only_host()
        {
            var host = "localhost";
            var port = "8091";

            var services = new ServiceCollection();
            services.AddHealthChecks().AddCouchbase(host, port);

      
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("couchbase");
            check.GetType().Should().Be(typeof(CouchbaseHealthCheck));
        }

        [Fact]
        public void add_named_health_check_when_properly_configured_cluster_with_check_host_and_buckets()
        {
            var host = "localhost";
            var port = "8091";
            var buckets = new string[] { "testbucket" };

            var services = new ServiceCollection();
            services.AddHealthChecks().AddCouchbase(host, port, buckets);


            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("couchbase");
            check.GetType().Should().Be(typeof(CouchbaseHealthCheck));
        }

        [Fact]
        public void add_named_health_check_when_properly_configured_couchbase_client_configuration()
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

            var services = new ServiceCollection();
            services.AddHealthChecks().AddCouchbase(configuration);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("couchbase");
            check.GetType().Should().Be(typeof(CouchbaseHealthCheck));
        }

        [Fact]
        public void add_named_health_check_when_properly_configured_couchbase_configuration_definition()
        {
            var definition = new CouchbaseClientDefinition
            {
                Servers = new List<Uri> {
                    new Uri("http://localhost:8091")
                },
                Buckets = new List<BucketDefinition>
                {   
                    new BucketDefinition
                    {
                        Name = "testbucket"                        
                    }
                }
            };

            var configuration = new ClientConfiguration(definition);
            
            var services = new ServiceCollection();
            services.AddHealthChecks().AddCouchbase(configuration);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("couchbase");
            check.GetType().Should().Be(typeof(CouchbaseHealthCheck));
        }
    }
}
