using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome;
using System.Drawing;

namespace ConsoleApp3
{
    class Program
    {

        static async Task Main(string[] args)
        {

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => BuyProductAsync("inputmeail", "password", "https://www.staples.com/BIC-Xtra-Comfort-Round-Stic-Grip-Ballpoint-Pens-Medium-Point-Black-Dozen/product_382241", 4000));

            Task.WaitAll(tasks);
        }


        public static async Task BuyProductAsync(string email, string password, string product, int delay)
        {

            CookieContainer cookies = new CookieContainer();
            while (true)
            {
                var cookies1 = cookies.GetCookies(new Uri("https://www.staples.com"));
                foreach (Cookie co in cookies1)
                {
                    co.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                }
                //Check for stock!!!!!
                HttpWebRequest request11 = (HttpWebRequest)WebRequest.Create(product);
                request11.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";
                request11.CookieContainer = cookies;
                HttpWebResponse response11 = (HttpWebResponse)request11.GetResponse();
                StreamReader sr11 = new StreamReader(response11.GetResponseStream());

                var source11 = sr11.ReadToEnd();
                sr11.Close();

                if (source11.Contains("This item is out of stock") == true)
                {
                    Console.WriteLine("Out fo stock!");
                }

                else if (source11.Contains("Out fo stock!") == false)
                {
                    string sku = getBetween(source11, "sku", ",");
                    sku = sku.Substring(1);
                    sku = sku.Replace('"', '@');
                    sku = getBetween(sku, "@", "@");
                    ya(cookies, sku, email, password);
                    delay = 100000000;
                }

                Console.WriteLine("Waiting for Stock!");
                Thread.Sleep(delay);

            }






        }
        public async static void ya(CookieContainer cookies, string sku, string email, string password)
        {
            var stringPayload00 = "{\"username\":\"" + email + "\",\"password\":\"" + password + "\",\"rememberMe\":true,\"placement\":\"Login\",\"jsessionId\":\"\",\"flow\":\"login\",\"successRedirection\":null,\"reloadFlag\":true,\"slideIn\":true,\"userAgent\":\"Mozilla/5.0(WindowsNT10.0;Win64;x64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/104.0.0.0Safari/537.36\",\"stplSessionId\":\"\",\"requestUrl\":\"https://www.staples.com/\",\"ndsModeValue\":\"\",\"page\":1}";
            var httpContent00 = new System.Net.Http.StringContent(stringPayload00, Encoding.UTF8, "application/json");

            var httpClientHandler2 = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
                AllowAutoRedirect = false
            };

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            string jssion = "";
            string link = "";
            string token1 = "";
            using (var httpClient12 = new HttpClient(httpClientHandler2))
            {



                httpClient12.DefaultRequestHeaders.Add("referer", "https://www.staples.com/");
                // httpClient12.DefaultRequestHeaders.Add("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjI2ODc4NTUiLCJhcCI6IjUxNTY2NzExMSIsImlkIjoiOWIzM2I5YzJkY2Q2ODE5ZCIsInRyIjoiZTczMjlmNjQ2NDZkZGUzYjE5MmFiNzRjYzNlOGNjMzAiLCJ0aSI6MTY2MDgzMDE1MDk1OCwidGsiOiIyNDU3NTY1In19");
                httpClient12.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                //httpClient12.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

                ///Add to cart


                HttpResponseMessage lol = await httpClient12.PostAsync("https://www.staples.com/idm/api/identityProxy/logincommon", httpContent00);

                var responseContent31 = await lol.Content.ReadAsStringAsync();

                string rat = lol.Headers.ToString();
                responseContent31 = getBetween(responseContent31, "src", "width");
                responseContent31 = responseContent31.Replace('"', '@');
                link = getBetween(responseContent31, "@", "@");
                jssion = getBetween(lol.Headers.ToString(), "JSESSIONID=", ";");
                token1 = getBetween(await lol.Content.ReadAsStringAsync(), "nucaptcha-token", ">");
                token1 = token1 + "@";
                token1 = getBetween(token1, "value=", "@");
                token1 = token1.Replace('"', '@');
                token1 = getBetween(token1, "@", "@");
                token1 = token1.Remove(token1.Length - 3);
                Console.WriteLine(responseContent31);
                Console.WriteLine(lol.Headers);

                Console.WriteLine(jssion);
                Console.WriteLine(token1);

            }
            ///Login again
            ///

