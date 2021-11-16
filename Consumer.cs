using Confluent.Kafka;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

class Consumer
{

  static void Main(string[] args)
  {
    if (args.Length != 1)
    {
      Console.WriteLine("Please provide the configuration file path as a command line argument");
    }

    Console.WriteLine("Press Ctrl+C to cancel...");

    IConfiguration configuration = new ConfigurationBuilder()
        .AddIniFile(args[0])
        .Build();

    configuration["group.id"] = "fizzbuzz-consumer-group";
    configuration["auto.offset.reset"] = "earliest";

    const string topic = "fizzbuzz";

    CancellationTokenSource cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) =>
    {
      e.Cancel = true; // prevent the process from terminating.
      cts.Cancel();
    };

    using (var consumer = new ConsumerBuilder<string, string>(
        configuration.AsEnumerable()).Build())
    {
      consumer.Subscribe(topic);
      try
      {
        while (true)
        {
          var cr = consumer.Consume(cts.Token);

          // Console.WriteLine($"Consumed event from topic {topic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");

          var key = int.Parse(cr.Message.Key);
          var value = int.Parse(cr.Message.Value);

          if (value % 15 == 0)
          {
            Console.Write("FizzBuzz\t");
          }
          else if (value % 3 == 0)
          {
            Console.Write("Fizz\t");
          }
          else if (value % 5 == 0)
          {
            Console.Write("Buzz\t");
          }
          else
          {
            Console.Write($"{value}\t");
          }
        }
      }
      catch (OperationCanceledException)
      {
        // Ctrl-C was pressed.
      }
      finally
      {
        consumer.Close();
      }
    }
  }
}