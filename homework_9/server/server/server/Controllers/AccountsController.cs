using System;
using System.Net;
using System.Text;
using HttpServer.Attributes;

using System.Text.Json;
using DefaultNamespace;
using HttpServer.Model;
using server;
using server.server.Orm;

namespace HttpServer
{
	[HttpController("accounts")]
	public class AccountsController
	{
		private EmailsSenderService emailSender;
		private readonly Config _config = ServerConfiguration._config;
		private static List<Account> accountList = new List<Account>();
		private static AccountsData db = new AccountsData();

		public AccountsController()
		{
			emailSender = new EmailsSenderService();
		}

		[HttpPost("Add")]
		public void Add(string login, string password)
		{
			db.Add(login, password);
		}

		[HttpPost("Delete/{id}")]
		public void Delete(string id)
		{
			db.Delete(id);
		}

		[HttpPost("Update")]
		public void Update(string id, string newEmail, string newPassword)
		{
			db.Update(id, newEmail, newPassword);
		}
		
		[HttpGet("GetById/{id}")]
		public string GetById(string id)
		{
			return db.GetById(id);
		}
		
		[HttpGet("GetAll")]
		public string GetAll()
		{
			return db.GetAll();
		}
	}
}