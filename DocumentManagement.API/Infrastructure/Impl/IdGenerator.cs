using System;

namespace DocumentManagement.API.Infrastructure.Impl
{
    public class IdGenerator : IIdGenerator
    {
        public Guid NewId()
        {
            return Guid.NewGuid();
        }
    }
}
