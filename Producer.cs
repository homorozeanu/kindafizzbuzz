using Confluent.Kafka;
using System;
using Microsoft.Extensions.Configuration;

class Producer
{
  static void Main(string[] args)
  {
    if (args.Length != 1)
    {
      Console.WriteLine("Please provide the configuration file path as a command line argument");
    }

    IConfiguration configuration = new ConfigurationBuilder()
        .AddIniFile(args[0])
        .Build();

    const string topic = "fizzbuzz";
    Random rnd = new Random();

    using (var producer = new ProducerBuilder<string, string>(
        configuration.AsEnumerable()).Build())
    {
      var numProduced = 0;
      const int numMessages = 100;

      for (int i = 1; i <= numMessages; i++)
      {
        producer.Produce(topic, new Message<string, string> { Key = i.ToString(), Value = i.ToString() },
            (deliveryReport) =>
            {
              if (deliveryReport.Error.Code != ErrorCode.NoError)
              {
                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
              }
              else
              {
                numProduced += 1;
              }
            });
      }

      int outQueueLength = producer.Flush(TimeSpan.FromSeconds(10));
      Console.WriteLine($"Flush reported {outQueueLength} as Out Queue Length");
      Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
    }
  }
}