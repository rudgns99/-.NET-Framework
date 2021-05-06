using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace GWF
{
    /// <summary>
    /// 이메일 전송 관련
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// 메일 보내기
        /// 메일 형식)
        ///         1) DisplayName &lt;email@address.com&gt;
        ///         2) email@address.com
        /// </summary>
        /// <param name="From">보내는 사람 메일 주소(empty일 경우 web.config 설정)</param>
        /// <param name="To">받는 사람 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Subject">메일 제목</param>
        /// <param name="Content">메일 내용</param>
        /// <param name="HTML">메일 내용이 HTML 일 경우 true</param>
        /// <param name="SmtpAddress">SMTP 서버 주소</param>
        /// <returns>전송 결과(250이면 성공, 250 이외는 오류)</returns>
        public static int SendEmail(string From, string To, string Subject, string Content, bool HTML, string SmtpAddress, System.Net.NetworkCredential Credential = null)
        {
            return SendEmail(From, To, null, null, Subject, Content, HTML, null, SmtpAddress, Credential);
        }

        /// <summary>
        /// 메일 보내기
        /// 메일 형식)
        ///         1) DisplayName &lt;email@address.com&gt;
        ///         2) email@address.com
        /// </summary>
        /// <param name="From">보내는 사람 메일 주소(empty일 경우 web.config 설정)</param>
        /// <param name="To">받는 사람 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Cc">참조 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Subject">메일 제목</param>
        /// <param name="Content">메일 내용</param>
        /// <param name="HTML">메일 내용이 HTML 일 경우 true</param>
        /// <param name="SmtpAddress">SMTP 서버 주소</param>
        /// <returns>전송 결과(250이면 성공, 250 이외는 오류)</returns>
        public static int SendEmail(string From, string To, string Cc, string Subject, string Content, bool HTML, string SmtpAddress, System.Net.NetworkCredential Credential = null)
        {
            return SendEmail(From, To, Cc, null, Subject, Content, HTML, null, SmtpAddress, Credential);
        }

        /// <summary>
        /// 메일 보내기
        /// 메일 형식)
        ///         1) DisplayName &lt;email@address.com&gt;
        ///         2) email@address.com
        /// </summary>
        /// <param name="From">보내는 사람 메일 주소(empty일 경우 web.config 설정)</param>
        /// <param name="To">받는 사람 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Subject">메일 제목</param>
        /// <param name="Content">메일 내용</param>
        /// <param name="HTML">메일 내용이 HTML 일 경우 true</param>
        /// <param name="attachedFiles">첨부파일</param>
        /// <param name="SmtpAddress">SMTP 서버 주소</param>
        /// <returns>전송 결과(250이면 성공, 250 이외는 오류)</returns>
        public static int SendEmail(string From, string To, string Subject, string Content, bool HTML, List<string> attachedFiles, string SmtpAddress, System.Net.NetworkCredential Credential = null)
        {
            return SendEmail(From, To, null, null, Subject, Content, HTML, attachedFiles, SmtpAddress, Credential);
        }

        /// <summary>
        /// 메일 보내기
        /// 메일 형식)
        ///         1) DisplayName &lt;email@address.com&gt;
        ///         2) email@address.com
        /// </summary>
        /// <param name="From">보내는 사람 메일 주소(empty일 경우 web.config 설정)</param>
        /// <param name="To">받는 사람 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Cc">참조 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Subject">메일 제목</param>
        /// <param name="Content">메일 내용</param>
        /// <param name="HTML">메일 내용이 HTML 일 경우 true</param>
        /// <param name="attachedFiles">첨부파일</param>
        /// <param name="SmtpAddress">SMTP 서버 주소</param>
        /// <returns>전송 결과(250이면 성공, 250 이외는 오류)</returns>
        public static int SendEmail(string From, string To, string Cc, string Subject, string Content, bool HTML, List<string> attachedFiles, string SmtpAddress, System.Net.NetworkCredential Credential = null)
        {
            return SendEmail(From, To, Cc, null, Subject, Content, HTML, attachedFiles, SmtpAddress, Credential);
        }

        /// <summary>
        /// 메일 보내기
        /// 메일 형식)
        ///         1) DisplayName &lt;email@address.com&gt;
        ///         2) email@address.com
        /// </summary>
        /// <param name="From">보내는 사람 메일 주소(empty일 경우 web.config 설정)</param>
        /// <param name="To">받는 사람 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Cc">참조 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Bcc">비밀참조 메일 주소(여러명일 경우 ; 구분)</param>
        /// <param name="Subject">메일 제목</param>
        /// <param name="Content">메일 내용</param>
        /// <param name="HTML">메일 내용이 HTML 일 경우 true</param>
        /// <param name="attachedFiles">첨부파일</param>
        /// <param name="SmtpAddress">SMTP 서버 주소</param>
        /// <returns>전송 결과(250이면 성공, 250 이외는 오류)</returns>
        public static int SendEmail(string From, string To, string Cc, string Bcc, string Subject, string Content, bool HTML, List<string> attachedFiles, string SmtpAddress, System.Net.NetworkCredential Credential = null)
        {
            try
            {
                // 메일 발송 규칙
                // 받는 사람 메일 주소(정규식 대신 길이와 @ 포함 여부만으로 처리)
                // 보네는 사람 메일 주소(정규식 대신 길이와 @ 포함 여부만으로 처리)
                // 제목 또는 내용이 없을 경우 안 보냄
                if (string.IsNullOrEmpty(To) || To.Length < 5 || !To.Contains("@")
                    || string.IsNullOrEmpty(From) || From.Length < 5 || !From.Contains("@")
                    || string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(Content))
                    return -4;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(From);

                if (To.Contains(";"))
                {
                    string[] arr = To.Split(new char[] { ';' });
                    if (null == arr || arr.Length < 1)
                        return 1;

                    foreach (string addr in arr)
                    {
                        try
                        {
                            MailAddress mailaddr = new MailAddress(addr);

                            mail.To.Add(mailaddr);
                            Log4netHelper.Info("Added EMail Address : " + addr);
                        }
                        catch (Exception ex)
                        {
                            Log4netHelper.WarnFormat("Invalid Mail format : {0}", ex.ToString());
                        }
                    }
                }
                else
                {
                    mail.To.Add(new MailAddress(To));
                }

                if (!string.IsNullOrEmpty(Cc))
                {
                    if (Cc.Contains(";"))
                    {
                        string[] arr = Cc.Split(new char[] { ';' });
                        if (null == arr || arr.Length < 1)
                            return 1;

                        foreach (string addr in arr)
                        {
                            try
                            {
                                MailAddress mailaddr = new MailAddress(addr);

                                mail.CC.Add(mailaddr);
                                Log4netHelper.Info("Added CC EMail Address : " + addr);
                            }
                            catch (Exception ex)
                            {
                                Log4netHelper.WarnFormat("Invalid CC Mail format : {0}", ex.ToString());
                            }
                        }
                    }
                    else
                    {
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }

                if (!string.IsNullOrEmpty(Bcc))
                {
                    if (Bcc.Contains(";"))
                    {
                        string[] arr = Bcc.Split(new char[] { ';' });
                        if (null == arr || arr.Length < 1)
                            return 1;

                        foreach (string addr in arr)
                        {
                            try
                            {
                                MailAddress mailaddr = new MailAddress(addr);

                                mail.Bcc.Add(mailaddr);
                                Log4netHelper.Info("Added BCC EMail Address : " + addr);
                            }
                            catch (Exception ex)
                            {
                                Log4netHelper.WarnFormat("Invalid BCC Mail format : {0}", ex.ToString());
                            }
                        }
                    }
                    else
                    {
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }

                mail.Subject = Subject;

                mail.Body = Content;

                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = HTML;

                if (attachedFiles != null && attachedFiles.Count > 0)
                {
                    foreach (string filepath in attachedFiles)
                    {
                        if (File.Exists(filepath))
                        {
                            Log4netHelper.InfoFormat("Attached file : {0}", filepath);
                            mail.Attachments.Add(new Attachment(filepath));
                        }
                        else
                        {
                            Log4netHelper.InfoFormat("Skipped attached file : {0}", filepath);
                        }
                    }
                }

                // MailServerIP의 서버에 들어가서 SMTP 설정에서 replay 설정을 해주어서 테스트할 것
                // 각 웹서버 IP 및 127.0.0.1 를 replay 가능으로 설정
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                SmtpAddress = settings.Smtp.Network.Host;
                SmtpClient smtp = new SmtpClient(string.IsNullOrEmpty(SmtpAddress) ? "127.0.0.1" : SmtpAddress);
                smtp.Credentials = new NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
                smtp.Send(mail);

                return 250;
            }
            catch (ArgumentException ex)
            {
                Log4netHelper.ErrorFormat("ArgumentException : {0}", ex.ToString());
                return -3;
            }
            catch (InvalidOperationException ex)
            {
                Log4netHelper.ErrorFormat("InvalidOperationException : {0}", ex.ToString());
                return -2;
            }
            catch (SmtpException ex)
            {
                Log4netHelper.ErrorFormat("SmtpException : {0}", ex.ToString());
                return (int)ex.StatusCode;
            }
            catch (Exception ex)
            {
                Log4netHelper.ErrorFormat("Exception : {0}", ex.ToString());
                return -1;
            }
        }
    }
}
