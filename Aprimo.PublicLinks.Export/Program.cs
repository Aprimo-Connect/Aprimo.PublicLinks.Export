using System.Configuration;
using System.Collections.Specialized;
using Aprimo.PublicLinks.Export.CustomObjects;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Aprimo.PublicLinks.Export
{
    internal class Program
    {
        // ARGS
        // -Destination - folder to save the new .csv to
        // -Name - name of the new csv. full path will be destination/name.csv
        private static AprimoRESTApi aprimoAPI;
        private static string destinationFolder = "C:\\Users\\James.Ratini\\OneDrive - Aprimo US, LLC\\Documents\\AprimoPublicCDNExport";
        private static string nameofCSV = "export_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
        private static int delayBetweenCalls = 5000;
        static void Main(string[] args)
        {
            
            /*if (args == null)
            {
                // No path passed in. 
                destinationFolder = Environment.CurrentDirectory;
            }*/
            
            // Create AprimoRESTApi
            try
            {
                aprimoAPI = new AprimoRESTApi();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Get Aprimo Access Token
            aprimoAPI.getAccessTokenClientCredentials();

            #region Aprimo Search
            
            // Create list of headers
            List<Tuple<string, string>> headers = new List<Tuple<string, string>>
            {
                new Tuple<string,string>("select-record", "masterfilelatestversion"),
                new Tuple<string,string>("select-fileversion", "publicuris"),
            };

            // Create search expression
            HttpContent expression = JsonContent.Create(new 
            { 
                searchExpression = new 
                {
                    expression = "LatestVersionOfMasterFile.HasPublicUri = true" 
                },
                logRequest = "true"
            });


            List<AprimoPublicLinkRecord> records = new List<AprimoPublicLinkRecord>();
            int pageSize = 50;
            int page = 1;

            dynamic response = aprimoAPI.searchAprimoRecords(expression, headers, pageSize.ToString(), page.ToString());
            int totalCount = (int)response["totalCount"];

            // Add first response to list
            records.AddRange(response["items"].ToObject<List<AprimoPublicLinkRecord>>());
            while ((pageSize * page) < totalCount)
            {
                page++;
                // Http Client disposes content after the request is sent, so the expression needs to be recreated after each request
                expression = JsonContent.Create(new
                {
                    searchExpression = new
                    {
                        expression = "LatestVersionOfMasterFile.HasPublicUri = true"
                    },
                    logRequest = "true"
                });
                // This process will need to be paged, so ensure we dont hit Aprimo rate limits
                Thread.Sleep(delayBetweenCalls);
                response = aprimoAPI.searchAprimoRecords(expression, headers, pageSize.ToString(), page.ToString());
                records.AddRange(response["items"].ToObject<List<AprimoPublicLinkRecord>>());
            }

            #endregion



            // Create csv at destination

            writeToLocalFile(records, destinationFolder + "/" + nameofCSV);


        }

        private static void writeToLocalFile(List<AprimoPublicLinkRecord> records, string filePath)
        {
            using (var writer = new StreamWriter(filePath)) // For Local Upload
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {

                    //Default delimiter is ','

                    csvWriter.WriteField("Record ID");
                    csvWriter.WriteField("Rendition Name");
                    csvWriter.WriteField("URI");
                    csvWriter.NextRecord();

                    // Add each item from Aprimo to the csv
                    foreach (AprimoPublicLinkRecord record in records)
                    {
                        // Assume record["masterFileLatestVersion"]["publicUris"]["items"][0] isnt null and there is only 1 item there
                        // NOTE: Searching is only available on the masterfile
                        record.masterFileLatestVersion.publicUris.items.ForEach(item =>
                        {
                            csvWriter.WriteField(record.id);
                            csvWriter.WriteField(item.renditionName);
                            csvWriter.WriteField(item.uri);
                            csvWriter.NextRecord();
                        });
                    }

                    writer.Flush();

                }
            }
        }

        private static bool writeToMemory(List<AprimoPublicLinkRecord> records, string filePath)
        {
            // Write the csv file into memory - this is used for uploading the CSV through the Aprimo REST API
            using (var mem = new MemoryStream())
            {
                using (var writer = new StreamWriter(mem))
                {
                    using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {

                        //Default delimiter is ','

                        csvWriter.WriteField("Record ID");
                        csvWriter.WriteField("Rendition Name");
                        csvWriter.WriteField("URI");
                        csvWriter.NextRecord();

                        // Add each item from Aprimo to the csv
                        foreach (AprimoPublicLinkRecord record in records)
                        {
                            // Assume record["masterFileLatestVersion"]["publicUris"]["items"][0] isnt null and there is only 1 item there
                            // NOTE: Searching is only available on the masterfile
                            csvWriter.WriteField(record.id);
                            csvWriter.WriteField(record.masterFileLatestVersion.publicUris.items[0].renditionName);
                            csvWriter.WriteField(record.masterFileLatestVersion.publicUris.items[0].uri);
                            csvWriter.NextRecord();
                            //Console.WriteLine(record["id"] + " | " + record["masterFileLatestVersion"]["publicUris"]["items"][0]["renditionName"] + " | " + record["masterFileLatestVersion"]["publicUris"]["items"][0]["uri"]);
                        }

                        writer.Flush();

                    }
                }
            }
            return true;
        }
    }
}
