// Ignore Spelling: api Xlsx

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public interface IFileHandler
    {
        IFileHandler CheckIfFileExist(string filePath);
        ByteArrayContent ConvertToBytesArray(string filePath, string mediaTypeHeaderValue = "image/png");
        DataSet ConvertXlsxStreamToDataSet(Stream stream);
        IFileHandler DeleteDirectoryOlderThen(string path, int numOfDays);
        IFileHandler DeleteFilesOlderThen(string path, int numOfDays);
        string GetCsvFile(string url, string exportLink, string apiKey = null);
        string GetFileFromDownloadFolder(string fileName);
        Stream GetFileStream(string fileName);
        IList<TResponse> ReadCSVFile<TResponse>(string fileString);
        Dictionary<string, string> ReadDataFromDataSet(DataSet dataSet, string tableName);
        List<Dictionary<string, string>> ReadDataFromDataSetNew(DataSet dataSet, string tableName);
        MultipartFormDataContent UploadFilePipe(string filePath, string contentDispositionName, string ContentType);
    }
}