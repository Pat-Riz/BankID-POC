using System.Net.Http.Headers;
using System.Text;

namespace Backend
{
    public class CustomStringContent : StringContent
    {
        public CustomStringContent(string content, Encoding encoding) : base(content, encoding)
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public CustomStringContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
        {

            Headers.ContentType = new MediaTypeHeaderValue(mediaType == null ? "application/json" : mediaType)
            {
                // disabled, will cause failure in the bank id apis
                //CharSet = encoding == null ? "" : encoding.WebName
            };

        }
    }
}