using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Couchbase.Authentication.X509;
using Couchbase.Configuration.Client;

namespace HealthChecks.Couchbase
{
    public class CouchbaseOptions
    {
        public string ClusterName { get; private set; } = "Test";

        public string Hostname { get; private set; } = "127.0.0.1";
        public string Port { get; private set; } = "8091";
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool EnhancedAuth { get; private set; } = false;

        public List<string> Buckets { get; private set; } = new List<string> { "Default" };
        
        public ClientConfiguration Configuration { get; private set; }

        public CouchbaseClientDefinition Definition { get; private set; }

        public CertificateStoreOptions CertificateStore { get; private set; }

        public CouchbaseOptions()
        {
            Configuration = GetDefaultConfiguration();
        }

        public CouchbaseOptions UseBasicAuthentication(string name, string password)
        {
            UserName = name ?? throw new ArgumentNullException(nameof(name));
            Password = password ?? throw new ArgumentNullException(nameof(password));

            return this;
        }

        public CouchbaseOptions UseServer(string hostname, string port)
        {
            Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            Port = port ?? throw new ArgumentNullException(nameof(port));

            Configuration = GetDefaultConfiguration();

            return this;
        }

        public CouchbaseOptions UseServer(string hostname, string port, string[] buckets)
        {
            var bucketsConfiguration = buckets.ToDictionary(item => item, value => new BucketConfiguration { BucketName = value }) ?? throw new ArgumentNullException(nameof(buckets));

            UseServer(hostname, port);
            Configuration.BucketConfigs = bucketsConfiguration;

            return this;
        }


        public CouchbaseOptions UseClientConfiguration(ClientConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Configuration.EnableDeadServiceUriPing = true;

            return this;
        }

        public CouchbaseOptions UseClientDefinition(ICouchbaseClientDefinition definition)
        {
            Configuration = new ClientConfiguration(definition) ?? throw new ArgumentNullException(nameof(definition));
            Configuration.EnableDeadServiceUriPing = true;

            return this;
        }

        public CouchbaseOptions UseCertificate(string path, string password)
        {
            var factory = CertificateFactory.GetCertificatesByPathAndPassword(
                new PathAndPasswordOptions
                {
                    Path = path,
                    Password = password
                });

            Configuration.CertificateFactory = factory;

            return this;
        }

        public CouchbaseOptions UseCertificate(CertificateStoreOptions certificateStore)
        {
            var factory = CertificateFactory.GetCertificatesFromStore(certificateStore);
            Configuration.CertificateFactory = factory;
            return this;
        }


        private ClientConfiguration GetDefaultConfiguration()
        {
            return new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    BuildBootStrapUrl()
                },
                EnableDeadServiceUriPing = true
            };
        }

        private Uri BuildBootStrapUrl()
        {
            return new Uri($"http://{Hostname}:{Port}/"); 
        }

    }
}