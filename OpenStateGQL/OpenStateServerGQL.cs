using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


namespace OpenStateGQL
{

    public class people
    {
        public people()
        {

        }
    }
    public class data
    {
        public data()
        {

        }
        public List<people> people;
    }
 
    public class OpenStateServerGQL
    {
 
        public OpenStateServerGQL()
        {

        }

        #region Properties
        static HttpClient client = null;
        #endregion

        public static void Init()
        {
            client = new HttpClient();

        }
        static async Task<Uri> AskAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        

    }
}
