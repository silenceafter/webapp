using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace webapp_homeWork1
{
	public class Response
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
			var Tasks = new List<Task<Response>>();
			Console.WriteLine("Получение данных:");
			for (int i = 4; i <= 13; i++)
			{
				var task = GetRequest(i);
				if (task.Exception == null)
				{
					//нет ошибки чтения => добавляем на выполнение (?)
					Tasks.Add(task);
				}
			}

			//ждем выполнения задач
			try
			{
				tokenSource.CancelAfter(10000);
				_ = await Task.WhenAll(Tasks);
			}
			catch (TaskCanceledException ex)
			{
				Console.WriteLine("Выполнение задач остановлено, ошибка: " + ex.Message);
			}
			finally
			{
				tokenSource.Dispose();
			}

			//запись в текстовый файл
			string filename = "result.txt";
			Console.WriteLine($"\nЗапись в файл {filename}");
			try
			{
				using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
				{
					for (int i = 0; i < Tasks.Count; i++)
					{						
						if (WriteToFile(Tasks[i], sw))
						{
							Console.WriteLine($"Строка {i} записана успешно");
						}
						else
						{
							Console.WriteLine($"Строка {i} не записана");
						}
					}
					Console.WriteLine($"Запись завершена");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
			}
		}

		static async Task<Response> GetRequest(int id)
		{
			Response result = new Response();
			try
			{
				Console.WriteLine($"https://jsonplaceholder.typicode.com/posts/{id}");
				HttpResponseMessage response = await myClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}", tokenSource.Token);
				response.EnsureSuccessStatusCode();//проверка                
				string responseBody = await response.Content.ReadAsStringAsync();
				result = JsonSerializer.Deserialize<Response>(responseBody);
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine("Exception ", ex.Message);
			}
			return result;
		}

        static bool WriteToFile(Task<Response> task, StreamWriter sw)
        {
			var status = true;
			try
			{
				if (task.IsCompletedSuccessfully)
				{
                    sw.WriteLine($"{task.Result.userId}");
					sw.WriteLine($"{task.Result.id}");
					sw.WriteLine($"{task.Result.title}");
					sw.WriteLine($"{task.Result.body}\n");                    
				}				
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
				status = false;
			}
            return status;
        }
	}
}