using Marten;
using Marten.Internal.Sessions;

namespace WebApplicationMicroservice.Marten
{
    public class MartenModule : ISessionFactory
    {
        private IDocumentStore _documentStore;
        public MartenModule(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }
        public IDocumentSession OpenSession()
        {
            return _documentStore.LightweightSession(System.Data.IsolationLevel.ReadCommitted);
        }

        public IQuerySession QuerySession()
        {
            return _documentStore.QuerySession();
        }
    }
}
