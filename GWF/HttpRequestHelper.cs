using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    public class HttpRequestHelper
    {
        private bool _ApplicationMode = false;
        private int _InternalBufferSize = 4096;
        private string _UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
        private string _Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*";
        private string _Refer = string.Empty;
        private HttpStatusCode _LastStatusCode = HttpStatusCode.OK;

        /// <summary>
        /// 응용프로그램 모드 여부(true이면 DoEvent로 메시지 펌프 처리)
        /// </summary>
        public bool ApplicationMode
        {
            set { this._ApplicationMode = value; }
            get { return this._ApplicationMode; }
        }

        /// <summary>
        /// 내부 버퍼 사이즈
        /// </summary>
        public int InternalBufferSize
        {
            set
            {
                if (value < 1024)
                    this._InternalBufferSize = 1024;
                else
                    this._InternalBufferSize = value;
            }

            get
            {
                return this._InternalBufferSize;
            }
        }

        /// <summary>
        /// 사용자 Agent
        /// </summary>
        public string UserAgent
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this._UserAgent = value;
            }

            get
            {
                return this._UserAgent;
            }
        }

        /// <summary>
        /// 허용 Accept 문자열
        /// </summary>
        public string Accept
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this._Accept = value;
            }

            get
            {
                return this._Accept;
            }
        }

        /// <summary>
        /// 참조 경로(URL)
        /// </summary>
        public string Refer
        {
            set
            {
                this._Refer = value;
            }

            get
            {
                return this._Refer;
            }
        }

        /// <summary>
        /// HTTP 결과 코드
        /// </summary>
        public HttpStatusCode LastStatusCode
        {
            get { return this._LastStatusCode; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        public HttpRequestHelper()
        {
        }

        /// <summary>
        /// 컨텐츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <returns>결과 내용</returns>
        public string GetContentsFromURL(string url)
        {
            return GetContentsFromURL(url, null, null, null, null, null);
        }

        /// <summary>
        /// 컨텐츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <returns>결과 내용</returns>
        public string GetContentsFromURL(string url, string postdata)
        {
            return GetContentsFromURL(url, null, postdata, null, null, null);
        }

        /// <summary>
        /// 컨텐츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="postencoding">요청하는 postcontent의 인코딩 처리(null일 경우 UTF8)</param>
        /// <returns>결과 내용</returns>
        public string GetContentsFromURL(string url, string postdata, Encoding postencoding)
        {
            return GetContentsFromURL(url, null, postdata, postencoding, null, null);
        }

        /// <summary>
        /// 컨텐츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="postencoding">요청하는 postcontent의 인코딩 처리(null일 경우 UTF8)</param>
        /// <param name="fetchencoding">받아오는 컨텐츠 문자열 인코딩(생량식 사이트의 ContentEncoding에 의존)</param>
        /// <returns>결과 내용</returns>
        public string GetContentsFromURL(string url, string postdata, Encoding postencoding, Encoding fetchencoding)
        {
            return GetContentsFromURL(url, null, postdata, postencoding, fetchencoding, null);
        }

        /// <summary>
        /// 컨텐츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="postencoding">요청하는 postcontent의 인코딩 처리(null일 경우 UTF8)</param>
        /// <param name="fetchencoding">받아오는 컨텐츠 문자열 인코딩(생량식 사이트의 ContentEncoding에 의존)</param>
        /// <param name="fetchencoding">추가할 헤더값</param>
        /// <returns>결과 내용</returns>
        public string GetContentsFromURL(string url, string postdata, Encoding postencoding, Encoding fetchencoding, NameValueCollection headers)
        {
            return GetContentsFromURL(url, null, postdata, postencoding, fetchencoding, headers);
        }

        /// <summary>
        /// 컨텐츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="cookies">요청에 포함될 쿠키 콜렉션</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="postencoding">요청하는 postcontent의 인코딩 처리(null일 경우 HextoByte로 처리)</param>
        /// <param name="fetchencoding">받아오는 컨텐츠 문자열 인코딩(생량식 사이트의 ContentEncoding에 의존)</param>
        /// <returns>결과 내용</returns>
        public string GetContentsFromURL(string url, CookieCollection cookies, string postdata, Encoding postencoding, Encoding fetchencoding, NameValueCollection headers)
        {
            StringBuilder sb = new StringBuilder(this._InternalBufferSize);

            try
            {
                this._LastStatusCode = HttpStatusCode.Unused;
                Encoding enc;

                HttpWebRequest webRequest = null;
                byte[] buff = new byte[this._InternalBufferSize];

                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.UserAgent = this._UserAgent;
                webRequest.Accept = this._Accept;

                if (headers != null && headers.Count > 0)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        webRequest.Headers.Add(key, headers[key]);
                    }
                }

                if (!string.IsNullOrEmpty(this._Refer))
                {
                    webRequest.Referer = webRequest.RequestUri.Scheme + "://" + webRequest.RequestUri.Host;
                }

                webRequest.CookieContainer = new CookieContainer();
                if (cookies != null && cookies.Count > 0)
                {
                    webRequest.CookieContainer.Add(cookies);
                }

                if (!string.IsNullOrEmpty(postdata))
                {
                    webRequest.Method = "POST";
                    byte[] senddata;

                    if (postencoding == null)
                    {
                        webRequest.ContentType = "application/x-www-form-urlencoded;";
                        senddata = SecurityHelper.HexToByte(postdata);
                    }
                    else
                    {
                        enc = postencoding;
                        webRequest.ContentType = "application/x-www-form-urlencoded; charset=" + enc.WebName;
                        senddata = enc.GetBytes(postdata);
                    }

                    webRequest.ContentLength = senddata.Length;

                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(senddata, 0, senddata.Length);
                        dataStream.Close();
                    }
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    this._LastStatusCode = webResponse.StatusCode;
                    if (fetchencoding == null)
                    {
                        if (!string.IsNullOrEmpty(webResponse.CharacterSet))
                        {
                            try
                            {
                                switch (webResponse.CharacterSet.ToLower())
                                {
                                    case "utf-8": enc = Encoding.UTF8; break;
                                    case "euc-kr": enc = Encoding.GetEncoding("euc-kr"); break;
                                    default: enc = string.IsNullOrEmpty(webResponse.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(webResponse.CharacterSet); break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4netHelper.Warn(ref ex);
                                enc = Encoding.UTF8;
                            }
                        }
                        else
                        {
                            enc = Encoding.Default;
                        }
                    }
                    else
                    {
                        enc = fetchencoding;
                    }

                    using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), enc))
                    {
                        while (streamReader.Peek() > 0)
                        {
                            sb.Append(streamReader.ReadLine());
                        }

                        streamReader.Close();
                    }
                }

                return sb.ToString();
            }
            catch (WebException webex)
            {
                this._LastStatusCode = HttpStatusCode.ServiceUnavailable;

                using (StreamReader streamReader = new StreamReader(webex.Response.GetResponseStream(), Encoding.UTF8))
                {
                    while (streamReader.Peek() > 0)
                    {
                        sb.Append(streamReader.ReadLine());
                    }

                    streamReader.Close();
                }
                Log4netHelper.Error(ref webex);

                return sb.ToString();
            }
        }

        public string GetContentsFromURL(string url, byte[] postdata)
        {
            StringBuilder sb = new StringBuilder(this._InternalBufferSize);

            try
            {
                this._LastStatusCode = HttpStatusCode.Unused;

                HttpWebRequest webRequest = null;
                byte[] buff = new byte[this._InternalBufferSize];

                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.UserAgent = this._UserAgent;
                webRequest.Accept = this._Accept;

                if (!string.IsNullOrEmpty(this._Refer))
                {
                    webRequest.Referer = webRequest.RequestUri.Scheme + "://" + webRequest.RequestUri.Host;
                }

                if (postdata != null)
                {
                    webRequest.Method = "POST";
                    webRequest.ContentLength = postdata.Length;
                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(postdata, 0, postdata.Length);
                        dataStream.Close();
                    }
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    this._LastStatusCode = webResponse.StatusCode;
                    using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        while (streamReader.Peek() > 0)
                        {
                            sb.Append(streamReader.ReadLine());
                        }

                        streamReader.Close();
                    }

                }

                return sb.ToString();
            }
            catch (WebException webex)
            {
                this._LastStatusCode = HttpStatusCode.ServiceUnavailable;

                using (StreamReader streamReader = new StreamReader(webex.Response.GetResponseStream(), Encoding.UTF8))
                {
                    while (streamReader.Peek() > 0)
                    {
                        sb.Append(streamReader.ReadLine());
                    }

                    streamReader.Close();
                }
                Log4netHelper.Error(ref webex);

                return sb.ToString();
            }
        }


        /// <summary>
        /// 컨텍츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(Json)</param>
        /// <param name="headers">추가할 헤더값</param>
        /// <returns></returns>
        public string GetContentsFromURL(string url, dynamic postdata, NameValueCollection headers)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.UserAgent = this._UserAgent;
            webRequest.Accept = this._Accept;

            if (headers != null && headers.Count > 0)
            {
                foreach (string key in headers.AllKeys)
                {
                    webRequest.Headers.Add(key, headers[key]);
                }
            }

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            byte[] senddata = Encoding.UTF8.GetBytes(GWF.JsonHelper.Serialize(postdata));
            webRequest.ContentLength = senddata.Length;

            using (Stream dataStream = webRequest.GetRequestStream())
            {
                dataStream.Write(senddata, 0, senddata.Length);
                dataStream.Close();
            }

            StringBuilder sb = new StringBuilder();
            using (var webResponse = webRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    while (streamReader.Peek() > 0)
                    {
                        sb.Append(streamReader.ReadLine());
                    }

                    streamReader.Close();
                }
            }

            return sb.ToString(); ;

        }

        /// <summary>
        /// 컨텍츠 문자열 가져오기
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(Json)</param>
        /// <param name="headers">추가할 헤더값</param>
        /// <returns></returns>
        public async Task<string> GetContentsFromURLAsync(string url, dynamic postdata, NameValueCollection headers)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.UserAgent = this._UserAgent;
            webRequest.Accept = this._Accept;

            if (headers != null && headers.Count > 0)
            {
                foreach (string key in headers.AllKeys)
                {
                    webRequest.Headers.Add(key, headers[key]);
                }
            }

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            byte[] senddata = Encoding.UTF8.GetBytes(GWF.JsonHelper.Serialize(postdata));
            webRequest.ContentLength = senddata.Length;

            using (Stream dataStream = webRequest.GetRequestStream())
            {
                dataStream.Write(senddata, 0, senddata.Length);
                dataStream.Close();
            }

            StringBuilder sb = new StringBuilder();
            using (var webResponse = await webRequest.GetResponseAsync())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    while (streamReader.Peek() > 0)
                    {
                        sb.Append(streamReader.ReadLine());
                    }

                    streamReader.Close();
                }
            }

            return sb.ToString(); ;

        }

        /// <summary>
        /// 파일을 다운로드
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="FileFullPathToSave">저장할 경로 및 파일명</param>
        /// <param name="FailIfExist">저장할 경로의 파일이 존재할 경우 실패 여부</param>
        /// <returns>성공하면 true</returns>
        public bool DownloadFileFromURL(string url, string FileFullPathToSave, bool FailIfExist)
        {
            return DownloadFileFromURL(url, null, null, null, FileFullPathToSave, FailIfExist);
        }

        /// <summary>
        /// 파일을 다운로드
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postcontent">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="FileFullPathToSave">저장할 경로 및 파일명</param>
        /// <param name="FailIfExist">저장할 경로의 파일이 존재할 경우 실패 여부</param>
        /// <returns>성공하면 true</returns>
        public bool DownloadFileFromURL(string url, string postcontent, string FileFullPathToSave, bool FailIfExist)
        {
            return DownloadFileFromURL(url, null, postcontent, null, FileFullPathToSave, FailIfExist);
        }

        /// <summary>
        /// 파일을 다운로드
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="postcontent">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="postencoding">요청하는 postcontent의 인코딩 처리(null일 경우 UTF8)</param>
        /// <param name="FileFullPathToSave">저장할 경로 및 파일명</param>
        /// <param name="FailIfExist">저장할 경로의 파일이 존재할 경우 실패 여부</param>
        /// <returns>성공하면 true</returns>
        public bool DownloadFileFromURL(string url, string postcontent, Encoding postencoding, string FileFullPathToSave, bool FailIfExist)
        {
            return DownloadFileFromURL(url, null, postcontent, postencoding, FileFullPathToSave, FailIfExist);
        }

        /// <summary>
        /// 파일을 다운로드
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="cookies">요청에 포함될 쿠키 콜렉션</param>
        /// <param name="postcontent">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="postencoding">요청하는 postcontent의 인코딩 처리(null일 경우 UTF8)</param>
        /// <param name="FileFullPathToSave">저장할 경로 및 파일명</param>
        /// <param name="FailIfExist">저장할 경로의 파일이 존재할 경우 실패 여부</param>
        /// <returns>성공하면 true</returns>
        public bool DownloadFileFromURL(string url, CookieCollection cookies, string postcontent, Encoding postencoding, string FileFullPathToSave, bool FailIfExist)
        {
            this._LastStatusCode = HttpStatusCode.Unused;

            try
            {
                // 파일 존재 여부
                if (File.Exists(FileFullPathToSave))
                {
                    // 존재할 경우 실패 리턴
                    if (FailIfExist)
                        return false;

                    // 기존 파일 삭제
                    File.Delete(FileFullPathToSave);
                }

                HttpWebRequest webRequest = null;
                byte[] buff = new byte[this._InternalBufferSize];

                using (FileStream fsDownloaded = new FileStream(FileFullPathToSave, FileMode.Create, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fsDownloaded))
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.UserAgent = this._UserAgent;
                    webRequest.Accept = this._Accept;

                    if (!string.IsNullOrEmpty(this._Refer))
                    {
                        webRequest.Referer = webRequest.RequestUri.Scheme + "://" + webRequest.RequestUri.Host;
                    }

                    webRequest.CookieContainer = new CookieContainer();
                    if (cookies != null && cookies.Count > 0)
                    {
                        webRequest.CookieContainer.Add(cookies);
                    }

                    if (!string.IsNullOrEmpty(postcontent))
                    {
                        webRequest.Method = "POST";

                        Encoding enc;

                        if (postencoding == null)
                        {
                            enc = Encoding.UTF8;
                        }
                        else
                        {
                            enc = postencoding;
                        }
                        webRequest.ContentType = "application/x-www-form-urlencoded; charset=" + enc.WebName;
                        byte[] senddata = enc.GetBytes(postcontent);


                        webRequest.ContentLength = senddata.Length;

                        using (Stream dataStream = webRequest.GetRequestStream())
                        {
                            dataStream.Write(senddata, 0, senddata.Length);
                            dataStream.Close();
                        }
                    }

                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        this._LastStatusCode = webResponse.StatusCode;

                        using (BinaryReader br = new BinaryReader(webResponse.GetResponseStream()))
                        {
                            int Received = 0;
                            while ((Received = br.Read(buff, 0, this._InternalBufferSize)) > 0)
                            {
                                bw.Write(buff, 0, Received);
                            }
                            br.Close();
                        }
                    }
                    bw.Close();
                    fsDownloaded.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                this._LastStatusCode = HttpStatusCode.ServiceUnavailable;
                File.Delete(FileFullPathToSave);
                Log4netHelper.Error(ref ex);
            }

            return false;
        }
        public string GetImagePathFromURL(string url, string FileFullPathToSave, bool FailIfExist)
        {
            this._LastStatusCode = HttpStatusCode.Unused;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(FileFullPathToSave))
                return string.Empty;

            try
            {
                // 파일 존재 여부
                if (System.IO.File.Exists(FileFullPathToSave))
                {
                    // 존재할 경우 실패 리턴
                    if (FailIfExist)
                        return FileFullPathToSave;

                    // 기존 파일 삭제
                    System.IO.File.Delete(FileFullPathToSave);
                }

                HttpWebRequest webRequest = null;
                byte[] buff = new byte[this._InternalBufferSize];

                string tempFile = FileFullPathToSave + "$$$";
                string mimetype = string.Empty;

                using (FileStream fsDownloaded = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fsDownloaded))
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.UserAgent = this._UserAgent;
                    webRequest.Accept = this._Accept;
                    webRequest.Timeout = 5000;
                    webRequest.ServicePoint.Expect100Continue = false;

                    if (!string.IsNullOrEmpty(this._Refer))
                    {
                        webRequest.Referer = this._Refer;
                    }

                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        this._LastStatusCode = webResponse.StatusCode;
                        mimetype = webResponse.ContentType;

                        if (!string.IsNullOrEmpty(mimetype))
                        {

                            using (BinaryReader br = new BinaryReader(webResponse.GetResponseStream()))
                            {
                                int Received = 0;
                                //result = streamReader.ReadToEnd();
                                while ((Received = br.Read(buff, 0, this._InternalBufferSize)) > 0)
                                {
                                    bw.Write(buff, 0, Received);
                                    if (this._ApplicationMode)
                                        System.Windows.Forms.Application.DoEvents();
                                }
                                br.Close();
                            }
                        } // if mimetype exists
                    }
                    bw.Close();
                    fsDownloaded.Close();

                    string newPath = string.Empty; //ImageHelper.TransformImageFile(tempFile, "image/png", 50, 50);
                    System.IO.File.Delete(tempFile);

                    return newPath;
                }
            }
            catch (Exception ex)
            {
                this._LastStatusCode = HttpStatusCode.ServiceUnavailable;
                System.IO.File.Delete(FileFullPathToSave);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// Original code from http://developer.nirvanix.com/forums/t/56.aspx
        /// </summary>
        /// <param name="posturl">POST할 URL</param>
        /// <param name="cookies">쿠기값</param>
        /// <param name="postdata">POST 데이터</param>
        /// <param name="postencoding">POST의 데이터 인코딩</param>
        /// <param name="files">POST 파일 목록</param>
        /// <param name="fetchencoding">컨텐츠 인코딩</param>
        /// <returns></returns>
        public string PostFileToURL(string posturl, CookieCollection cookies, System.Collections.Generic.Dictionary<string, string> postdata, Encoding postencoding, System.Collections.Generic.Dictionary<string, string> files, Encoding fetchencoding)
        {
            if (string.IsNullOrEmpty(posturl))
                return string.Empty;

            this._LastStatusCode = HttpStatusCode.Unused;

            try
            {
                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                Encoding enc;
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                if (postencoding == null)
                {
                    enc = Encoding.UTF8;
                }
                else
                {
                    enc = postencoding;
                }

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(posturl);
                webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                webRequest.Method = "POST";

                webRequest.UserAgent = this._UserAgent;
                webRequest.Accept = this._Accept;

                if (!string.IsNullOrEmpty(this._Refer))
                {
                    webRequest.Referer = this._Refer;
                }

                webRequest.CookieContainer = new CookieContainer();
                if (cookies != null && cookies.Count > 0)
                {
                    webRequest.CookieContainer.Add(cookies);
                }

                using (System.IO.Stream requestStream = webRequest.GetRequestStream())
                {
                    if (postdata != null && postdata.Count > 0)
                    {
                        foreach (string key in postdata.Keys)
                        {
                            // 바운더리
                            requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                            byte[] postcontent = enc.GetBytes(postdata[key]);
                            byte[] headerbytes = enc.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\nnContent-Type: application/x-www-form-urlencoded; charset={1}\r\nContent-Length: {2}\r\n\r\n", key, enc.WebName, postcontent.Length));
                            requestStream.Write(headerbytes, 0, headerbytes.Length); // header
                            requestStream.Write(postcontent, 0, postcontent.Length); // content
                        }
                    } // if postdata

                    if (files != null && files.Count > 0)
                    {
                        foreach (string key in files.Keys)
                        {
                            if (System.IO.File.Exists(files[key]))
                            {
                                // 바운더리
                                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                                System.IO.FileInfo fi = new System.IO.FileInfo(files[key]);
                                byte[] headerbytes = enc.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\nContent-Length: {2}\r\n\r\n", key, System.IO.Path.GetFileName(files[key]), fi.Length));
                                fi = null;
                                requestStream.Write(headerbytes, 0, headerbytes.Length);

                                using (System.IO.FileStream fileStream = new System.IO.FileStream(files[key], System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                {
                                    // Use 4096 for the buffer
                                    byte[] buffer = new byte[4096];

                                    int bytesRead = 0;
                                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                    {
                                        requestStream.Write(buffer, 0, bytesRead);
                                        requestStream.Flush();
                                        if (this._ApplicationMode)
                                            System.Windows.Forms.Application.DoEvents();
                                    }

                                    fileStream.Close();
                                }
                            }
                        }
                    } // if files

                    // 종료 바운더리 보냄
                    boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    // Write out the trailing boundry
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                    // Close the request and file stream
                    requestStream.Close();
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    this._LastStatusCode = webResponse.StatusCode;
                    if (fetchencoding == null)
                    {
                        if (!string.IsNullOrEmpty(webResponse.CharacterSet))
                        {
                            try
                            {
                                switch (webResponse.CharacterSet.ToLower())
                                {
                                    case "utf-8": enc = Encoding.UTF8; break;
                                    case "euc-kr": enc = Encoding.GetEncoding("euc-kr"); break;
                                    default: enc = string.IsNullOrEmpty(webResponse.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(webResponse.CharacterSet); break;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.ToString());
                                enc = Encoding.UTF8;
                            }
                        }
                        else
                        {
                            enc = Encoding.Default;
                        }
                    }
                    else
                    {
                        enc = fetchencoding;
                    }
                    using (System.IO.Stream responseStream = webResponse.GetResponseStream())
                    using (System.IO.StreamReader responseReader = new System.IO.StreamReader(responseStream, enc))
                    {

                        string responseString = responseReader.ReadToEnd();

                        // Close response object.
                        webResponse.Close();

                        return responseString;
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return string.Empty;
        }

        public string PostFileToURL(string posturl, CookieCollection cookies, System.Collections.Generic.Dictionary<string, string> postdata, Encoding postencoding, System.Collections.Generic.Dictionary<string, string> files, Encoding fetchencoding, NameValueCollection headers)
        {
            if (string.IsNullOrEmpty(posturl))
                return string.Empty;

            this._LastStatusCode = HttpStatusCode.Unused;

            try
            {
                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                Encoding enc;
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                if (postencoding == null)
                {
                    enc = Encoding.UTF8;
                }
                else
                {
                    enc = postencoding;
                }

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(posturl);
                webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                webRequest.Method = "POST";

                webRequest.UserAgent = this._UserAgent;
                webRequest.Accept = this._Accept;

                if (!string.IsNullOrEmpty(this._Refer))
                {
                    webRequest.Referer = this._Refer;
                }

                webRequest.CookieContainer = new CookieContainer();
                if (cookies != null && cookies.Count > 0)
                {
                    webRequest.CookieContainer.Add(cookies);
                }

                if (headers != null && headers.Count > 0)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        webRequest.Headers.Add(key, headers[key]);
                    }
                }

                using (System.IO.Stream requestStream = webRequest.GetRequestStream())
                {
                    if (postdata != null && postdata.Count > 0)
                    {
                        foreach (string key in postdata.Keys)
                        {
                            // 바운더리
                            requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                            byte[] postcontent = enc.GetBytes(postdata[key]);
                            byte[] headerbytes = enc.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\nnContent-Type: application/x-www-form-urlencoded; charset={1}\r\nContent-Length: {2}\r\n\r\n", key, enc.WebName, postcontent.Length));
                            requestStream.Write(headerbytes, 0, headerbytes.Length); // header
                            requestStream.Write(postcontent, 0, postcontent.Length); // content
                        }
                    } // if postdata

                    if (files != null && files.Count > 0)
                    {
                        foreach (string key in files.Keys)
                        {
                            if (System.IO.File.Exists(files[key]))
                            {
                                // 바운더리
                                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                                System.IO.FileInfo fi = new System.IO.FileInfo(files[key]);
                                byte[] headerbytes = enc.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\nContent-Length: {2}\r\n\r\n", key, System.IO.Path.GetFileName(files[key]), fi.Length));
                                fi = null;
                                requestStream.Write(headerbytes, 0, headerbytes.Length);

                                using (System.IO.FileStream fileStream = new System.IO.FileStream(files[key], System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                {
                                    // Use 4096 for the buffer
                                    byte[] buffer = new byte[4096];

                                    int bytesRead = 0;
                                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                    {
                                        requestStream.Write(buffer, 0, bytesRead);
                                        requestStream.Flush();
                                        if (this._ApplicationMode)
                                            System.Windows.Forms.Application.DoEvents();
                                    }

                                    fileStream.Close();
                                }
                            }
                        }
                    } // if files

                    // 종료 바운더리 보냄
                    boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    // Write out the trailing boundry
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                    // Close the request and file stream
                    requestStream.Close();
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    this._LastStatusCode = webResponse.StatusCode;
                    if (fetchencoding == null)
                    {
                        if (!string.IsNullOrEmpty(webResponse.CharacterSet))
                        {
                            try
                            {
                                switch (webResponse.CharacterSet.ToLower())
                                {
                                    case "utf-8": enc = Encoding.UTF8; break;
                                    case "euc-kr": enc = Encoding.GetEncoding("euc-kr"); break;
                                    default: enc = string.IsNullOrEmpty(webResponse.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(webResponse.CharacterSet); break;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.ToString());
                                enc = Encoding.UTF8;
                            }
                        }
                        else
                        {
                            enc = Encoding.Default;
                        }
                    }
                    else
                    {
                        enc = fetchencoding;
                    }
                    using (System.IO.Stream responseStream = webResponse.GetResponseStream())
                    using (System.IO.StreamReader responseReader = new System.IO.StreamReader(responseStream, enc))
                    {

                        string responseString = responseReader.ReadToEnd();

                        // Close response object.
                        webResponse.Close();

                        return responseString;
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// application/force-download 데이터를 다운로드한다.
        /// </summary>
        /// <param name="url">요청 URL</param>
        /// <param name="headers">추가할 헤더</param>
        /// <param name="postdata">요청에 포함될 POST 데이타(null 또는 empty가 아니면 요청은 POST로 변경됨)</param>
        /// <param name="FolderPath">저장할 경로</param>
        /// <param name="FileName">저장할 파일 이름 (null 또는 empty 인 경우, Content-Disposition 에 정의된 파일 이름으로 읽거나 temp.file에 저장한다.)</param>
        /// <returns>저장 경로 (empty 인 경우, 실패)</returns>
        public String DownloadForceFile(string url, NameValueCollection headers, string postdata, string FolderPath, string FileName)
        {
            this._LastStatusCode = HttpStatusCode.Unused;
            String filePath = String.Empty;

            try
            {
                HttpWebRequest webRequest = null;
                byte[] buff = new byte[this._InternalBufferSize];

                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.UserAgent = this._UserAgent;
                webRequest.Accept = this._Accept;

                if (headers != null && headers.Count > 0)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        webRequest.Headers.Add(key, headers[key]);
                    }
                }

                if (!string.IsNullOrEmpty(this._Refer))
                {
                    webRequest.Referer = webRequest.RequestUri.Scheme + "://" + webRequest.RequestUri.Host;
                }

                if (!string.IsNullOrEmpty(postdata))
                {
                    webRequest.Method = "POST";
                    Encoding enc = Encoding.UTF8;

                    webRequest.ContentType = "application/x-www-form-urlencoded; charset=" + enc.WebName;

                    byte[] senddata = enc.GetBytes(postdata);

                    webRequest.ContentLength = senddata.Length;

                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(senddata, 0, senddata.Length);
                        dataStream.Close();
                    }

                    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        this._LastStatusCode = webResponse.StatusCode;
                        if (String.IsNullOrEmpty(FileName))
                        {
                            if (webResponse.Headers.AllKeys.Contains("Content-Disposition"))
                            {
                                string cpString = webResponse.Headers.Get("Content-Disposition");
                                ContentDisposition contentDisposition = new ContentDisposition(cpString);
                                FileName = contentDisposition.FileName;
                            }
                            else
                            {
                                FileName = "temp.file";
                            }
                        }

                        filePath = Path.Combine(FolderPath, FileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        using (FileStream fsDownloaded = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fsDownloaded))
                            {
                                using (BinaryReader br = new BinaryReader(webResponse.GetResponseStream()))
                                {
                                    int Received = 0;
                                    while ((Received = br.Read(buff, 0, this._InternalBufferSize)) > 0)
                                    {
                                        bw.Write(buff, 0, Received);
                                    }
                                    br.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._LastStatusCode = HttpStatusCode.ServiceUnavailable;
                Log4netHelper.Error(ref ex);
                filePath = String.Empty;
            }

            return filePath;
        }
    }
}
