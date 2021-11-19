using System;
using System.Threading.Tasks;
using PracticeAPI.Data;
using PracticeAPI.Models;

namespace PracticeAPI.Repositories
{
    public interface iUnitOfWork
    {
        Task CommitChangesAsync();

        GenericRepository<Product> ProductRepo { get; }
        GenericRepository<PrivateClient> PrivateClientRepo { get; }
        GenericRepository<PublicClient> PublicClientRepo { get; }
        GenericRepository<ParentA> ParentARepo { get; }
        GenericRepository<ParentB> ParentBRepo { get; }
        GenericRepository<ChildA> ChildARepo { get; }
        GenericRepository<ChildB> ChildBRepo { get; }        
    }

    public class UnitOfWork : iUnitOfWork
    {
        public ApiContext _context;
        private GenericRepository<Product> _productRepo;        
        private GenericRepository<PrivateClient> _privateClientRepo;
        private GenericRepository<PublicClient> _publicClientRepo;
        private GenericRepository<ParentA> _parentARepo;
        private GenericRepository<ParentB> _parentBRepo;
        private GenericRepository<ChildA> _childARepo;
        private GenericRepository<ChildB> _childBRepo;
        
        public UnitOfWork(ApiContext context)
        {
            _context = context;
        }

        public async Task CommitChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public GenericRepository<Product> ProductRepo
        {
            get
            {
                if(_productRepo == null)
                    _productRepo = new GenericRepository<Product>(_context);
                return _productRepo;
            }
        }

        public GenericRepository<PrivateClient> PrivateClientRepo 
        {
            get
            {
                if(_privateClientRepo == null)
                    _privateClientRepo = new GenericRepository<PrivateClient>(_context);
                return _privateClientRepo;
            }
        }

        public GenericRepository<PublicClient> PublicClientRepo
        {
            get
            {
                if(_publicClientRepo == null)
                    _publicClientRepo = new GenericRepository<PublicClient>(_context);
                return _publicClientRepo;
            }
        }

        public GenericRepository<ParentA> ParentARepo
        {
            get
            {
                if(_parentARepo == null)
                    _parentARepo = new GenericRepository<ParentA>(_context);
                return _parentARepo;
            }
        }

        public GenericRepository<ParentB> ParentBRepo
        { 
            get
            {
                if(_parentBRepo == null)
                   _parentBRepo = new GenericRepository<ParentB>(_context);
                return _parentBRepo;
            }
        }
        
        public GenericRepository<ChildA> ChildARepo
        { 
            get
            {
                if(_childARepo == null)
                   _childARepo = new GenericRepository<ChildA>(_context);
                return _childARepo;
            }
        }
        
        public GenericRepository<ChildB> ChildBRepo
        { 
            get
            {
                if(_childBRepo == null)
                   _childBRepo = new GenericRepository<ChildB>(_context);
                return _childBRepo;
            }
        }

    }
}