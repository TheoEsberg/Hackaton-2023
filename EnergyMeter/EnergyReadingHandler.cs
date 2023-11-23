using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

namespace client.EnergyMeter;

public interface IEnergyReadingHandler
{
    Task HandleMessage(string topic, string payload);
}

public class EnergyReadingHandler : IEnergyReadingHandler
{
    public const string TopicPrefix = "foredev";
    private const string OperationEnergyReading = "EnergyReader";
    private const string OperationLog = "log";
    

	public async Task HandleMessage(string topic, string payload)
    {
        var topicComponents = topic.Split("/");
        if (topicComponents.Length != 3 && topicComponents.Length != 4)
        {
            return;
        }

        var operation = topicComponents.Length == 3 ? topicComponents[1] : topicComponents[3];
        var id = topicComponents[2];

        switch (operation)
        {
            case OperationEnergyReading:
                await HandleEnergyReader(id, payload);
                break;
            case OperationLog:
                break;
        }
    }

    private Task HandleEnergyReader(string id, string payload)
    {
        var data = EnergyMeterDataParser.Parse(payload);
       
		//var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		using (StreamWriter sw = 
            new StreamWriter("C:\\Users\\Theo\\Desktop\\Hackaton\\Big App\\Minimal-API\\data.txt", true)){
            sw.WriteLine(id + JsonSerializer.Serialize(data));
        }

		//using (StreamWriter sw = new StreamWriter(path, false)) { sw.WriteLine(data); }

		//if (id == "Varberg_A")
		//{
  //          return Task.CompletedTask;
		//	//NewData = JsonSerializer.Serialize(data);
		//	//await GetData(data.ToString());
		//}

		//if (id == "Varberg_B")
  //      {
  //          string jsonString = JsonSerializer.Serialize(data);
  //          Console.WriteLine(jsonString);
  //      }

        return Task.CompletedTask;
    }
}