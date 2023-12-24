using System;
using System.Net;
using System.Text;
using HttpServer.Attributes;

using System.Text.Json;
using DefaultNamespace;
using HttpServer.Model;
using server;

namespace HttpServer
{
	[HttpController("accounts")]
	public class AccountsController
	{
		private EmailsSenderService emailSender;
		private readonly Config _config = ServerConfiguration._config;


		public AccountsController()
		{
			emailSender = new EmailsSenderService();
		}

		[HttpPost("Add")]
		public void Add(string message)
		{
			emailSender.SendMail(message);
		}

		[HttpGet("GetEmailList")]
		public string GetEmailList(object anyObject)
		{
			if (anyObject is String)
			{
				return ((string)anyObject).ToString();
			}
			else
			{
				var json = JsonSerializer.Serialize(anyObject);
				return json;
			}
		}

		[HttpGet("GetAccountList")]
		public Account[] GetAccountList()
		{
			var accounts = new[]
			{
				new Account() {Email = "123", password = "222"},
				new Account() {Email = "222", password = "111"}

			};
			return accounts;
		}

		public void Delete()
		{

		}

		public void Update()
		{

		}

		public void Select()
		{

		}

		public void SelectByEmail()
		{

		}

	}
}