            var img = Bitmap.FromStream(new MemoryStream(new WebClient().DownloadData(link)));
            string base64ImageRepresentation = "";
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                base64ImageRepresentation = Convert.ToBase64String(ms.ToArray());
            }


            var parameters = new Dictionary<string, string> { { "body", base64ImageRepresentation } };
            parameters.Add("key", "37abc6d2d977f716d5aeecb6b760f2bf");
            parameters.Add("method", "base64");

            var stringPayload1 = await Task.Run(() => JsonConvert.SerializeObject(parameters));

            var httpContent1 = new System.Net.Http.StringContent(stringPayload1, Encoding.UTF8, "application/json");

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class

            string id = "";
            string answer = "";
            using (var httpClient12 = new HttpClient())
            {




                // httpClient12.DefaultRequestHeaders.Add("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjI2ODc4NTUiLCJhcCI6IjUxNTY2NzExMSIsImlkIjoiOWIzM2I5YzJkY2Q2ODE5ZCIsInRyIjoiZTczMjlmNjQ2NDZkZGUzYjE5MmFiNzRjYzNlOGNjMzAiLCJ0aSI6MTY2MDgzMDE1MDk1OCwidGsiOiIyNDU3NTY1In19");
                httpClient12.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                //httpClient12.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

                ///Add to cart


                HttpResponseMessage lol = await httpClient12.PostAsync("http://2captcha.com/in.php", httpContent1);

                var responseContent31 = await lol.Content.ReadAsStringAsync();
                id = responseContent31.Substring(3);
                string rat = lol.Headers.ToString();

                Console.WriteLine(responseContent31);
                Console.WriteLine(lol.Headers);


            }
            while (true)
            {
                HttpWebRequest request12 = (HttpWebRequest)WebRequest.Create("https://2captcha.com/res.php?key=37abc6d2d977f716d5aeecb6b760f2bf&action=get&id=" + id);
                request12.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36";
                HttpWebResponse response121 = (HttpWebResponse)request12.GetResponse();
                StreamReader sr12 = new StreamReader(response121.GetResponseStream());

                var source12 = sr12.ReadToEnd();
                sr12.Close();



                if (source12.Contains("OK") == true)
                {

                    answer = source12;
                    answer = answer.Remove(0, 3);
                    Console.WriteLine(answer);

                    break;



                }
                Console.WriteLine(source12);
                Thread.Sleep(2000);
            }

            string yo = "{\"username\":\"" + email + "\",\"password\":\"" + password + "\",\"rememberMe\":true,\"placement\":\"Login\",\"jsessionId\":\"" + jssion + "\",\"flow\":\"login\",\"successRedirection\":null,\"reloadFlag\":true,\"slideIn\":true,\"userAgent\":\"Mozilla/5.0(WindowsNT10.0;Win64;x64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/104.0.0.0Safari/537.36\",\"stplSessionId\":\"" + jssion + "\",\"requestUrl\":\"https://www.staples.com/\",\"ndsModeValue\":\"{\\\"jvqtrgQngn\\\":{\\\"oq\\\":\\\"951:969:1920:1040:1920:1040\\\",\\\"wfi\\\":\\\"flap-152535\\\",\\\"oc\\\":\\\"2501pp0s72219oop\\\",\\\"fe\\\":\\\"1080k192024\\\",\\\"qvqgm\\\":\\\"300\\\",\\\"jxe\\\":590544,\\\"syi\\\":\\\"snyfr\\\",\\\"si\\\":\\\"si,btt,zc4,jroz\\\",\\\"sn\\\":\\\"sn,zcrt,btt,jni\\\",\\\"us\\\":\\\"22rqp3sponq8492q\\\",\\\"cy\\\":\\\"Jva32\\\",\\\"sg\\\":\\\"{\\\\\\\"zgc\\\\\\\":0,\\\\\\\"gf\\\\\\\":snyfr,\\\\\\\"gr\\\\\\\":snyfr}\\\",\\\"sp\\\":\\\"{\\\\\\\"gp\\\\\\\":gehr,\\\\\\\"ap\\\\\\\":gehr}\\\",\\\"sf\\\":\\\"gehr\\\",\\\"jt\\\":\\\"s2nno0056qp62o71\\\",\\\"sz\\\":\\\"qq24746n55316rn4\\\",\\\"vce\\\":\\\"apvc,-1,6303pp64,2,1;fg,0,frnepuVachg,0,hfreanzr,18,cnffjbeq,0;zz,18,339,95,;zzf,3rs,0,n,237r,9o203o,ns7,o0s,-s8np,qs15,-704;zp,15,287,oo,cnffjbeq;xq,23n;xq,19q;xh,6n;xh,0;xq,84;xh,59;xq,q3;xq,9;xh,1o;xh,29;xq,8q;zzf,16,6s6,n,ABC;xq,54;xh,1n;xh,16;xq,n1;xh,53;xq,26;xh,3p;xq,7r;xh,4q;xq,6q;xh,62;xq,7s;zzf,6,3s9,n,ABC;xh,40;xq,74;xh,44;xq,po;xq,99;xh,41;xh,q;zzf,139,3r3,n,22poqs9,22poqs9,3p0,699,-2867p,q50p,-2o58;zzf,3r9,3r9,n,ABC;zzf,3r6,3r6,n,ABC;zzf,3r9,3r9,n,ABC;zz,371,3o6,118,;zzf,76,3r7,n,10o20,92p31pr,6op,77o,-1o33s,19s1r,-203;zp,127,305,122,ybtvaOga;zzf,2p2,3r9,n,26792,18401q4,499,4o2,-80s2,s5n9,-rr8;fg,100,ahpncgpun-nafjre,0,frnepuVachg,0,hfreanzr,18,cnffjbeq,12;xx,3,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;so,2o4,ahpncgpun-nafjre;zzf,30,3r7,n,ABC;zzf,2so5,2so5,32,ABC;gf,0,59sp;zz,866,3o6,14q,;xx,1n8,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;so,5,ahpncgpun-nafjre;zzf,5q3p,674s,32,5913s5,81s14oo,111,ss9,-83qn,57sr,-31p;gf,0,p14o;zzf,o794,o794,32,ABC;gf,0,178qs;zzf,p34r,p34r,32,ABC;gf,0,23p2q;zzf,p351,p351,32,ABC;gf,0,2ss7r;zzf,p34o,p34o,32,ABC;gf,0,3p2p9;zzf,rn65,rn65,1r,ABC;gf,0,4nq2r;zzf,rn5s,rn5s,1r,ABC;gf,0,5978q;zzf,2ns87,2ns87,1r,ABC;gf,0,84714;zzf,rn5s,rn5s,1r,ABC;gf,0,93173;zzf,rr43,rr43,1r,ABC;gf,0,n1so6;zz,23p93,327,3no,;gf,0,p5p49;zz,6os27,3n2,324,;gf,0,131o70;zz,154rr,3o6,1os,;gf,0,14705r;zz,28p3q,337,3p8,;gf,0,16sp9o;zz,r41r9,3o3,173,;gf,0,253r84;xx,1qr,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;zp,4p,2pr,1oq,ahpncgpun-nafjre;xq,3no;xq,q7;xh,7p;xq,15p;xh,7p;xq,70;xh,71;xh,7n;xq,235;xq,206;xq,1o;xq,1q;xh,67;xh,17;zz,rr,368,1s6,qbgpbz_ybtvasbez_purpxobk;so,3np,ahpncgpun-nafjre;xx,4p2,4,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;so,6,ahpncgpun-nafjre;zp,5s,2qp,22r,ybtvaOga;zz,9p79,3o5,2rp,;gf,0,25s40n;zz,3649,3o1,165,;zz,4qpp,3o6,rp,;gf,0,26781s;zp,1q1,27p,153,cnffjbeq;xq,35n;xq,139;xh,2n;xh,22;xq,7s;xh,3p;xq,5s;xh,4p;xq,112;xh,3n;xq,12q;xq,202;xq,20;xq,1q;xq,21;xq,1q;xq,22;xq,1q;xq,30;xq,22;xh,7;xq,80;xq,10q;xh,15;xh,37;xq,6s;xh,4o;xq,838;xh,41;xq,5r;xh,46;xq,5n;xh,49;xq,r8;xq,q8;xh,46;xh,15;xq,n1;xh,3r;xq,pn;xh,52;xq,9;xh,4o;xq,7s;xq,60;xh,22;xh,24;xq,n6;xh,53;xq,7;xh,46;xq,107;xh,4q;xq,70;xh,3r;xq,77;xh,4o;xq,p4;zz,3r,27q,153,cnffjbeq;xq,42;xh,54;xh,5;zz,1105,2o0,1oq,ybtvaOga;zp,s4,2no,1o5,;zz,259n,3o3,r3,;gf,0,26q4qs;zp,293,261,12s,;zp,31s,28p,12n,cnffjbeq;xq,8r;xq,4;xq,8;xh,5n;xh,r;xh,r;xq,40;xq,9;xq,12;xh,45;xh,6;xh,s;xq,37;xq,9;xq,7;xh,51;xh,7;xh,9;zp,p7,2p2,18o,ybtvaOga;fg,208,ahpncgpun-nafjre,0,frnepuVachg,0,hfreanzr,18,cnffjbeq,9;xx,2,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;so,38q,ahpncgpun-nafjre;zz,4569,3o2,17n,;gf,0,2728p5;xx,758,0,ahpncgpun-nafjre;ss,1,ahpncgpun-nafjre;zp,43,2o7,1o6,ahpncgpun-nafjre;xq,16q;xh,n9;xq,7n;xq,1o;xq,s;xh,51;xh,8;xh,6;xq,63;xq,1p;xh,3q;xh,r;so,9r,ahpncgpun-nafjre;zp,5s,2po,225,;fg,47q,ahpncgpun-nafjre,0,frnepuVachg,0,hfreanzr,18,cnffjbeq,9;zz,2o914,362,3nn,;gf,0,29s2q2;zz,12s3,305,19n,ahpncgpun-cynlre;xx,112,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;zp,3s,2nn,1p8,ahpncgpun-nafjre;xq,22q;xq,1sr;xq,1s;xq,1s;xq,20;xq,1s;xq,2q;xq,1s;xq,20;xq,1s;xq,11;xh,75;xh,11;xq,3o3;xq,17n;xh,50;xq,154;xh,55;xq,16r;xh,69;xh,2s;so,4qo,ahpncgpun-nafjre;zz,13o,3o1,19r,;xx,195,4,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;so,8,ahpncgpun-nafjre;zp,58,2q6,22p,;zz,r2n3,32s,3p7,;gf,0,2o00on;zp,nr5,2n4,150,cnffjbeq;xq,308;xq,7s;xh,53;xh,q;xq,6r;xh,3r;xq,o0;xq,45;xh,8;xh,35;xq,95;xq,58;xh,29;xh,1p;xq,n1;xh,52;xq,16;xh,47;xq,26r;xh,52;xq,78;xh,41;xq,7r;xh,40;zz,op,2n5,151,cnffjbeq;xq,24;xq,n0;xh,5s;xh,0;zp,596,2pn,1np,;fg,217,ahpncgpun-nafjre,0,frnepuVachg,0,hfreanzr,18,cnffjbeq,11;xx,3,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;so,70s,ahpncgpun-nafjre;zz,201,30r,195,ahpncgpun-cynlre;xx,16s,0,ahpncgpun-nafjre;ss,0,ahpncgpun-nafjre;zp,4p,2or,1p5,ahpncgpun-nafjre;xq,1n9;xq,20p;xh,19;xq,1r1;xh,5q;xq,5sp;xh,61;xq,r5;xh,3q;xh,47;so,395,ahpncgpun-nafjre;gf,0,2o3qqr;\\\",\\\"ns\\\":\\\"\\\",\\\"qvg\\\":\\\"\\\",\\\"vp\\\":\\\"\\\",\\\"ji\\\":\\\"\\\"},\\\"jg\\\":\\\"\\\"}\",\"page\":1,\"captchaAnswer\":\"" + answer + "\",\"nuCaptchaToken\":\"" + token1 + ",|0|VIDEO|5||0|0" + "\"}";
            var httpContent001 = new System.Net.Http.StringContent(yo, Encoding.UTF8, "application/json");

            var httpClientHandler21 = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true
            };

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class


            using (var httpClient12 = new HttpClient(httpClientHandler21))
            {



                httpClient12.DefaultRequestHeaders.Add("referer", "https://www.staples.com/");
                // httpClient12.DefaultRequestHeaders.Add("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjI2ODc4NTUiLCJhcCI6IjUxNTY2NzExMSIsImlkIjoiOWIzM2I5YzJkY2Q2ODE5ZCIsInRyIjoiZTczMjlmNjQ2NDZkZGUzYjE5MmFiNzRjYzNlOGNjMzAiLCJ0aSI6MTY2MDgzMDE1MDk1OCwidGsiOiIyNDU3NTY1In19");
                httpClient12.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                //httpClient12.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

                ///Add to cart


                HttpResponseMessage lol = await httpClient12.PostAsync("https://www.staples.com/idm/api/identityProxy/logincommon", httpContent001);

                var responseContent31 = await lol.Content.ReadAsStringAsync();


                Console.WriteLine(responseContent31);

                Console.WriteLine(lol.Headers);
            }

            //  Thread.Sleep(2000000);


            // string stringPayload01 = "{\"items\":[{\"itemId\":\"24381063\",\"qty\":1}],\"yourStoreNo\":\"\"}";
            //var httpContent01 = new System.Net.Http.StringContent(stringPayload01, Encoding.UTF8, "application/json");

            var stringPayload0011 = "{\"items\":[{\"itemId\":\"" + sku + "\",\"qty\":1,\"deliveryType\":\"STA\"}],\"yourStoreNo\":\"" + "" + "\"}";
            var httpContent0011 = new System.Net.Http.StringContent(stringPayload0011, Encoding.UTF8, "application/json");

            var httpClientHandler211 = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
                AllowAutoRedirect = false
            };

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class

            string cartid1 = "";

            using (var httpClient1211 = new HttpClient(httpClientHandler211))
            {



                httpClient1211.DefaultRequestHeaders.Add("referer", "https://www.staples.com/");
                // httpClient12.DefaultRequestHeaders.Add("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjI2ODc4NTUiLCJhcCI6IjUxNTY2NzExMSIsImlkIjoiOWIzM2I5YzJkY2Q2ODE5ZCIsInRyIjoiZTczMjlmNjQ2NDZkZGUzYjE5MmFiNzRjYzNlOGNjMzAiLCJ0aSI6MTY2MDgzMDE1MDk1OCwidGsiOiIyNDU3NTY1In19");
                httpClient1211.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                httpClient1211.DefaultRequestHeaders.Add("request-id", "|d29ea4f06d6049d48b833fea6284ad5b.b961316ec98d4b2f");
                //httpClient12.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

                ///Add to cart


                HttpResponseMessage lol11 = await httpClient1211.PostAsync("https://www.staples.com/cc/api/checkout/default/item?mmx=true&responseType=WHOLE&dataEncode=false&buyNow=true&skipInv=false&fromCC=true&ux2=true&memItem=true", httpContent0011);

                var responseContent3111 = await lol11.Content.ReadAsStringAsync();
                cartid1 = responseContent3111;
                cartid1 = getBetween(cartid1, "cartId", ",");
                cartid1 = cartid1.Substring(1);
                cartid1 = cartid1.Replace('"', '@');
                cartid1 = getBetween(cartid1, "@", "@");
                Console.WriteLine(responseContent3111);

            }





            string html = string.Empty;
            string url = "https://www.staples.com/cc/sparq/cart";

            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            _request.AutomaticDecompression = DecompressionMethods.GZip;
            _request.AllowAutoRedirect = false;
            _request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";
            _request.CookieContainer = cookies;
            string secret = "";

            using (HttpWebResponse _response = (HttpWebResponse)_request.GetResponse())
            using (Stream stream = _response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {


                html = reader.ReadToEnd();








                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class


                var stringPayload009 = "{\"msg\":{\"status\":\"REGULAR_SUBMIT_ORDER\",\"cartId\":\"" + cartid1 + "\",\"nd3DSAuthRequired\":false},\"x-nds-pmd\":\"%7B%22jvqtrgQngn%22:%7B%22oq%22:%22951:969:1920:1040:1920:1040%22,%22wfi%22:%22flap-152535%22,%22oc%22:%222501pp0s72219oop%22,%22fe%22:%221080k1920%2024%22,%22qvqgm%22:%22300%22,%22jxe%22:772649,%22syi%22:%22snyfr%22,%22si%22:%22si,btt,zc4,jroz%22,%22sn%22:%22sn,zcrt,btt,jni%22,%22us%22:%2222rqp3sponq8492q%22,%22cy%22:%22Jva32%22,%22sg%22:%22%7B%5C%22zgc%5C%22:0,%5C%22gf%5C%22:snyfr,%5C%22gr%5C%22:snyfr%7D%22,%22sp%22:%22%7B%5C%22gp%5C%22:gehr,%5C%22ap%5C%22:gehr%7D%22,%22sf%22:%22gehr%22,%22jt%22:%22s2nno0056qp62o71%22,%22sz%22:%22qq24746n55316rn4%22,%22vce%22:%22apvc,0,630522s4,2,1;fg,0,frpPbqr,0;ss,0,frpPbqr;xq,36r;xq,q;xh,70;xh,1s;xq,151;xh,7n;zz,203,189,261,frpPbqr;zzf,46p,0,n,34%20226,491s%201243,on9,oop,-2s949,17110,-3n4;zzf,432,432,n,ABC;zzf,3r2,3r2,n,ABC;zzf,3r8,3r8,n,1388%203982,1388%203982,613,613,25s58,25s58,3pop;%22,%22ns%22:%22%22,%22qvg%22:%22%22,%22vp%22:%22%22,%22ji%22:%22%22%7D,%22jg%22:%22%22%7D\"}";
                var httpContent009 = new System.Net.Http.StringContent(stringPayload009, Encoding.UTF8, "application/json");

                var httpClientHandler29 = new HttpClientHandler
                {
                    CookieContainer = cookies,
                    UseCookies = true
                };
                using (var httpClient129 = new HttpClient(httpClientHandler29))
                {



                    httpClient129.DefaultRequestHeaders.Add("referer", "https://www.staples.com/cc/sparq/cart/");
                    // httpClient12.DefaultRequestHeaders.Add("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjI2ODc4NTUiLCJhcCI6IjUxNTY2NzExMSIsImlkIjoiOWIzM2I5YzJkY2Q2ODE5ZCIsInRyIjoiZTczMjlmNjQ2NDZkZGUzYjE5MmFiNzRjYzNlOGNjMzAiLCJ0aSI6MTY2MDgzMDE1MDk1OCwidGsiOiIyNDU3NTY1In19");
                    httpClient129.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                    //httpClient12.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

                    ///Add to cart


                    HttpResponseMessage lol9 = await httpClient129.PostAsync("https://www.staples.com/cc/api/log", httpContent009);

                    var responseContent319 = await lol9.Content.ReadAsStringAsync();

                    Console.WriteLine(responseContent319);


                    Console.WriteLine(lol9.Headers);
                }



                ///Final checkout
                var stringPayload001 = "inputcardinfo";
                var httpContent00111 = new System.Net.Http.StringContent(stringPayload001, Encoding.UTF8, "application/json");

                var httpClientHandler2111 = new HttpClientHandler
                {
                    CookieContainer = cookies,
                    UseCookies = true

                };

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                string cartid = "";
                using (var httpClient124 = new HttpClient(httpClientHandler2111))
                {



                    httpClient124.DefaultRequestHeaders.Add("referer", "https://www.staples.com/cc/sparq/cart/");
                    // httpClient12.DefaultRequestHeaders.Add("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjI2ODc4NTUiLCJhcCI6IjUxNTY2NzExMSIsImlkIjoiOWIzM2I5YzJkY2Q2ODE5ZCIsInRyIjoiZTczMjlmNjQ2NDZkZGUzYjE5MmFiNzRjYzNlOGNjMzAiLCJ0aSI6MTY2MDgzMDE1MDk1OCwidGsiOiIyNDU3NTY1In19");
                    httpClient124.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                    httpClient124.DefaultRequestHeaders.Add("x-payment-auth-token", "Dr0gvVeQTVDf2xUOAJp0549ar7QMqSHh0UzsVz8OJ6nc2GgcxOY2OCC8-4UpfkCOMAmdGqziVQKinqI7vuTMku5ZvBOo2qwwYsJ4hAEP8N9IpcT12_rLj9LbBOaG9hs4bIU7spj8LsS2PPCQGyCltSrBSvBvLs-J_8sxyA5ZWFJGosU61T4oM7j1U8lXocV_g_omRxr2d4ydEjxRDA1SmByXtgRPzTjvLsUVzWk6sP92aVPaLeJSYcUYf1yixkh48lWGALIO5WmzqFZzlhhyMw==");
                    //httpClient12.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    httpClient124.DefaultRequestHeaders.Add("request-id", "|d29ea4f06d6049d48b833fea6284ad5b.b961316ec98d4b2f");
                    ///Add to cart


                    HttpResponseMessage lol2 = await httpClient124.PostAsync("https://www.staples.com/cc/api/checkout/default/order?tenantId=StaplesDotCom&locale=en-US&channel=WEB&responseType=WHOLE&ux2=true&mmx=true&buyNow=true&memItem=true&updatePayment=N&ean=N&textSub=true", httpContent00111);

                    var responseContent312 = await lol2.Content.ReadAsStringAsync();

                    Console.WriteLine(responseContent312);
                    Console.WriteLine(lol2.Headers);
                    Console.WriteLine(cartid);

                    Console.WriteLine("DONE!!!!");
                    Thread.Sleep(1000000);



                }








            }
        }
        static void doStuff()
        {



            string line1 = File.ReadLines("MyFile.txt").First(); // gets the first line from file.
            string strFile = File.ReadAllText("MyFile.txt");
            strFile = Regex.Replace(strFile, line1, "");

            File.WriteAllText("MyFile.txt", strFile);


            string line11 = File.ReadLines("MyFile.txt").First();
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public void encryption()
        {
            try
            {
                string random = "";





            }
            catch (Exception e)
            {
                //checks for any error during encrypt process 
                Console.WriteLine(e);
            }
        }
        public static string Base64Encode(string plaingtext)
        {
            var plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaingtext);
            return System.Convert.ToBase64String(plaintextBytes);
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);

            }
            return "";

        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        

        public static double GetCurrentMilli()
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaspan = DateTime.UtcNow - Jan1970;
            return javaspan.TotalMilliseconds;
        }

    }
}