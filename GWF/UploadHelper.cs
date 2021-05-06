using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GWF
{
    /// <summary>
    /// HTTP Posted File Class
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 업로드된 총 파일 수
        /// </summary>
        /// <returns>업로드된 파일 수</returns>
        public static int UploadFileCount()
        {
            if (HttpContext.Current != null && HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Files != null)
                return HttpContext.Current.Request.Files.Count;

            return 0;
        }

        /// <summary>
        /// 대상 파일을 저장
        /// 
        /// 대상 경로에 같은 파일이 있을 경우  파일명(숫자).확장자 로 변경 저장하고 저장된 파일명 리턴
        /// </summary>
        /// <param name="idx">업로드 파일 대상 인덱스</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="FileName">저장된 실제 파일명</param>
        /// <param name="FileSize">저장된 파일의 사이즈(byte)</param>
        /// <returns>저장 성공시 true</returns>
        public static bool SaveUploadFile(int idx, string SaveToPath, out string FileName, out int FileSize)
        {
            if (idx < HttpContext.Current.Request.Files.Count)
                return SaveUploadFile(HttpContext.Current.Request.Files.GetKey(idx), SaveToPath, true, out FileName, out FileSize);

            FileName = string.Empty;
            FileSize = 0;
            return false;
        }

        /// <summary>
        /// 대상 파일을 저장
        /// 
        /// overwrite가 false 일 때, 대상 경로에 같은 파일이 있을 경우  파일명(숫자).확장자 로 변경 저장하고 저장된 파일명 리턴
        /// </summary>
        /// <param name="idx">업로드 파일 대상 인덱스</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="overwrite">true이면 대상 파일 존재 시 덮어쓰기, 아니면 새 파일명 생성</param>
        /// <param name="FileName">저장된 실제 파일명</param>
        /// <param name="FileSize">저장된 파일의 사이즈(byte)</param>
        /// <returns>저장 성공시 true</returns>
        public static bool SaveUploadFile(int idx, string SaveToPath, bool overwrite, out string FileName, out int FileSize)
        {
            if (idx < HttpContext.Current.Request.Files.Count)
                return SaveUploadFile(HttpContext.Current.Request.Files.GetKey(idx), SaveToPath, overwrite, out FileName, out FileSize);

            FileName = string.Empty;
            FileSize = 0;
            return false;
        }

        /// <summary>
        /// 대상 파일을 저장
        /// 
        /// 대상 경로에 같은 파일이 있을 경우  파일명(숫자).확장자 로 변경 저장하고 저장된 파일명 리턴
        /// </summary>
        /// <param name="id">업로드 파일 대상 컨트롤 ID</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="FileName">저장된 실제 파일명</param>
        /// <param name="FileSize">저장된 파일의 사이즈(byte)</param>
        /// <returns>저장 성공시 true</returns>
        public static bool SaveUploadFile(string id, string SaveToPath, out string FileName, out int FileSize)
        {
            FileName = string.Empty;
            FileSize = 0;
            try
            {
                return SaveUploadFile(HttpContext.Current.Request.Files[id], SaveToPath, false, out FileName, out FileSize);
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 대상 파일을 저장
        /// 
        /// overwrite가 false 일 때, 대상 경로에 같은 파일이 있을 경우  파일명(숫자).확장자 로 변경 저장하고 저장된 파일명 리턴
        /// </summary>
        /// <param name="id">업로드 파일 대상 컨트롤 ID</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="overwrite">true이면 대상 파일 존재 시 덮어쓰기, 아니면 새 파일명 생성</param>
        /// <param name="FileName">저장된 실제 파일명</param>
        /// <param name="FileSize">저장된 파일의 사이즈(byte)</param>
        /// <returns>저장 성공시 true</returns>
        public static bool SaveUploadFile(string id, string SaveToPath, bool overwrite, out string FileName, out int FileSize)
        {
            FileName = string.Empty;
            FileSize = 0;
            try
            {
                return SaveUploadFile(HttpContext.Current.Request.Files[id], SaveToPath, overwrite, out FileName, out FileSize);
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 대상 파일 저장
        /// </summary>
        /// <param name="file">대상 HttpPostedFile</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="overwrite">true이면 대상 파일 존재 시 덮어쓰기, 아니면 새 파일명 생성</param>
        /// <param name="FileName">저장된 실제 파일명</param>
        /// <param name="FileSize">저장된 파일의 사이즈(byte)</param>
        /// <returns>저장 성공시 true</returns>
        public static bool SaveUploadFile(HttpPostedFile file, string SaveToPath, bool overwrite, out string FileName, out int FileSize)
        {
            try
            {
                try
                {
                    Log4netHelper.InfoFormat("Upload {0} - {1} - {2}", file.ContentType, file.FileName, file.ContentLength);
                }
                catch
                {
                }

                FileName = string.Empty;
                FileSize = 0;

                Directory.CreateDirectory(SaveToPath);

                string uploadfilename = Path.GetFileName(file.FileName);

                if (file != null)
                {
                    if (File.Exists(Path.Combine(SaveToPath, uploadfilename)))
                    {
                        // 파일 존재시 처리
                        if (overwrite)
                        {
                            file.SaveAs(Path.Combine(SaveToPath, uploadfilename));
                            FileName = uploadfilename;
                            FileSize = file.ContentLength;
                            Log4netHelper.InfoFormat("Upload Saved {0} - {1}", Path.Combine(SaveToPath, uploadfilename), FileSize);
                            return true;
                        }
                        else
                        {
                            int n = 1;
                            while (true)
                            {
                                string temp = string.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(file.FileName), n, Path.GetExtension(file.FileName));

                                if (!File.Exists(Path.Combine(SaveToPath, temp)))
                                {
                                    // 파일이 존재하지 않으면 저장 후 저장된 파일명 리턴
                                    file.SaveAs(Path.Combine(SaveToPath, temp));
                                    FileName = temp;
                                    FileSize = file.ContentLength;
                                    Log4netHelper.InfoFormat("Upload Saved {0} - {1}", Path.Combine(SaveToPath, temp), FileSize);
                                    return true;
                                }
                                else
                                    n++;
                            }
                        }
                    }
                    else
                    {
                        // 파일 없을 때

                        file.SaveAs(Path.Combine(SaveToPath, uploadfilename));
                        FileName = uploadfilename;
                        FileSize = file.ContentLength;
                        Log4netHelper.InfoFormat("Upload Saved {0} - {1}", Path.Combine(SaveToPath, uploadfilename), FileSize);
                        return true;
                    }
                }
            }
            catch
            {
            }
            FileName = string.Empty;
            FileSize = 0;
            Log4netHelper.Info("Uploaded empty");
            return false;
        }



        /// <summary>
        /// 대상 파일 저장
        /// </summary>
        /// <param name="file">대상 HttpPostedFile</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="SaveFileName">파일이름 지정하여 저장</param>
        /// <param name="overwrite">true이면 대상 파일 존재 시 덮어쓰기, 아니면 새 파일명 생성</param>
        /// <param name="FileName">저장된 실제 파일명</param>
        /// <param name="FileSize">저장된 파일의 사이즈(byte)</param>
        /// <returns>저장 성공시 true</returns>
        public static bool SaveUploadFile(HttpPostedFile file, string SaveToPath, string SaveFileName, bool overwrite, out string FileName, out int FileSize)
        {
            try
            {
                try
                {
                    Log4netHelper.InfoFormat("Upload {0} - {1} - {2}", file.ContentType, file.FileName, file.ContentLength);
                }
                catch
                {
                }

                FileName = string.Empty;
                FileSize = 0;

                Directory.CreateDirectory(SaveToPath);

                if (file != null)
                {
                    if (File.Exists(Path.Combine(SaveToPath, SaveFileName)))
                    {
                        // 파일 존재시 처리
                        if (overwrite)
                        {
                            file.SaveAs(Path.Combine(SaveToPath, SaveFileName));
                            FileName = SaveFileName;
                            FileSize = file.ContentLength;
                            Log4netHelper.InfoFormat("Upload Saved {0} - {1}", Path.Combine(SaveToPath, SaveFileName), FileSize);
                            return true;
                        }
                        else
                        {
                            int n = 1;
                            while (true)
                            {
                                string temp = string.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(file.FileName), n, Path.GetExtension(file.FileName));

                                if (!File.Exists(Path.Combine(SaveToPath, temp)))
                                {
                                    // 파일이 존재하지 않으면 저장 후 저장된 파일명 리턴
                                    file.SaveAs(Path.Combine(SaveToPath, temp));
                                    FileName = temp;
                                    FileSize = file.ContentLength;
                                    Log4netHelper.InfoFormat("Upload Saved {0} - {1}", Path.Combine(SaveToPath, temp), FileSize);
                                    return true;
                                }
                                else
                                    n++;
                            }
                        }
                    }
                    else
                    {
                        // 파일 없을 때

                        file.SaveAs(Path.Combine(SaveToPath, SaveFileName));
                        FileName = SaveFileName;
                        FileSize = file.ContentLength;
                        Log4netHelper.InfoFormat("Upload Saved {0} - {1}", Path.Combine(SaveToPath, SaveFileName), FileSize);
                        return true;
                    }
                }
            }
            catch
            {
            }
            FileName = string.Empty;
            FileSize = 0;
            Log4netHelper.Info("Uploaded empty");
            return false;
        }


        /// <summary>
        /// 대상 파일의 조각 저장
        /// </summary>
        /// <param name="file">대상 조각 HttpPostedFile</param>
        /// <param name="FileName">파일명</param>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="ChunkIndex">조각 인덱스</param>
        /// <param name="ChunkSize">조각 사이즈(Mbyte):1보다 작거나 10보다 크면 기본 2M</param>
        /// <param name="FileSize">0번 조각부터 저장된 조각까지의 사이즈 합(byte)</param>
        /// <returns></returns>
        public static bool SaveUploadChunk(HttpPostedFile file, string FileName, string SaveToPath, int ChunkIndex, int ChunkSize, out int FileSize)
        {
            try
            {
                try
                {
                    Log4netHelper.InfoFormat("Upload Chunk {0} - {1} - {2}", file.ContentType, FileName, file.ContentLength);
                }
                catch
                {
                }
                FileSize = 0;
                if (file != null)
                {
                    string uploadfilename = FileName;
                    string filefullpathname = Path.Combine(SaveToPath, uploadfilename);
                    FileInfo ofi = new FileInfo(filefullpathname);
                    if (ChunkIndex == 0 && ofi.Exists) ofi.Delete();
                    using (Stream fs = file.InputStream)
                    {
                        int bufferLength = 4096 <= (int)fs.Length ? 4096 : (int)fs.Length;
                        byte[] buffer = new byte[bufferLength];
                        int contentLength = fs.Read(buffer, 0, bufferLength);

                        using (FileStream fRead = new FileStream(filefullpathname, ChunkIndex == 0 ? FileMode.Create : FileMode.Open, FileAccess.ReadWrite))
                        {
                            if (ChunkSize > 0 || ChunkSize < 11) fRead.Seek(ChunkIndex * (ChunkSize * 1024 * 1024), SeekOrigin.Begin);
                            else fRead.Seek(ChunkIndex * (2 * 1024 * 1024), SeekOrigin.Begin);
                            while (contentLength != 0)
                            {
                                fRead.Write(buffer, 0, contentLength);
                                contentLength = fs.Read(buffer, 0, bufferLength);
                            }
                            fRead.Close();
                        }
                        fs.Close();
                    }
                    FileInfo fi = new FileInfo(filefullpathname);
                    FileSize = ConvertHelper.ToInt(fi.Length);
                    return true;
                }
            }
            catch
            {
            }
            FileSize = 0;
            Log4netHelper.Info("Uploaded empty");
            return false;
        }

        /// <summary>
        /// 고유한 파일명으로 변경
        /// </summary>
        /// <param name="SaveToPath">파일이 저장될 대상 폴더</param>
        /// <param name="FileName">원본 파일명</param>
        /// <returns>고유한 파일명</returns>
        public static string UniqFileName(string SaveToPath, string FileName)
        {
            if (File.Exists(Path.Combine(SaveToPath, FileName)))
            {
                int n = 1;
                while (true)
                {
                    string temp = string.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(FileName), n, Path.GetExtension(FileName));

                    if (!File.Exists(Path.Combine(SaveToPath, temp)))
                    {
                        //파일명 리턴
                        return temp;
                    }
                    else
                        n++;
                }
            }
            else
            {
                return FileName;
            }
        }

        /// <summary>
        /// 파일 다운로드
        /// 
        /// 다운로드 하려는 파일이 없으면 false 리턴
        /// </summary>
        /// <param name="filepath">파일의 풀경로(경로+파일이름)</param>
        /// <param name="outputfilename">다운로드시 사용자에게 표시될 파일명</param>
        /// <returns>filepath 파일이 없을 경우 false 리턴</returns>
        public static bool DownloadFile(string filepath, string outputfilename)
        {
            if (HttpContext.Current != null && HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Files != null)
            {
                FileInfo fi = new FileInfo(filepath);

                if (fi.Exists)
                {
                    HttpContext.Current.Response.Clear();

                    if (HttpContext.Current.Request.UserAgent.IndexOf("MSIE 6.0") >= 0) // 6.0
                    {
                        HttpContext.Current.Response.ContentType = "application/x-msdownload";
                        HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                    }
                    else if (HttpContext.Current.Request.UserAgent.IndexOf("5.5") >= 0)
                    {
                        HttpContext.Current.Response.ContentType = "doesn/matter";
                        HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                    }
                    else if (HttpContext.Current.Request.UserAgent.IndexOf("5.1") >= 0)
                    {
                        HttpContext.Current.Response.ContentType = "application/unknown";
                    }
                    else if (HttpContext.Current.Request.UserAgent.IndexOf("5.0") >= 0)
                    {
                        HttpContext.Current.Response.ContentType = "application/unknown";
                    }
                    else
                    {
                        HttpContext.Current.Response.ContentType = "application/unknown";
                    }

                    HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                    HttpContext.Current.Response.AddHeader("Expires", "0");

                    // 한글명일 경우 깨지지 않게 하기 위해     
                    if (HttpContext.Current.Request.UserAgent.IndexOf("NT 5.0") >= 0)
                    {
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;FileName = " + System.Web.HttpUtility.UrlEncode(outputfilename));
                    }
                    else
                    {
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;FileName = " + System.Web.HttpUtility.UrlEncode(outputfilename, new System.Text.UTF8Encoding()).Replace("+", "%20"));
                    }

                    HttpContext.Current.Response.AddHeader("Content-Length", fi.Length.ToString());
                    HttpContext.Current.Response.WriteFile(filepath);
                    HttpContext.Current.Response.Flush();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// directory 삭제
        /// </summary>
        /// <param name="path">전체 경로</param>
        /// <param name="recursive">폴더 안 파일까지 제거할지 여부</param>
        /// <returns>제거 후 존재하지 않으면 true, 오류거나 존재하면 false</returns>
        public static bool RemoveDirectory(string path, bool recursive)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    di.Delete(recursive);
                }

                if (!Directory.Exists(path)) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="dirpath">directory 경로</param>
        /// <param name="filename">파일명</param>
        /// <returns>제거 후 존재하지 않으면 true, 오류거나 존재하면 false</returns>
        public static bool RemoveFile(string dirpath, string filename)
        {
            string filefullpath = dirpath.EndsWith(@"\") ? string.Format("{0}{1}", dirpath, filename) : Path.Combine(dirpath, filename);
            return RemoveFile(filefullpath);
        }

        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="filefullpath">전체 경로</param>
        /// <returns>제거 후 존재하지 않으면 true, 오류거나 존재하면 false</returns>
        public static bool RemoveFile(string filefullpath)
        {
            try
            {
                FileInfo fi = new FileInfo(filefullpath);
                if (fi.Exists)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    fi.Delete();
                }

                if (!File.Exists(filefullpath)) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 파일을 삭제하고 폴더에 파일이 없으면 dirdelete에 따라 폴더가 비워져 있으면 삭제
        /// </summary>
        /// <param name="dirpath">폴더 경로</param>
        /// <param name="filename">파일명</param>
        /// <returns>폴더까지 제거 되면 true, 파일만 삭제되면 false</returns>
        public static bool RemoveDirectoryFile(string dirpath, string filename)
        {
            string filefullpath = dirpath.EndsWith(@"\") ? string.Format("{0}{1}", dirpath, filename) : Path.Combine(dirpath, filename);
            return RemoveDirectoryFile(filefullpath);
        }

        /// <summary>
        /// 파일을 삭제하고 폴더에 파일이 없으면 폴더 삭제
        /// </summary>
        /// <param name="filefullpath">파일 전체 경로</param>
        /// <returns>폴더까지 제거 되면 true, 파일만 삭제되면 false</returns>
        public static bool RemoveDirectoryFile(string filefullpath)
        {
            string dirpath = Path.GetDirectoryName(filefullpath);
            if (RemoveFile(filefullpath))
            {
                DirectoryInfo di = new DirectoryInfo(dirpath);
                if (di.Exists)
                {
                    if (di.GetFiles().Length == 0)
                    {
                        RemoveDirectory(dirpath, false);
                    }
                }
            }
            if (!Directory.Exists(dirpath)) return true;
            else return false;
        }
    }
}
