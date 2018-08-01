using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace AnalisadorFotos.Services
{
    public class VisionAPI
    {
        private byte[] ImagemEmBytes(string imagem)
        {
            var webClient = new HttpClient();
            var stream = webClient.GetStreamAsync(imagem);

            BinaryReader binaryReader = new BinaryReader(stream.Result);
            return binaryReader.ReadBytes(1454000);
        }

        public async Task<string> FazAnalise(string imageFilePath)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "cafda6d865aa4b56bc5b2753ec52d241");

            string requestParameters = "visualFeatures=Categories,Tags,Description,Faces,ImageType,Color,Adult";

            string uri = @"https://brazilsouth.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;

            byte[] byteData = ImagemEmBytes(imageFilePath);


            HttpResponseMessage response;
            string responseContent;

            
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }



            return responseContent;
        }
    }
}
