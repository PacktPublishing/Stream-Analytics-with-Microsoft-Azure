using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.StreamAnalytics;
using Microsoft.Azure.Management.StreamAnalytics.Models;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Rest;
using System.Threading;
using System.Configuration;


namespace ASAManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string AzureresourceGroupName = "<<Your Azure Resource Group Name>>";
            string AzurestreamingJobName = "<<Your Azure ASA job name>>";
            string ASAinputName = "<<Your ASA job Input>>";
            string ASAtransformationName = "<<Your ASA Job Transformation Name>>";
            string ASAoutputName = "<<Your ASA job output Name>>";

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            // Get credentials
            ServiceClientCredentials credentials = GetAzureCredentials().Result;

            // Create Stream Analytics management client
            StreamAnalyticsManagementClient streamAnalyticsManagementClient = new StreamAnalyticsManagementClient(credentials)
            {
                SubscriptionId = ConfigurationManager.AppSettings["SubscriptionId"]
            };

            // Create Azure  streaming job
            StreamingJob streamingJob = new StreamingJob()
            {
                Tags = new Dictionary<string, string>()
    {
        { "Origin", ".NET SDK" },
        { "ReasonCreated", "Stream Analytics Job Provision Using .NET Management SDK" }
    },
                Location = "East Asia",
                EventsOutOfOrderPolicy = EventsOutOfOrderPolicy.Adjust,
                EventsOutOfOrderMaxDelayInSeconds = 2,
                EventsLateArrivalMaxDelayInSeconds = 16,
                OutputErrorPolicy = OutputErrorPolicy.Stop,
                DataLocale = "en-US",
                CompatibilityLevel = CompatibilityLevel.OneFullStopZero,
                Sku = new Sku()
                {
                    Name = SkuName.Standard
                }
            };
            StreamingJob createNewStreamingJobResult = streamAnalyticsManagementClient.StreamingJobs.CreateOrReplace(streamingJob, AzureresourceGroupName, AzurestreamingJobName);


            // Create an input
            StorageAccount storageAccount = new StorageAccount()
            {
                AccountName = "<<Your Azure Storage Account Name>>",
                AccountKey = "<<Azure Storage Account Key>>"
            };
            Input input = new Input()
            {
                Properties = new StreamInputProperties()
                {
                    Serialization = new CsvSerialization()
                    {
                        FieldDelimiter = ",",
                        Encoding = Microsoft.Azure.Management.StreamAnalytics.Models.Encoding.UTF8
                    },
                    Datasource = new BlobStreamInputDataSource()
                    {
                        StorageAccounts = new[] { storageAccount },
                        Container = "<<Your Storage Account Container>>",
                        PathPattern = "{date}/{time}",
                        DateFormat = "yyyy/MM/dd",
                        TimeFormat = "HH",
                        SourcePartitionCount = 16
                    }
                }
            };
            Input createInputResult = streamAnalyticsManagementClient.Inputs.CreateOrReplace(input, AzureresourceGroupName, AzurestreamingJobName, ASAinputName);
            // Test the connection to the input
            ResourceTestStatus testInputResult = streamAnalyticsManagementClient.Inputs.Test(AzureresourceGroupName, AzurestreamingJobName, ASAinputName);


            // Create an output
            Output output = new Output()
            {
                Datasource = new AzureSqlDatabaseOutputDataSource()
                {
                    Server = "<<Your Azure SQL Database Server>>",
                    Database = "<<Your Azure SQL db name>>",
                    User = "<<Your Azure SQL db user>>",
                    Password = "<<Your Azure SQL db password>>",
                    Table = "<<Your Azure SQL db table>>"
                }
            };
            Output createOutputResult = streamAnalyticsManagementClient.Outputs.CreateOrReplace(output, AzureresourceGroupName, AzurestreamingJobName, ASAoutputName);
            // Create a transformation
            Transformation transformation = new Transformation()
            {
                Query = "Select Id, Name from input", // '<your input name>' should be replaced with the value you put for the 'inputName' variable above or in a previous step
                StreamingUnits = 1
            };
            Transformation createTransformationResult = streamAnalyticsManagementClient.Transformations.CreateOrReplace(transformation, AzureresourceGroupName, AzurestreamingJobName, ASAtransformationName);

            
            // Start a streaming job
            StartStreamingJobParameters startStreamingJobParameters = new StartStreamingJobParameters()
            {
                OutputStartMode = OutputStartMode.CustomTime,
                OutputStartTime = new DateTime(2017, 10, 12, 12, 12, 12, DateTimeKind.Utc)
            };
            streamAnalyticsManagementClient.StreamingJobs.Start(AzureresourceGroupName, AzurestreamingJobName, startStreamingJobParameters);

            // Stop a streaming job
            streamAnalyticsManagementClient.StreamingJobs.Stop(AzureresourceGroupName, AzurestreamingJobName);

            // Delete a streaming job
         //   streamAnalyticsManagementClient.StreamingJobs.Delete(resourceGroupName, streamingJobName);

            
        }

        private static async Task<ServiceClientCredentials> GetAzureCredentials()
        {
            var activeDirectoryClientSettings = ActiveDirectoryClientSettings.UsePromptOnly(ConfigurationManager.AppSettings["ClientId"], new Uri("urn:ietf:wg:oauth:2.0:oob"));
            ServiceClientCredentials credentials = await UserTokenProvider.LoginWithPromptAsync(ConfigurationManager.AppSettings["ActiveDirectoryTenantId"], activeDirectoryClientSettings);

            return credentials;
        }

        
    }
}
