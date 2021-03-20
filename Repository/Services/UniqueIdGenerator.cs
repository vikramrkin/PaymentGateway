using System;

namespace Repository.Services
{
    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        public string GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public interface IUniqueIdGenerator
    {
        string GetUniqueId();
    }
}
