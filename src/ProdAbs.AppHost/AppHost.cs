using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataBindMount("../../container-data/postgres/Data")
    .WithLifetime(ContainerLifetime.Persistent);

var pgAdmin = postgres.WithPgAdmin(pgadmin => pgadmin.WithHostPort(9200));

var gedDb = postgres.AddDatabase("gedDb");

var eventStoreDb = postgres.AddDatabase("eventStoreDb");

var zookeeper = builder.AddContainer("zookeeper", "confluentinc/cp-zookeeper", "latest")
    .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
    .WithEnvironment("ZOOKEEPER_TICK_TIME", "2000")
    .WithEnvironment("ZOOKEEPER_SYNC_LIMIT", "2")
    .WithEnvironment("ZOOKEEPER_AUTOPURGE_PURGE_INTERVAL", "1")
    .WithEnvironment("ZOOKEEPER_AUTOPURGE_SNAP_RETAIN_COUNT", "3")
    .WithEnvironment("ZOOKEEPER_MAX_CLIENT_CNXNS", "1000")
    .WithEndpoint(2181, 2181)
    .WithLifetime(ContainerLifetime.Persistent);

var kafka = builder.AddKafka("kafka")
    .WithImageRegistry("wurstmeister")
    .WithImage("kafka", "latest")
    .WithReferenceRelationship(zookeeper)
    .WithDataBindMount(
        "../../container-data/kafka/data")
    .WithEnvironment("KAFKA_CREATE_TOPICS", "documento-criado-topic:1:1")
    .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", "zookeeper:2181")
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(zookeeper);

var kafkaUI = kafka.WithKafkaUI(kafkaUi => { kafkaUi.WithHostPort(9100); });

var seq = builder.AddSeq("seq")
    .WithDataBindMount(
        "../../container-data/seq/Data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithDataVolume()
            .WithBlobPort(27000)
            .WithQueuePort(27001)
            .WithTablePort(27002);
    })
    .AddBlobs("azure-blob-connection");

builder.AddProject<ProdAbs_Presentation_Api>("prodabs-api")
    .WithReference(postgres)
    .WithReference(kafka)
    .WithReference(gedDb)
    .WithReference(eventStoreDb)
    .WithReference(kafka)
    .WithReference(seq)
    .WithReference(storage)
    .WaitFor(storage)
    .WaitFor(kafka)
    .WaitFor(seq)
    .WaitFor(gedDb)
    .WaitFor(eventStoreDb)
    .WithEnvironment("storage-settings:provider", "Azure")
    .WithEnvironment("storage-settings:azure:container-name", "app-container");

await builder.Build().RunAsync();