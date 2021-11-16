# A kinda fizz buzz using Kafka

## Run Kafka and Zookeeper with docker-compose

```sh
docker-compose up -d
```

## Create Topic

```sh
docker compose exec kafka-1 \
  kafka-topics --create \
    --topic fizzbuzz \
    --bootstrap-server localhost:9092 \
    --replication-factor 1 \
    --partitions 1
```

## Run the producer

```sh
dotnet run --project producer.csproj $PWD/getting-started.properties
```

## Run the consumer

```sh
dotnet run --project consumer.csproj $PWD/getting-started.properties 
```

## Hint
Use [OffsetExplorer](https://www.kafkatool.com/) to inspect Kafka Brokers, Topics, Producers and Consumers

## Sources
- https://www.baeldung.com/ops/kafka-docker-setup
- https://developer.confluent.io/get-started/dotnet