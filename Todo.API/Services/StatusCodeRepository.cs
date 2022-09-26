using Todo.API.DbContexts;
using Todo.API.Entities;
using System.Linq;

namespace Todo.API.Services
{
    public class StatusCodeRepository : IStatusCodeRepository
    {
        private DataContext _context;

        public StatusCodeRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<StatusCode>> GetAllStatusCodesAsync()
        {
            var listOfStatusCodes = await _context.StatusCodes
                        .OrderBy(p => p.Description).ToListAsync();

            return listOfStatusCodes;
        }

        public async Task<StatusCode?> GetStatusCodeAsync(string code)
        {
            var record =  await _context.StatusCodes.
                    FirstOrDefaultAsync(p => p.Code == code);

            return record;
        }


        public async Task<StatusCode?> InsertStatusCodeAsync(StatusCode statusCode)
        {
            var entry = await _context.StatusCodes.
                FirstOrDefaultAsync(p => p.Code == statusCode.Code);

            if (entry == null)
            {
                await _context.StatusCodes.AddAsync(statusCode);
                await _context.SaveChangesAsync();
                return statusCode;
            }

            return null;
        }


        public StatusCode? UpdateStatusCode(StatusCode statusCode)
        {
            var scRec = _context.StatusCodes
                                .FirstOrDefault(p => p.Code == statusCode.Code);
            if (scRec != null)
            {
                scRec.Description = statusCode.Description;
                _context.SaveChanges();

                return scRec;
            }

            return null;
        }

        public StatusCode? RemoveStatusCode(StatusCode statusCode)
        {
            var usedInTasks = _context.UserTasks
                .FirstOrDefault(p => p.StatusCode   == statusCode.Code);

            if (usedInTasks == null)
            {
                var entry = _context.StatusCodes
                    .FirstOrDefault(p => p.Code == statusCode.Code);

                if (entry != null)
                {
                    _context.StatusCodes.Remove(entry);
                    _context.SaveChanges();

                    return statusCode;
                }
            }

            return null;
        }
    }
}
