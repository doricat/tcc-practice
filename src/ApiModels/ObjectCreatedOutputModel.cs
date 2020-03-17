using System;

namespace ApiModels
{
    public class ObjectCreatedOutputModel<TKey>
    {
        public ObjectCreatedOutputModel()
        {
        }

        public ObjectCreatedOutputModel(TKey id, string location)
        {
            Id = id;
            Location = location;
        }

        public TKey Id { get; set; }

        public string Location { get; set; }
    }

    public class TransactionObjectCreatedOutputModel<TKey> : ObjectCreatedOutputModel<TKey>
    {
        public TransactionObjectCreatedOutputModel(TKey id, string location, DateTime expires) : base(id, location)
        {
            Expires = expires;
        }

        public DateTime Expires { get; set; }
    }
}