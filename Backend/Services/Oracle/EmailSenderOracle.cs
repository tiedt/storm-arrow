using SIMP.Classes;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class EmailSenderOracle : IEmailSender{
        
        private readonly IServidorEmailRepository servidorEmailRepository;

        public EmailSenderOracle(IServidorEmailRepository servidorEmailRepository){
            this.servidorEmailRepository = servidorEmailRepository;
        }

        private bool IsBodyHtml(string Body){
            if(String.IsNullOrEmpty(Body))
                return false;
            string[] TagsHtml = new string[] { // Tags do html
                "<!--","<!doctype>","<a>","<abbr>","<address>","<area>","<b>","<base>","<bdo>","<blockquote>","<body>","<br>","<button>","<caption>","<cite>",
                "<code>","<col>","<colgroup>","<dd>","<del>","<dfn>","<div>","<dl>","<dt>","<em>","<fieldset>","<form>","<h1>","<h2>","<h3>","<h4>","<h5>","<h6>",
                "<head>","<hr>","<html>","<i>","<iframe>","<img>","<input>","<ins>","<kbd>","<label>","<legend>","<li>","<link>","<map>","<menu>","<meta>",
                "<noscript>","<object>","<ol>","<optgroup>","<option>","<p>","<param>","<pre>","<q>","<s>","<samp>","<script>","<select>","<small>","<span>",
                "<strong>","<style>","<sub>","<table>","<tbody>","<td>","<textare>","<tfoot>","<th>","<thread>","<title>","<tr>","<ul>","<var>","<acronym>",
                "<applet>","<basefont>","<big>","<center>","<dir>","<font>","<frame>","<frameset>","<noframes>","<strike>","<tt>","<u>","<xmp>" };
            foreach(string tag in TagsHtml)
                if(Body.ToUpper().Contains(tag.ToUpper()))
                    return true;            
            return false;
        }


        public async Task<bool> SendEmail(string Service, Email Email){
            ServidorEmail servidorEmail = await servidorEmailRepository.GetByName(Service);
            Email.ServerAddress = servidorEmail.Ds_endereco_smtp;
            Email.Username = "simpcontatosimp@gmail.com";
            Email.Password = "Trabalhosimp!2";
            Email.Port = servidorEmail.Nr_porta;
            Email.EnableSsl = servidorEmail.Nr_usa_ssl == 1;
            Email.FromEmail = Email.Username;
            Email.CcEmails.Append(Email.Username);
            Email.IsBodyHtml = IsBodyHtml(Email.Body);            
            
            MailMessage mailMessage = new MailMessage(){
                From = new MailAddress(Email.FromEmail),
                Subject = Email.Subject,
                Body = Email.Body
            };            
            foreach(string recipient in Email.Recipients)
                mailMessage.To.Add(new MailAddress(recipient));
            mailMessage.IsBodyHtml = Email.IsBodyHtml;

            SmtpClient smtpClient = new SmtpClient(Email.ServerAddress){
                Port = Email.Port,
                Credentials = new NetworkCredential(Email.Username, Email.Password),
                EnableSsl = Email.EnableSsl
            };

            try{
                await smtpClient.SendMailAsync(mailMessage);
            }catch (Exception e){
                throw new Exception(e.Message);
            }

            return true;
        }
    }
}
