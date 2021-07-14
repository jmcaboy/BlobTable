using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;


namespace Hyeon.Function
{
    public static class GetJSONData
    {
        [FunctionName("GetJSONData")]
        public static string Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req, ILogger log, ExecutionContext context)

        {
            string connStrA = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);//Json으로 만든 Object
            string valueA = data.a;
            
            BlobServiceClient ClientA = new BlobServiceClient(connStrA);
            //serviceClient에서 컨테이너 가져오기
            BlobContainerClient containerA = ClientA.getBlobContainerClient("hyeoncon");
            //JSON같은 데이터 가져오기
            BlobClient blobA = containerA.getBlobClient(valueA + ".json");
            
            //text로 변환
            string responseA = "No Data";
            if(blobA.Exists())
            {   
                using (MemoryStream msA = new MemoryStream())
                {
                    blobA.DownloadTo(msA);
                    responseA = System.Text.Encoding.UTF8.GetString(msA.ToArray());
                }
            } 
            return valueA;
           
        }
    }
}
