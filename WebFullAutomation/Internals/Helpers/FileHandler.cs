// Ignore Spelling: api Xlsx

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using ExcelDataReader;
using log4net;
using log4net.Config;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public class FileHandler : IFileHandler
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private readonly IApiAccess _apiAccess;
        //private readonly ILog4Net _log4Net;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly string _crmUrl = Config.appSettings.CrmUrl;
        #endregion Members

        public FileHandler(IApplicationFactory apiFactory,
            IApiAccess apiAccess) //ILog4Net log4Net)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
            //_log4Net = log4Net;
        }

        public Stream GetFileStream(string fileName)
        {
            return File.OpenRead(fileName);
        }

        public IFileHandler DeleteFilesOlderThen(string path, int numOfDays)
        {
            Directory.GetFiles(path)
                .Select(f => new FileInfo(f))
                .Where(f => f.LastWriteTime < DateTime.Now.AddDays(numOfDays))
                .ToList()
                .ForEach(f => f?.Delete());

            return this;
        }

        public IFileHandler DeleteDirectoryOlderThen(string path, int numOfDays)
        {
            var directoryInfo = new DirectoryInfo(path);

            var recentDirectories = directoryInfo.GetDirectories()
                .Where(x => x != null && x.CreationTime < DateTime.Now.AddDays(numOfDays))
                .Select(x => x.FullName)
                .ToList();

            foreach (var dir in recentDirectories)
            {
                Directory.Delete(dir, true);
            }

            return this;
        }

        public string GetFileFromDownloadFolder(string fileName)
        {
            // driver.get("chrome://downloads/")

            //var manager = _driver.find_element_by_css_selector('body/deep/downloads-manager')
            // item = manager.find_element_by_css_selector('body/deep/downloads-item')
            // shadow = driver.execute_script('return arguments[0].shadowRoot;', item)
            // link = shadow.find_element_by_css_selector('div#title-area>a')

            // return link.get_attribute("href")

            return "";
        }

        public ByteArrayContent ConvertToBytesArray(string filePath,
            string mediaTypeHeaderValue = "image/png")
        {
            CheckIfFileExist(filePath);
            var fileStream = File.OpenRead(filePath);
            var streamContent = new StreamContent(fileStream);

            var fileContent = new ByteArrayContent(streamContent
                .ReadAsByteArrayAsync()
                .Result);

            fileContent.Headers.ContentType = MediaTypeHeaderValue
                .Parse(mediaTypeHeaderValue);

            return fileContent;
        }

        public IFileHandler CheckIfFileExist(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {

                //throw new FileNotFoundException($"File [{filePath}] not found.");
            }

            return this;
        }

        public MultipartFormDataContent UploadFilePipe(string filePath,
            string contentDispositionName, string ContentType)
        {
            CheckIfFileExist(filePath);
            var content = new MultipartFormDataContent();
            var memoryStream = new MemoryStream();
            var stream = GetFileStream(filePath);
            stream.CopyTo(memoryStream);
            var fileData = memoryStream.ToArray();
            var bytecontent = new ByteArrayContent(fileData);

            bytecontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = contentDispositionName,
                FileName = Path.GetFileName(filePath)
            };

            bytecontent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            content.Add(bytecontent);

            return content;
        }

        public string GetCsvFile(string url, string exportLink, string apiKey = null)
        {
            //_log4Net.Info("GetCsvFile");
            _apiKey = apiKey ?? _apiKey;
            exportLink = url + exportLink;
            var route = $"{exportLink}&api_key={_apiKey}";
            string json = null;

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (json == "\n" || json == "")
            {
                var exceMessage = "actualText: the file is empty";

                throw new Exception(exceMessage);
            }

            return json;
        }

        public IList<TResponse> ReadCSVFile<TResponse>(string fileString)
        {
            var headers = new List<string>();
            var dataRows = new List<dynamic>();
            IDictionary<string, object> rowDataAsDictionary = null;

            if (fileString == null)
            {
                throw new InvalidOperationException("CSV File is null");
            }

            List<TResponse> response;

            try
            {
                using (var reader = new StringReader(fileString))
                {
                    using var parser = new TextFieldParser(reader);
                    parser.Delimiters = new[] { "," };
                    parser.HasFieldsEnclosedInQuotes = true;
                    //parser.TrimWhiteSpace = true;

                    for (var rowIdx = 0; !parser.EndOfData; rowIdx++)
                    {
                        var colIdx = 0;
                        var rowData = new ExpandoObject();
                        rowDataAsDictionary = rowData;
                        var file = parser.ReadFields().ToList();
                        //var f = parser.ReadFields().AsEnumerable();

                        foreach (var field in parser.ReadFields().AsEnumerable())
                        {
                            if (rowIdx == 0)
                            {
                                // header
                                headers.Add(field.Replace(",", "_"));
                                rowDataAsDictionary.Add(file[colIdx], field);
                            }
                            else
                            {
                                if (field == "null" || field == "NULL")
                                {
                                    rowDataAsDictionary.Add(headers[colIdx], null);
                                }
                                else
                                {
                                    rowDataAsDictionary.Add(headers[colIdx], field);
                                }
                            }
                            colIdx++;
                        }

                        if (rowDataAsDictionary.Keys.Count > 0)
                        {
                            dataRows.Add(rowData);
                        }
                    }
                }

                var jsonString = JsonConvert.SerializeObject(dataRows);
                response = JsonConvert.DeserializeObject<List<TResponse>>(jsonString);
            }
            catch (Exception ex)
            {
                var exceMessage = $" Exception Message: {ex.Message}," +
                    $" row Data As Dictionary:{rowDataAsDictionary} ";

                throw new Exception(exceMessage);
            }

            return response;
        }

        public DataSet ConvertXlsxStreamToDataSet(Stream stream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //var dsexcelRecords = new DataSet();
            var dsexcelRecords = reader.AsDataSet();
            reader.Close();

            return dsexcelRecords;
        }

        public Dictionary<string, string> ReadDataFromDataSet(DataSet dataSet,
            string tableName)
        {
            try
            {
                var titleValues = new List<string>();
                var rowValues = new List<string>();

                dataSet.Tables[tableName]?.Rows[0].ItemArray
                    .ForEach(p => titleValues.Add(p.ToString()));

                dataSet.Tables[tableName]?.Rows[1].ItemArray
                    .ForEach(p => rowValues.Add(p.ToString()));

                return titleValues.Zip(rowValues, (k, v) => new { Key = k, Value = v })
                     .ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                var exceMessage = ($" Exception Message: {ex.Message}, request method: GET");

                throw new Exception(exceMessage);
            }
        }

        public List<Dictionary<string, string>> ReadDataFromDataSetNew(DataSet dataSet,
            string tableName)
        {

            var titleValues = new List<string>();
            var rowValues = new List<string>();
            var row = new Dictionary<string, string>();
            var rows = new List<Dictionary<string, string>>();
            var rowsValues = new List<Dictionary<string, string>>();

            dataSet.Tables[tableName]
                .AsEnumerable()
                .FirstOrDefault()
                .ItemArray.ForEach(p => titleValues.Add(p.ToString()));

            // run on all the rows
            for (var i = 0; i < dataSet.Tables[tableName].Rows.Count - 1; i++)
            {
                dataSet.Tables[tableName]
                        .Rows[i + 1].ItemArray
                        .AsEnumerable()
                        .ForEach(p => rowValues.Add(p.ToString()));

                row = new Dictionary<string, string>();

                // run on all the values in a row starting from row 1 because row 0 is titles
                for (var j = 0; j < titleValues.Count; j++)
                {
                    row.Add(titleValues[j], rowValues[j]);
                }

                rows.Add(row);
                rowValues.Clear();
            }

            return rows;
        }
    }
}