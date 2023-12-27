using System.Data;
using System.Text.Json;
using HttpServer.Model;
using Npgsql;

namespace server.server.Orm;

public class AccountsData
{
	static string connectionString = ServerConfiguration._config.ConnectionString;
	
	public void Add(string email, string password)
	{
		using (var connection = new NpgsqlConnection(connectionString))
		{
			connection.Open();
			using (var command = new NpgsqlCommand("INSERT INTO Accounts (email, password) VALUES (@email, @password)", connection))
			{
				command.Parameters.AddWithValue("email", email);
				command.Parameters.AddWithValue("password", password);
				command.ExecuteNonQuery();
			}
		}
	}

	public void Delete(string id)
	{
		using (var connection = new NpgsqlConnection(connectionString))
		{
			connection.Open();
			using (var command = new NpgsqlCommand("DELETE FROM Accounts WHERE id = @id", connection))
			{
				command.Parameters.AddWithValue("id", id);
				command.ExecuteNonQuery();
			}
		}
	}
	
	public void Update(string id, string newEmail, string newPassword)
	{
		using (var connection = new NpgsqlConnection(connectionString))
		{
			connection.Open();
			using (var command = new NpgsqlCommand("UPDATE Accounts SET email = @email, password = @password WHERE id = @id", connection))
			{
				command.Parameters.AddWithValue("email", newEmail);
				command.Parameters.AddWithValue("password", newPassword);
				command.Parameters.AddWithValue("id", id);
				command.ExecuteNonQuery();
			}
		}
	}
	
	public string GetById(string id)
	{
		using (var connection = new NpgsqlConnection(connectionString))
		{
			connection.Open();
			using (var command = new NpgsqlCommand("SELECT * FROM Accounts WHERE id = @id", connection))
			{
				command.Parameters.AddWithValue("id", id);

				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						var account = new Account()
						{
							id = reader.GetString(0),
							Email = reader.GetString(1),
							password = reader.GetString(2)
						};
						var json = JsonSerializer.Serialize(account);
						return json;
					}
				}
			}
		}

		return null;
	}
	
	public string GetAll()
	{
		var accountList = new List<Account>();

		using (var connection = new NpgsqlConnection(connectionString))
		{
			connection.Open();
			using (var command = new NpgsqlCommand("SELECT * FROM Accounts", connection))
			{
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var account = new Account()
						{
							id = reader.GetString(0),
							Email = reader.GetString(1),
							password = reader.GetString(2)
						};
						accountList.Add(account);
					}
				}
			}
		}

		var json = JsonSerializer.Serialize(accountList);
		return json;
	}
}