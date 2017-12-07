using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unclassified.Util;

namespace SendPhoneSensorDataToAzure
{
    class Program
    {
        static string eventHubName = "iotasa";
        static string connectionString = GetServiceBusConnectionString();
        static string data = string.Empty;
        static void Main(string[] args)
        {
            
            string csv_file_path = string.Empty;
            install();
            //string csv_file_path = @"C:\ASA\input\demodata.csv";
            string[] filePath = Directory.GetFiles(@"E:\MSFT Laptop\ASA-Packt\ASA-Demos\accelerometer", "*.csv");
            int size = filePath.Length;
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(filePath[i]);
                 csv_file_path = filePath[i];
            }
            
           // string csv_file_path = @"C:\Users\Anindita\OneDrive\MAX451\BGClub\Attendance\All_Clubs_Progrma_Attendance_12.2015.csv";
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
                            foreach (DataRow rows in table.Rows)
                            {
                                var info = new Accelerometer
                                {



                                    ID = rows.ItemArray[0].ToString(),
                                    Coordinate_X = rows.ItemArray[1].ToString(),
                                    Coordinate_Y = rows.ItemArray[2].ToString(),
                                    Coordinate_Z = rows.ItemArray[3].ToString()

                                    

                                };
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

        public static void install()
        {
            string url = @"https://sensoriot.blob.core.windows.net/accelerometer/AccelerometerSensorData.csv";
            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            //   Console.WriteLine("Download OnProgress......");

            ConsoleHelper.ProgressTitle = "Downloading";
            ConsoleHelper.ProgressTotal = 10;
            for (int i = 0; i <= 10; i++)
            {
                ConsoleHelper.ProgressValue = i;
                Thread.Sleep(500);
                if (i >= 5)
                {
                    ConsoleHelper.ProgressHasWarning = true;
                }
                if (i >= 8)
                {
                    ConsoleHelper.ProgressHasError = true;
                }
            }
            ConsoleHelper.ProgressTotal = 0;
            try
            {
                wc.DownloadFile(new Uri(url), @"E:\MSFT Laptop\ASA-Packt\ASA-Demos\accelerometer\AccelerometerSensorData.csv");
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Console.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
            }
        }


        public static void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download Completed!");
        }

        public static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("{0} Downloaded {1} of {2} bytes,{3} % Complete....",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
            DrawProgressBar(0, 100, Console.WindowWidth, '1');
        }

        private static void DrawProgressBar(int complete, int maxVal, int barSize, char ProgressCharacter)
        {
            Console.CursorVisible = false;
            int left = Console.CursorLeft;
            decimal perc = (decimal)complete / (decimal)maxVal;
            int chars = (int)Math.Floor(perc / ((decimal)1 / (decimal)barSize));
            string p1 = String.Empty, p2 = String.Empty;

            for (int i = 0; i < chars; i++) p1 += ProgressCharacter;
            for (int i = 0; i < barSize - chars; i++) p2 += ProgressCharacter;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(p1);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(p2);

            Console.ResetColor();
            Console.Write("{0}%", (perc * 100).ToString("N2"));
            Console.CursorLeft = left;
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
            builder.TransportType = Microsoft.ServiceBus.Messaging.TransportType.Amqp;
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
    