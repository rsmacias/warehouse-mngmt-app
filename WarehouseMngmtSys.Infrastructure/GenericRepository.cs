using System.Linq.Expressions;

namespace warehouseManagementSystem.Infrastructure;

public abstract class GenericRepository<T> 
    : IRepository<T> where T : class {

        protected WarehouseContext context;

        public GenericRepository(WarehouseContext context) {
            this.context = context;
        }

        public T Add(T entity) {
            throw new NotImplementedException();            
        }
        
        public T Update(T entity) {
            throw new NotImplementedException();
        }
        
        public T Get(Guid id) {
            throw new NotImplementedException();
        }
        
        public IEnumerable<T> All() {
            throw new NotImplementedException();
        }
        
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) {
            throw new NotImplementedException();
        }

        public void SaveChanges() {
            throw new NotImplementedException();
        }
}
