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
		private static List<Account> accountList = new List<Account>();

		public AccountsController()
		{
			emailSender = new EmailsSenderService();
		}

		[HttpPost("Add")]
		public void Add(string login, string password)
		{
			var account = new Account() { id = new Random().Next().ToString(), Email = login, password = password };
			accountList.Add(account);
			emailSender.SendMail("Новое сообщение с сайта: login: " + login +
				"\npassword: " + password, login);
		}
		
		[HttpGet("GetAll")]
		public string GetAll()
		{
			var json = JsonSerializer.Serialize(accountList);
			return json;
		}

		[HttpPost("Delete/{id}")]
		public void Delete(string id)
		{
			var account = accountList.Find(a => a.id == id);
			if (account != null)
			{
				accountList.Remove(account);
			}
		}

		[HttpPost("Update")]
		public void Update(string id, string newEmail, string newPassword)
		{
			var account = accountList.Find(a => a.id == id);
			if (account != null)
			{
				account.Email = newEmail;
				account.password = newPassword;
			}
		}
		
		[HttpGet("GetById/{id}")]
		public string GetById(string id)
		{
			var account = accountList.Find(a => a.id == id);
			if (account != null)
			{
				var json = JsonSerializer.Serialize(account);
				return json;
			}
			return null;
		}

	}
}