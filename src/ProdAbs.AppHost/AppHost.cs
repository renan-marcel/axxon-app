var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithDataBindMount(source: "../../container-data/postgres/Data")
    .WithLifetime(ContainerLifetime.Persistent);

var pgAdmin = postgres.WithPgAdmin(pgadmin => pgadmin.WithHostPort(9200));

var ged_db = postgres.AddDatabase("ged-db");

var event_store_db = postgres.AddDatabase("event-store-db");

var kafka = builder.AddKafka("kafka")
    .WithImageRegistry("wurstmeister")
    .WithImage("kafka", "latest")
    .WithLifetime(ContainerLifetime.Persistent);

var kafkaUI = kafka.WithKafkaUI(kafkaUi =>
{
    kafkaUi.WithHostPort(9100);
});

var seq = builder.AddSeq("seq")
    .WithDataBindMount(
        source: "../../../container-data/seq/Data",
        isReadOnly: false)
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
    .AddBlobs("blobs");

builder.AddProject<Projects.ProdAbs_Presentation_Api>("prodabs-api")
    .WithReference(postgres)
    .WithReference(kafka)
    .WithReference(ged_db)
    .WithReference(event_store_db)
    .WithReference(kafka)
    .WithReference(seq)
    .WithReference(storage)
    .WaitFor(storage)
    .WaitFor(kafka)
    .WaitFor(seq)
    .WaitFor(ged_db)
    .WaitFor(event_store_db);

await builder.Build().RunAsync();