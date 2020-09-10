using leave_management.Contracts;
using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace leave_management.Respository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;
        // {get;}
        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LeaveType entity)
        {
            await _db.LeaveTypes.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            var leaveTypes = await _db.LeaveTypes.ToListAsync();
            return leaveTypes;
        }

        public async Task<LeaveType> FindById(int id)
        {
            // throw new NotImplementedException();
            var leaveType = await _db.LeaveTypes.FindAsync(id);
            // _db.LeaveTypes.FirstOrDefault();

            return leaveType;
        }

        public async Task<ICollection<LeaveType>> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isExists(int id)
        {
            var exists = await _db.LeaveTypes.AnyAsync(q=>q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            //throw new NotImplementedException();
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveType entity)
        {
            //throw new NotImplementedException();
            _db.LeaveTypes.Update(entity);
            return await Save();
        }
    }
}
