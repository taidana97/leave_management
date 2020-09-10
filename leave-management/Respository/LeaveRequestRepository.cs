﻿using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Respository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;
        // {get;}
        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LeaveRequest entity)
        {
            await _db.LeaveRequests.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            //throw new NotImplementedException();
            _db.LeaveRequests.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            //throw new NotImplementedException();
            var LeaveRequest = await _db.LeaveRequests
                .Include(q=>q.RequestingEmployee)
                .Include(q=>q.ApprovedBy)
                .Include(q=>q.LeaveType)
                .ToListAsync();
            return LeaveRequest;
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            // var LeaveHistory = _db.LeaveRequests.Find(id);
            // _db.LeaveTypes.FirstOrDefault();
            var LeaveHistory = await _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id == id);

            return LeaveHistory;
        }

        public async Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeId)
        {
            var leaveRequests = await FindAll();
            //_db.LeaveRequests
            //    .Include(q => q.RequestingEmployee)
            //    .Include(q => q.ApprovedBy)
            //    .Include(q => q.LeaveType)

            return leaveRequests.Where(q => q.RequestingEmployeeId == employeeId)
                    .ToList();
        }

        public async Task<bool> isExists(int id)
        {
            var exists = await _db.LeaveRequests.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            //throw new NotImplementedException();
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            //throw new NotImplementedException();
            _db.LeaveRequests.Update(entity);
            return await Save();
        }
    }
}
