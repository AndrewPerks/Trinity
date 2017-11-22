using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trinity.DataAccess.Interfaces;
using Trinity.Model;
using Trinity.Services.Interfaces;

namespace Trinity.Services.Concrete
{
    /// <summary>
    /// Title service that carries out the business transactions involving titles
    /// </summary>
    public class TitleService : ITitleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Title> Get(Expression<Func<Title, bool>> predicate = null, string includeProperties = "")
        {
            var titles = _unitOfWork.Repository<Title>().Get(predicate, includeProperties);
            return titles.ToList();
        }

        public Title GetById(int id)
        {
            var title = _unitOfWork.Repository<Title>().GetById(id);
            return title;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
