using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace webapp_homeWork1
{
	public class MyResponse
	{
		public int userId { get; set; }
		public int id { get; set; }
		public string title { get; set; }
		public string body { get; set; }
	}

	class Program
	{
		private static readonly HttpClient myClient = new HttpClient();
		private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

		public static async Task Main(string[] args)
		{
			//получить записи с 4-й по 13-ю включительно
			var myTasks = new List<Task<MyResponse>>();
			Console.WriteLine("Получение данных:");
			for (int i = 4; i <= 13; i++)
			{				
				var task = GetRequest(i);
				myTasks.Add(task);
			}

			//ждем выполнения задач
			try
			{
				tokenSource.CancelAfter(10000);
				_ = await Task.WhenAll(myTasks);               
			} 
			catch (TaskCanceledException myex)
			{
				Console.WriteLine("Выполнение задач остановлено, ошибка: " + myex.Message);
			}
			Console.WriteLine("Все данные получены");

			//запись в текстовый файл
			string fileName = "result.txt";
			Console.WriteLine($"Запись в файл {fileName}");
			try
			{
				using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
				{
					for (int i = 0; i < myTasks.Count; i++)
					{
						if (myTasks[i].Result.title != null)
						{
							await sw.WriteLineAsync($"{myTasks[i].Result.userId}");
							await sw.WriteLineAsync($"{myTasks[i].Result.id}");
							await sw.WriteLineAsync($"{myTasks[i].Result.title}");
							await sw.WriteLineAsync($"{myTasks[i].Result.body}\n");
						}                                                
					}
					Console.WriteLine($"Запись завершена");
				}                
			}
			catch(Exception myex)
			{
				Console.WriteLine($"Ошибка записи в файл: {myex.Message}");
			}
		}

		static async Task<MyResponse> GetRequest(int id)
		{
			MyResponse result = new MyResponse();			
			try
			{
				Console.WriteLine($"https://jsonplaceholder.typicode.com/posts/{id}");
				HttpResponseMessage response = await myClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}", tokenSource.Token);
				response.EnsureSuccessStatusCode();//проверка                
				string responseBody = await response.Content.ReadAsStringAsync();
				result = JsonSerializer.Deserialize<MyResponse>(responseBody);				
			}
			catch (HttpRequestException myex)
			{                
				Console.WriteLine("Exception ", myex.Message);
			}
			return result;
		}
	}
}
