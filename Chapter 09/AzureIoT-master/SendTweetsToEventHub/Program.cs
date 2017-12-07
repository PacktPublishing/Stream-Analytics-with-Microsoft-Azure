using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SendTweetsToEventHub
{
    class Program
    {
        static string eventHubName = "tweetsentiments";
        static string connectionString = GetServiceBusConnectionString();
        static string data = string.Empty;

                
        static  void Main(string[] args)
        {

            //string csv_file_path = string.Empty;
            //string csv_file_path = @"C:\ASA\input\demodata.csv";
            //string[] filePath = Directory.GetFiles(@"C:\Users\Anindita\OneDrive\Hadoop Resources\Training\TwitterDemo\TwitterData\", "*.csv");
            //int size = filePath.Length;
            //for (int i = 0; i < size; i++)
            //{
            //    Console.WriteLine(filePath[i]);
            //     csv_file_path = filePath[i];
            //}

            string csv_file_path = @"E:\MSFT Laptop\ASA-Packt\ASA-Demos\SentimentTweets.csv";
            DataTable csvData = GetDataTableFromCSVFile(csv_file_path);
            Console.WriteLine("Rows count:" + csvData.Rows.Count);
            DataTable table = csvData;
            foreach (DataRow row in table.Rows)
            {
                // Console.WriteLine("---Row---");
                   foreach (var item in row.ItemArray)
                    {

                        data = item.ToString();
                        Console.Write(data);

                        var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
                        //while (true)
                        //{

                        try
                        {
                            Regex reg = new Regex("^[A-Za-z0-9]+$");
                            foreach (DataRow rows in table.Rows)
                            {
                                var info = new Tweets
                                {


                                   Text = rows.ItemArray[0].ToString()
                                    //DateTime = rows.ItemArray[0].ToString(),
                                    //TwitterUserName = rows.ItemArray[1].ToString(),
                                    //ProfileLocation = rows.ItemArray[2].ToString(),
                                    
                                    //MorePreciseLocation = rows.ItemArray[3].ToString(),
                                                                      
                                    //Country = rows.ItemArray[4].ToString(),
                                    //TweetID = rows.ItemArray[5].ToString()
                                    
                                                                        

                                };
                                //if(reg.IsMatch(info.MorePreciseLocation))
                                //{
                                //    info.MorePreciseLocation = "";
                                //    info.Country = "";
                                //    info.TweetID = rows.ItemArray[3].ToString();
                                //}
                                //if(info.ProfileLocation == "")
                                //{
                                //    info.ProfileLocation = "";
                                //    info.TweetID = rows.ItemArray[3].ToString();
                                //}
                                
                                var serializedString = JsonConvert.SerializeObject(info);
                                var message = data;
                                Console.WriteLine("{0}> Sending events: {1}", DateTime.Now.ToString(), serializedString.ToString());
                                eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(serializedString.ToString())));

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("{0} > Exception: {1}", DateTime.Now.ToString(), ex.Message);
                            Console.ResetColor();
                        }
                            Task.Delay(200);
                        //}


                    }
                
            }
            // Console.ReadLine();

            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
            
         //   SendingRandomMessages().Wait();

        }

        private static DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            string data = string.Empty;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return csvData;
        }

        private static string GetServiceBusConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)");
                return string.Empty;
            }
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
            builder.TransportType = TransportType.Amqp;
            return builder.ToString();
        }

        static async Task SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {
                try
                {
                    var message = data;
                    Console.WriteLine("{0}> Sending events: {1}", DateTime.Now.ToString(), message);
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now.ToString(), ex.Message);
                    Console.ResetColor();
                }
                await Task.Delay(200);
            }

        }
    }
  }
    
