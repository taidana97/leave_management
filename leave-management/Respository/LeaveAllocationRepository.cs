using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Respository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;
        // {get;}
        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            var allocaiton = await FindAll();
            return allocaiton.Where(q => 
                    q.EmployeeId == employeeid && 
                    q.LeaveTypeId == leavetypeid && 
                    q.Period == period
                )
                .Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            //throw new NotImplementedException();
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            //throw new NotImplementedException();
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            //throw new NotImplementedException();
            var leaveAllocations = await _db.LeaveAllocations
                .Include(q=>q.LeaveType)
                .Include(q=>q.Employee)
                .ToListAsync();
            return leaveAllocations;
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            var leaveAllocation = await _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id==id);
                
            // _db.LeaveTypes.FirstOrDefault();

            return leaveAllocation;
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string employeeId)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();

            return allocations.Where(q => q.EmployeeId == employeeId && q.Period == period)
                    .ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string employeeId, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();

            return allocations.FirstOrDefault(q =>
                    q.EmployeeId == employeeId &&
                    q.Period == period &&
                    q.LeaveTypeId == leavetypeid
                    );                    
        }

        public async Task<bool> isExists(int id)
        {
            var exists = await _db.LeaveAllocations.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            //throw new NotImplementedException();
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            //throw new NotImplementedException();
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
