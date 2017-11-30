using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Model;
using SQLite;

namespace MPS.Data
{
    public class MessageDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public MessageDatabase(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<Message>().Wait();
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _database.Table<Message>().ToListAsync();
        }

        public Task<Message> GetMessageAsync(string title)
        {
            return _database.Table<Message>()
                .Where(i => i.Title.Equals(title))
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveMessageAsync(Message message)
        {
            return message.Id != 0 ? 
                _database.UpdateAsync(message) :
                _database.InsertAsync(message);
        }

        public Task<int> DeleteMessageAsync(Message message)
        {
            return _database.DeleteAsync(message);
        }
    }
}
